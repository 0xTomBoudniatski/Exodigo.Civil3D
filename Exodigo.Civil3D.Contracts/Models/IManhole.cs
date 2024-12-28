using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Contracts.Models;

public interface IManhole
{
    public long Id { get; }
    public string Type { get; }
    public double CoverDiameter { get; }
    public string Material { get; }
    public double Depth { get; }
    public double Width { get; }
    public double Height { get; }
    public double Azimuth { get; }
    public IEnumerable<IDuct> Ducts { get; }
}
