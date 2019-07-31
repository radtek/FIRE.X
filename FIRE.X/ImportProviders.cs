using FIRE.X.Mintos.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FIRE.X
{
    public static class ImportProviders
    {
        private static List<IImportProvider> CreatedImportProviders = new List<IImportProvider>();
        public static void RegisterImportProviders()
        {
            CreatedImportProviders.Add(new MintosImportProvider());
        }

        public static IImportProvider GetImportProvider(string name)
        {
            return CreatedImportProviders.FirstOrDefault(p => p.GetName() == name);
        }
    }
}
