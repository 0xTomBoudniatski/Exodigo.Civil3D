using Exodigo.Civil3D.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exodigo.Civil3D.Core.Models;

public class RectangleModel : IRectangle
{
    private readonly XyzPointModel _bottomLeft;
    private readonly XyzPointModel _topRight;
    private readonly XyzPointModel _topLeft;
    private readonly XyzPointModel _bottomRight;

    public RectangleModel(XyzPointModel BottomLeft, XyzPointModel topRight, XyzPointModel topLeft, XyzPointModel bottomRight)
    {
        _bottomLeft = BottomLeft;
        _topRight = topRight;
        _topLeft = topLeft;
        _bottomRight = bottomRight;
    }

    public IXyzPoint BottomLeft => _bottomLeft;

    public IXyzPoint BottomRight => _bottomRight;

    public IXyzPoint TopRight => _topRight;

    public IXyzPoint TopLeft => _topLeft;
}
