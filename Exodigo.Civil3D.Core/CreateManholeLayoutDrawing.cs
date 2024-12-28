using Exodigo.Civil3D.Contracts.Models;
using Exodigo.Civil3D.Core.Models;
using Exodigo.Civil3D.Utilities;

namespace Exodigo.Civil3D.Core;

public class CreateManholeLayoutDrawing
{
    public IEnumerable<IManholeSurveyData> GetManholeSurveyData()
    {
        var handleCsvUtility = new HandleCsvUtility();
        return handleCsvUtility.LoadManholeSurveyData();
    }

    public IEnumerable<IManhole> ParseManholeSurveyData(IEnumerable<IManholeSurveyData> manholeSurveyData)
    {
        var result = new List<ManholeModel>();

        foreach (var item in manholeSurveyData.ToList())
        {
            var manhole = new ManholeModel(item);
            result.Add(manhole);
        }

        return result;
    }

    public ICircle ExtractCoverData(IManhole manhole, double offset = 0)
    {
        var circle = new CircleModel()
        {

            Radius = manhole.CoverDiameter / 2,

            Center = new XyzPointModel()
            {
                X = offset,
            }
        };

        return circle;
    }

    public IEnumerable<(ICircle circle, int position)> ExtractDuctsData(IManhole manhole, double offset = 0)
    {
        var result = new List<(ICircle, int)>();

        foreach (var duct in manhole.Ducts)
        {
            if (double.IsNaN(duct.Diameter)) continue;
            if (double.IsNaN(duct.DistanceFromFloor)) continue;
            if (double.IsNaN(duct.DistanceFromRight)) continue;

            var circle = new CircleModel()
            {
                Center = GetDuctCenterPoint(manhole, duct, offset),
                Radius = duct.Diameter / 2
            };

            result.Add((circle, duct.Position));
        }

        return result;
    }

    public (IRectangle horz, IRectangle vert) ExtractStructureData(IManhole manhole, double offset = 0)
    {

        var horizontal = GetHorizontalStructure(manhole, offset);
        var vertical = GetVerticalStructure(manhole, offset);

        return (horizontal, vertical);
    }

    public (IXyzPoint p1, IXyzPoint p2, IXyzPoint p3, IXyzPoint p4) GetCircleQuadrantPoints(ICircle myCircle)
    {
        var centerPoint = myCircle.Center;

        var rightPoint = new XyzPointModel()
        {
            X = centerPoint.X + myCircle.Radius,
            Y = centerPoint.Y,
        };

        var leftPoint = new XyzPointModel()
        {
            X = centerPoint.X - myCircle.Radius,
            Y = centerPoint.Y,
        };

        var upperPoint = new XyzPointModel()
        {
            X = centerPoint.X,
            Y = centerPoint.Y + myCircle.Radius,
        };

        var lowerPoint = new XyzPointModel()
        {
            X = centerPoint.X,
            Y = centerPoint.Y - myCircle.Radius,
        };

        return (upperPoint, rightPoint, lowerPoint, leftPoint);
    }

    public (IXyzPoint p1, IXyzPoint p2) GetDuctVerticalDimPoints(IManhole manhole, ICircle myCircle, int position)
    {
        var duct = manhole.Ducts.FirstOrDefault(x=>x.Position == position);
        if (duct is null) throw new ArgumentNullException(nameof(duct));
        
        var statPoint = new XyzPointModel();
        var endPoint = new XyzPointModel();

        switch (position)
        {
            case 1:
                // Logic to get duct 1 dim distance starting point
                statPoint.X = myCircle.Center.X;
                statPoint.Y = myCircle.Center.Y - myCircle.Radius;
                // Logic to get duct 1 dim distance ending point
                endPoint.X = myCircle.Center.X;
                endPoint.Y = myCircle.Center.Y - myCircle.Radius - duct.DistanceFromFloor;
                break;

            case 2:
                // Logic to get duct 2 dim distance starting point
                statPoint.X = myCircle.Center.X;
                statPoint.Y = myCircle.Center.Y + myCircle.Radius;
                // Logic to get duct 2 dim distance ending point
                endPoint.X = myCircle.Center.X;
                endPoint.Y = myCircle.Center.Y + myCircle.Radius + duct.DistanceFromRight;
                break;

            case 3:
                // Logic to get duct 3 dim distance starting point
                statPoint.X = myCircle.Center.X;
                statPoint.Y = myCircle.Center.Y + myCircle.Radius;
                // Logic to get duct 3 dim distance ending point
                endPoint.X = myCircle.Center.X;
                endPoint.Y = myCircle.Center.Y + myCircle.Radius + duct.DistanceFromFloor;
                break;

            case 4:
                // Logic to get duct 4 dim distance starting point
                statPoint.X = myCircle.Center.X;
                statPoint.Y = myCircle.Center.Y + myCircle.Radius;
                // Logic to get duct 4 dim distance ending point
                endPoint.X = myCircle.Center.X;
                endPoint.Y = myCircle.Center.Y + myCircle.Radius + duct.DistanceFromRight;
                break;

            default:
                throw new ArgumentOutOfRangeException("Duct Number must be within range 1-4");
        }

        return (statPoint, endPoint);
    }

    public (IXyzPoint p1, IXyzPoint p2) GetDuctHorizontalDimPoints(IManhole manhole, ICircle myCircle, int position)
    {

        var duct = manhole.Ducts.FirstOrDefault(x => x.Position == position);
        if (duct is null) throw new ArgumentNullException(nameof(duct));

        var statPoint = new XyzPointModel();
        var endPoint = new XyzPointModel();

        switch (position)
        {
            case 1:
                // Logic to get duct 1 dim distance starting point
                statPoint.X = myCircle.Center.X + myCircle.Radius;
                statPoint.Y = myCircle.Center.Y;
                // Logic to get duct 1 dim distance ending point
                endPoint.X = myCircle.Center.X + myCircle.Radius + duct.DistanceFromRight;
                endPoint.Y = myCircle.Center.Y;
                break;

            case 2:
                // Logic to get duct 2 dim distance starting point
                statPoint.X = myCircle.Center.X - myCircle.Radius;
                statPoint.Y = myCircle.Center.Y;
                // Logic to get duct 2 dim distance ending point
                endPoint.X = myCircle.Center.X - myCircle.Radius - duct.DistanceFromFloor;
                endPoint.Y = myCircle.Center.Y;
                break;

            case 3:
                // Logic to get duct 3 dim distance starting point
                statPoint.X = myCircle.Center.X + myCircle.Radius;
                statPoint.Y = myCircle.Center.Y;
                // Logic to get duct 3 dim distance ending point
                endPoint.X = myCircle.Center.X + myCircle.Radius + duct.DistanceFromRight;
                endPoint.Y = myCircle.Center.Y;
                break;

            case 4:
                // Logic to get duct 4 dim distance starting point
                statPoint.X = myCircle.Center.X + myCircle.Radius;
                statPoint.Y = myCircle.Center.Y;
                // Logic to get duct 4 dim distance ending point
                endPoint.X = myCircle.Center.X + myCircle.Radius + duct.DistanceFromFloor;
                endPoint.Y = myCircle.Center.Y;
                break;

            default:
                throw new ArgumentOutOfRangeException("Duct Number must be within range 1-4");
        }

        return (statPoint, endPoint);
    }

    private static XyzPointModel GetDuctCenterPoint(IManhole manhole, IDuct duct, double offset)
    {
        var result = new XyzPointModel();

        double widthMidPoint = manhole.Width / 2;
        double heightMidPoint = manhole.Height / 2;
        double ductRadius = duct.Diameter / 2;

        switch (duct.Position)
        {
            case 1:
                // Logic to get duct 1 center
                result.X = offset + widthMidPoint - duct.DistanceFromRight - ductRadius;
                result.Y = heightMidPoint + duct.DistanceFromFloor + ductRadius;
                break;

            case 2:
                // Logic to get duct 2 center
                result.X = offset + widthMidPoint + duct.DistanceFromFloor + ductRadius;
                result.Y = heightMidPoint - ductRadius - duct.DistanceFromRight;
                break;

            case 3:
                // Logic to get duct 3 center
                result.X = offset + widthMidPoint - duct.DistanceFromRight - ductRadius;
                result.Y = 0 - heightMidPoint - duct.DistanceFromFloor - ductRadius;
                break;

            case 4:
                // Logic to get duct 4 center
                result.X = offset - widthMidPoint - duct.DistanceFromFloor - ductRadius;
                result.Y = heightMidPoint - duct.DistanceFromRight - ductRadius;
                break;

            default:
                throw new ArgumentOutOfRangeException("Duct Number must be within range 1-4");
        }

        return result;
    }

    private static RectangleModel GetHorizontalStructure(IManhole manhole, double offset)
    {
        double widthMidPoint = manhole.Width / 2;
        double heightMidPoint = manhole.Height / 2;
        
        var BottomLeftPoint = new XyzPointModel()
        {
            X = offset - widthMidPoint - manhole.Depth,
            Y = 0 - heightMidPoint,
        };

        var BottomRightPoint = new XyzPointModel()
        {
            X = offset + widthMidPoint + manhole.Depth,
            Y = 0 - heightMidPoint,
        };

        var TopLeftPoint = new XyzPointModel()
        {
            X = offset - widthMidPoint - manhole.Depth,
            Y = 0 + heightMidPoint,
        };

        var TopRightPoint = new XyzPointModel()
        {
            X = offset + widthMidPoint + manhole.Depth,
            Y = 0 + heightMidPoint,
        };


        return new RectangleModel(BottomLeftPoint, TopRightPoint, TopLeftPoint, BottomRightPoint);

    }

    private static RectangleModel GetVerticalStructure(IManhole manhole, double offset)
    {
        double widthMidPoint = manhole.Width / 2;
        double heightMidPoint = manhole.Height / 2;

        var BottomLeftPoint = new XyzPointModel()
        {
            X = offset - widthMidPoint,
            Y = 0 - heightMidPoint - manhole.Depth,
        };

        var BottomRightPoint = new XyzPointModel()
        {
            X = offset + widthMidPoint,
            Y = 0 - heightMidPoint - manhole.Depth,
        };

        var TopLeftPoint = new XyzPointModel()
        {
            X = offset - widthMidPoint,
            Y = 0 + heightMidPoint + manhole.Depth,
        };

        var TopRightPoint = new XyzPointModel()
        {
            X = offset + widthMidPoint,
            Y = 0 + heightMidPoint + manhole.Depth,
        };


        return new RectangleModel(BottomLeftPoint, TopRightPoint, TopLeftPoint, BottomRightPoint);

    }

}
