using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;

using Exodigo.Civil3D.Contracts.Models;

using CoreApplication = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace Exodigo.Civil3D.Plugin.Functions;

public static class PlaceBlock
{
    public static void PlaceNorthArrowRotatedByAzimuth(IXyzPoint point, double rotationAngle = 0, double scale = 1.0)
    {
        // Set location point and rotation
        double radians = rotationAngle * Math.PI / 180.0;
        var location = new Point3d(point.X, point.Y, point.Z);

        // Get the current document and database
        Document acDoc = CoreApplication.DocumentManager.MdiActiveDocument;
        Database acCurDb = acDoc.Database;

        using Transaction trans = acCurDb.TransactionManager.StartTransaction();
        
        try
        {
            BlockTableRecord modelSpace = (BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(acCurDb),OpenMode.ForWrite);

            BlockReference arrow = new BlockReference(location, ((BlockTable)trans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead))["AeccArrow"])
            {
                Rotation = radians,
                ScaleFactors = new Scale3d(scale)
            };

            // Add object to the BlockTableRecord
            modelSpace.AppendEntity(arrow);
            trans.AddNewlyCreatedDBObject(arrow, true);

            // Commit the transaction
            trans.Commit();

            // Regenerate the document to update the display
            acDoc.Editor.Regen();
        }
        catch
        {
            trans.Abort();
            throw;
        }
    }
}
