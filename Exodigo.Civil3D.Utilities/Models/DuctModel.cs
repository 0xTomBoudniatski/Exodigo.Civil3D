using Exodigo.Civil3D.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Utilities.Models;

public class DuctModel : IDuct
{
    public int Position { get; set; }

    public double Diameter { get; set; }

    public double DistanceFromFloor { get; set; }

    public double DistanceFromRight { get; set; }

}
