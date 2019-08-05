using FIRE.X.Mintos.Import;
using System.Collections.Generic;
using System.Linq;

namespace FIRE.X
{
    public static class ImportProviders
    {
        private static System.Collections.Generic.List<IImportProvider> CreatedImportProviders = new List<IImportProvider>();
        public static void RegisterImportProviders()
        {
            CreatedImportProviders.Add(new MintosImportProvider());
        }

        public static IImportProvider GetImportProvider(string name)
        {
            return CreatedImportProviders.FirstOrDefault(p => p.GetName() == name);
        }

        public static string[] ImportProviderNames => CreatedImportProviders.Select(ip => ip.GetName()).ToArray();
    }
}
