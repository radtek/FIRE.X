using FIRE.X.Mintos.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FIRE.X
{
    public static class ImportProviders
    {
        private static List<IImportProvider<IImportModel>> CreatedImportProviders = new List<IImportProvider<IImportModel>>();
        public static void RegisterImportProviders()
        {
            CreatedImportProviders.Add(new MintosImportProvider());
        }

        public static List<T> GetRecords<T>(string importProivderName, Stream stream) where T: IImportModel
        {
            var importProvider = CreatedImportProviders.FirstOrDefault(p => p.GetName() == importProivderName);

            if (importProvider == null)
                throw new NotImplementedException($"Could not find import provider with the name {importProivderName}");

            return importProvider.GetRecords<T>(stream);
        }
    }
}
