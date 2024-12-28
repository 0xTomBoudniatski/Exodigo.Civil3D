using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Contracts.Models;

public interface IDuct
{
    int Position { get; }
    double Diameter { get; }
    double DistanceFromFloor { get; }
    double DistanceFromRight { get; }
}
