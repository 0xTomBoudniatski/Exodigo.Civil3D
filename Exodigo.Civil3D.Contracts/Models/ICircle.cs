using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Contracts.Models;

public interface ICircle
{
    double Radius { get; }

    IXyzPoint Center { get; }
}
