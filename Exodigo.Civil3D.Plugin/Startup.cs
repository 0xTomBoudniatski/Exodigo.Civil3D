using Autodesk.AutoCAD.Runtime;

[assembly: ExtensionApplication(typeof(Exodigo.Civil3D.Plugin.Startup))]

namespace Exodigo.Civil3D.Plugin;

public class Startup : IExtensionApplication
{

    public void Initialize()
    {

    }

    public void Terminate()
    {

    }
}