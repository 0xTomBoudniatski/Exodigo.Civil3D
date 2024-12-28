namespace Exodigo.Civil3D.Contracts.Models;

public interface IManholeSurveyData
{
    public long Id { get; }
    public string Type { get; }
    public double CoverDiameter { get; }
    public string Material { get; }
    public double Depth { get; }
    public double Width { get; }
    public double Height { get; }
    public double Azimuth { get; }
    public double? Duct1Diameter { get; }
    public double? Duct1DistanceFromFloor { get; }
    public double? Duct1DistanceFromRight { get; }
    public double? Duct2Diameter { get; }
    public double? Duct2DistanceFromFloor { get; }
    public double? Duct2DistanceFromRight { get; }
    public double? Duct3Diameter { get; }
    public double? Duct3DistanceFromFloor { get; }
    public double? Duct3DistanceFromRight { get; }
    public double? Duct4Diameter { get; }
    public double? Duct4DistanceFromFloor { get; }
    public double? Duct4DistanceFromRight { get; }
}
