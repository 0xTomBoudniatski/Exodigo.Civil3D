using Exodigo.Civil3D.Contracts.Models;
using Exodigo.Civil3D.Utilities.Models;

namespace Exodigo.Civil3D.Core.Models;

public class ManholeModel : IManhole
{
    // Private backing fields
    private readonly long _id;
    private readonly string _type;
    private readonly double _coverDiameter;
    private readonly string _material;
    private readonly double _depth;
    private readonly double _width;
    private readonly double _height;
    private readonly double _azimuth;
    private readonly IEnumerable<IDuct> _ducts;

    // Constructor
    public ManholeModel(IManholeSurveyData manholeSurveyData)
    {
        ArgumentNullException.ThrowIfNull(manholeSurveyData);

        // Initialize private fields using data from the survey
        _id = manholeSurveyData.Id;
        _type = manholeSurveyData.Type;
        _coverDiameter = manholeSurveyData.CoverDiameter;
        _material = manholeSurveyData.Material;
        _depth = manholeSurveyData.Depth;
        _width = manholeSurveyData.Width;
        _height = manholeSurveyData.Height;
        _azimuth = manholeSurveyData.Azimuth;
        _ducts = GetDucts(manholeSurveyData);
    }

    // Private Methods
    private static List<IDuct> GetDucts(IManholeSurveyData manholeSurveyData)
    {
        var ducts = new List<IDuct>()
        {
            new DuctModel()
            {
                Position = 1,
                Diameter = manholeSurveyData?.Duct1Diameter ?? double.NaN,
                DistanceFromFloor = manholeSurveyData?.Duct1DistanceFromFloor ?? double.NaN,
                DistanceFromRight = manholeSurveyData?.Duct1DistanceFromRight ?? double.NaN,
            },

            new DuctModel()
            {
                Position = 2,
                Diameter = manholeSurveyData?.Duct2Diameter ?? double.NaN,
                DistanceFromFloor = manholeSurveyData?.Duct2DistanceFromFloor ?? double.NaN,
                DistanceFromRight = manholeSurveyData?.Duct2DistanceFromRight ?? double.NaN,
            },

            new DuctModel()
            {
                Position = 3,
                Diameter = manholeSurveyData?.Duct3Diameter ?? double.NaN,
                DistanceFromFloor = manholeSurveyData?.Duct3DistanceFromFloor ?? double.NaN,
                DistanceFromRight = manholeSurveyData?.Duct3DistanceFromRight ?? double.NaN,
            },

            new DuctModel()
            {
                Position = 4,
                Diameter = manholeSurveyData?.Duct4Diameter ?? double.NaN,
                DistanceFromFloor = manholeSurveyData?.Duct4DistanceFromFloor ?? double.NaN,
                DistanceFromRight = manholeSurveyData?.Duct4DistanceFromRight ?? double.NaN,
            }
        };

        return ducts;

    }

    // Public properties
    public long Id => _id;
    public string Type => _type;
    public double CoverDiameter => _coverDiameter;
    public string Material => _material;
    public double Depth => _depth;
    public double Width => _width;
    public double Height => _height;
    public double Azimuth => _azimuth;
    public IEnumerable<IDuct> Ducts => _ducts;
}
