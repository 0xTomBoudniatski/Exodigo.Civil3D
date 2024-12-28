
using Exodigo.Civil3D.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Core.Models;

public class XyzPointModel : IXyzPoint
{
    public double X { get; set; } = 0;

    public double Y { get; set; } = 0;

    public double Z { get; set; } = 0;
}
