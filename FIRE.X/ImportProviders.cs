using System.Collections.Generic;
using System.Linq;

namespace FIRE.X
{
    public static class ImportProviders
    {
        private static List<ImportProvider> CreatedImportProviders = new List<ImportProvider>();
        public static void RegisterImportProviders()
        {
            CreatedImportProviders.Add(new Mintos.Import.MintosImportProvider<Mintos.Import.MintosImport>());
            CreatedImportProviders.Add(new Grupeer.Import.GrupeerImportProvider<Grupeer.Import.GrupeerImport>());
        }

        public static ImportProvider GetImportProvider(string name)
        {
            return CreatedImportProviders.FirstOrDefault(p => p.GetName() == name);
        }

        public static string[] ImportProviderNames => CreatedImportProviders.Select(ip => ip.GetName()).ToArray();
    }
}
