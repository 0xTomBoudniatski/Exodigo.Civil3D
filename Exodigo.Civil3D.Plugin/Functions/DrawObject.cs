using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;

using Exodigo.Civil3D.Contracts.Models;

using CoreApplication = Autodesk.AutoCAD.ApplicationServices.Core.Application;


namespace Exodigo.Civil3D.Plugin.Functions;

public class DrawObject
{
    public static void CircleByDiameter(ICircle myCircle, bool addHatch = false)
    {
        // Set Center Point Location and Radius
        var center = new Point3d(myCircle.Center.X, myCircle.Center.Y, myCircle.Center.Z);
        var radius = myCircle.Radius;

        // Get the current document and database
        Document acDoc = CoreApplication.DocumentManager.MdiActiveDocument;
        Database acCurDb = acDoc.Database;

        // Start a transaction
        using Transaction tr = acCurDb.TransactionManager.StartTransaction();

        // Open the Block Table for write
        BlockTable? blockTable = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
        ArgumentNullException.ThrowIfNull(blockTable);

        BlockTableRecord? blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
        ArgumentNullException.ThrowIfNull(blockTableRecord);

        // Create a new circle
        var circle = new Circle(center, Vector3d.ZAxis, radius);

        // Add object to the BlockTableRecord
        _ = blockTableRecord.AppendEntity(circle);
        tr.AddNewlyCreatedDBObject(circle, true);

        if (addHatch)
        {
            // Create a new hatch
            var hatch = new Hatch();

            // Add object to the BlockTableRecord
            blockTableRecord.AppendEntity(hatch);
            tr.AddNewlyCreatedDBObject(hatch, true);

            // Set hatch properties
            hatch.SetHatchPattern(HatchPatternType.PreDefined, "SOLID"); // Solid fill
            hatch.Associative = true;
            hatch.Color = Color.FromRgb(255, 0, 0);

            // Create a hatch loop for the circle
            var ids = new ObjectIdCollection { circle.ObjectId };
            hatch.AppendLoop(HatchLoopTypes.Default, ids);
            hatch.EvaluateHatch(true);
        }

        // Commit the changes
        tr.Commit();

        // Regenerate the document to update the display
        acDoc.Editor.Regen();

    }

    public static void RectangleByCorners(IRectangle myRectangle)
    {
        // Set Corner Points Locations
        var point1 = new Point2d(myRectangle.BottomLeft.X, myRectangle.BottomLeft.Y);
        var point2 = new Point2d(myRectangle.BottomRight.X, myRectangle.BottomRight.Y);
        var point3 = new Point2d(myRectangle.TopRight.X, myRectangle.TopRight.Y);
        var point4 = new Point2d(myRectangle.TopLeft.X, myRectangle.TopLeft.Y); 

        // Get the current document and database
        Document acDoc = CoreApplication.DocumentManager.MdiActiveDocument;
        Database acCurDb = acDoc.Database;

        // Start a transaction
        using Transaction tr = acCurDb.TransactionManager.StartTransaction();

        // Open the Block Table for write
        BlockTable? blockTable = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
        ArgumentNullException.ThrowIfNull(blockTable);

        BlockTableRecord? blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
        ArgumentNullException.ThrowIfNull(blockTableRecord);

        // Create the rectangle as a Polyline
        var rectPolyline = new Polyline(4);
        rectPolyline.AddVertexAt(0, point1, 0, 0, 0); // Bottom-left
        rectPolyline.AddVertexAt(1, point2, 0, 0, 0); // Bottom-right
        rectPolyline.AddVertexAt(2, point3, 0, 0, 0); // Top-right
        rectPolyline.AddVertexAt(3, point4, 0, 0, 0); // Top-left
        rectPolyline.Closed = true; // Close the polyline to form a rectangle

        // Add object to the BlockTableRecord
        _ = blockTableRecord.AppendEntity(rectPolyline);
        tr.AddNewlyCreatedDBObject(rectPolyline, true);

        // Commit the transaction
        tr.Commit();

        // Regenerate the document to update the display
        acDoc.Editor.Regen();

    }

    public static void LinearDimension(IXyzPoint startPoint, IXyzPoint endPoint, double textSize = 0.1, double offset = 0, bool isVertical = false)
    {
        // Set starting and ending points
        var pt1 = new Point3d(startPoint.X, startPoint.Y, startPoint.Z);
        var pt2 = new Point3d(endPoint.X, endPoint.Y, endPoint.Z);

        // Get the current document and database
        Document acDoc = CoreApplication.DocumentManager.MdiActiveDocument;
        Database acCurDb = acDoc.Database;

        // Start a transaction
        using Transaction tr = acCurDb.TransactionManager.StartTransaction();

        // Open the Block Table for write
        BlockTable? blockTable = tr.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
        ArgumentNullException.ThrowIfNull(blockTable);

        BlockTableRecord? blockTableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
        ArgumentNullException.ThrowIfNull(blockTableRecord);

        // Calculate offset point based on the orientation
        Point3d pt3;
        if (isVertical)
        {
            // For vertical dimensions, offset along the X-axis
            pt3 = new Point3d(Math.Max(pt1.X, pt2.X) + offset, pt1.Y, 0.0);
        }
        else
        {
            // For horizontal dimensions, offset along the Y-axis
            pt3 = new Point3d(pt1.X, Math.Max(pt1.Y, pt2.Y) + offset, 0.0);
        }

        // Calculate the rotation angle based on the orientation
        double rotationAngle = isVertical ? Math.PI / 2 : 0.0;

        // Create a new RotatedDimension
        var dim = new RotatedDimension(rotationAngle, pt1, pt2, pt3, "", acCurDb.Dimstyle);

        // Set dimension variables directly on the dimension object
        dim.Dimtxt = textSize;      // Text size
        dim.Dimasz = 0.03;          // Arrow size
        dim.Dimexe = 0.01;          // Extension line extension
        dim.Dimtad = 1;             // Position text above dimension line
        dim.Dimgap = 0.02;          // Text offset distance
        dim.Dimtih = false;         // Text aligned with dimension line
        dim.Dimtofl = true;         // Dimension line is drawn between the extension lines

        dim.TransformBy(acDoc.Editor.CurrentUserCoordinateSystem);

        // Add object to the BlockTableRecord
        _ = blockTableRecord.AppendEntity(dim);
        tr.AddNewlyCreatedDBObject(dim, true);

        // Commit the transaction
        tr.Commit();

        // Regenerate the document to update the display
        acDoc.Editor.Regen();
    }
}
