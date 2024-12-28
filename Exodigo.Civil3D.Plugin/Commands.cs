using Autodesk.AutoCAD.Runtime;
using Exodigo.Civil3D.Core;
using Exodigo.Civil3D.Plugin.Functions;

[assembly: CommandClass(typeof(Exodigo.Civil3D.Plugin.Commands))]

namespace Exodigo.Civil3D.Plugin;

public class Commands
{
    [CommandMethod(nameof(CreateManholeLayoutDrawing))]
    public void CreateManholeLayoutDrawing()
    {
        double baseOffset = 0;
        double drawingOffset = 5;

        var core = new CreateManholeLayoutDrawing();

        var manholeSurveyData = core.GetManholeSurveyData();
        var manholeDrawingDataSet = core.ParseManholeSurveyData(manholeSurveyData);

        foreach (var manholeDrawingData in manholeDrawingDataSet)
        {
            var cover = core.ExtractCoverData(manholeDrawingData, baseOffset);
            var diameterDimPoints = core.GetCircleQuadrantPoints(cover);
            
            DrawObject.CircleByDiameter(cover, addHatch: true);
            DrawObject.LinearDimension(diameterDimPoints.p2, diameterDimPoints.p4, textSize: 0.04);

            var ducts = core.ExtractDuctsData(manholeDrawingData, baseOffset);
            ducts.ToList().ForEach(duct =>
            {
                var diameterDimPoints = core.GetCircleQuadrantPoints(duct.circle);
                var horizontalDimPoints = core.GetDuctHorizontalDimPoints(manholeDrawingData, duct.circle, duct.position);
                var verticalDimPoints = core.GetDuctVerticalDimPoints(manholeDrawingData, duct.circle, duct.position);

                DrawObject.CircleByDiameter(duct.circle);
                // Draw horizontal dims
                DrawObject.LinearDimension(diameterDimPoints.p2, diameterDimPoints.p4, textSize: 0.01, offset: 0.25);
                DrawObject.LinearDimension(horizontalDimPoints.p1, horizontalDimPoints.p2, textSize: 0.01, offset: 0.25);
                // Draw vertical dims
                DrawObject.LinearDimension(diameterDimPoints.p1, diameterDimPoints.p3, textSize: 0.01, offset: 0.25, isVertical: true);
                DrawObject.LinearDimension(verticalDimPoints.p1, verticalDimPoints.p2, textSize: 0.01, offset: 0.25, isVertical: true);

            });

            var walls = core.ExtractStructureData(manholeDrawingData, baseOffset);

            DrawObject.RectangleByCorners(walls.horz);
            DrawObject.RectangleByCorners(walls.vert);
            DrawObject.LinearDimension(walls.vert.TopLeft, walls.vert.TopRight, textSize: 0.04, offset: 0.2);
            DrawObject.LinearDimension(walls.vert.TopLeft, walls.horz.TopRight, textSize: 0.04, offset: 0.2, isVertical: true);
            DrawObject.LinearDimension(walls.horz.TopRight, walls.horz.BottomRight, textSize: 0.04, offset: 0.2, isVertical: true);
            PlaceBlock.PlaceNorthArrowRotatedByAzimuth(walls.vert.TopRight, manholeDrawingData.Azimuth, scale: 0.5);


            baseOffset += drawingOffset;
        }
    }
}
