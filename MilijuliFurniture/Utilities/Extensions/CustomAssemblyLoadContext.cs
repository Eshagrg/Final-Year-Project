using System.Reflection;
using System.Runtime.Loader;
using System.Xml.Linq;

namespace PointOfSale.Utilities.Extensions
{
    // Create a custom implementation of the "AssemblyLoadContext" class
    public class CustomAssemblyLoadContext: AssemblyLoadContext
    {
        // Load an unmanaged library from the specified path and return a pointer to it
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        // Override the "LoadUnmanagedDll" method to load an unmanaged DLL from the specified path and return a pointer to it
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            // Call the "LoadUnmanagedDllFromPath" method with the specified name and return the result
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }
        // Override the "Load" method to load a managed assembly from the specified name
        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Throw a "NotImplementedException" to indicate that this method is not implemented in this class
            throw new NotImplementedException();
        }
    }
}
