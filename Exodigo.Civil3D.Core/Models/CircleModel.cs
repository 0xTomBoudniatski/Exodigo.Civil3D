using Exodigo.Civil3D.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Core.Models;

public class CircleModel : ICircle
{
    public required double Radius { get; set; }

    public required IXyzPoint Center { get; set; }
}
