using CsvHelper.Configuration.Attributes;
using Exodigo.Civil3D.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Exodigo.Civil3D.Utilities.Models;

public class ManholeSurveyDataModel : IManholeSurveyData
{
    [Name("id")]
    public long Id { get; set; }

    [Name("type")]
    public string Type { get; set; } = string.Empty;

    [Name("cover diameter")]
    public double CoverDiameter { get; set; }

    [Name("material")]
    public string Material { get; set; } = string.Empty;

    [Name("depth")]
    public double Depth { get; set; }

    [Name("width")]
    public double Width { get; set; }

    [Name("height")]
    public double Height { get; set; }

    [Name("azimuth")]
    public double Azimuth { get; set; }

    [Name("duct_1_diameter")]
    public double? Duct1Diameter { get; set; }

    [Name("duct_1_distance_from_floor")]
    public double? Duct1DistanceFromFloor { get; set; }

    [Name("duct_1_distance_from_right")]
    public double? Duct1DistanceFromRight { get; set; }

    [Name("duct_2_diameter")]
    public double? Duct2Diameter { get; set; }

    [Name("duct_2_distance_from_floor")]
    public double? Duct2DistanceFromFloor { get; set; }

    [Name("duct_2_distance_from_right")]
    public double? Duct2DistanceFromRight { get; set; }

    [Name("duct_3_diameter")]
    public double? Duct3Diameter { get; set; }

    [Name("duct_3_distance_from_floor")]
    public double? Duct3DistanceFromFloor { get; set; }

    [Name("duct_3_distance_from_right")]
    public double? Duct3DistanceFromRight { get; set; }

    [Name("duct_4_diameter")]
    public double? Duct4Diameter { get; set; }

    [Name("duct_4_distance_from_floor")]
    public double? Duct4DistanceFromFloor { get; set; }

    [Name("duct_4_distance_from_right")]
    public double? Duct4DistanceFromRight { get; set; }
}
