using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FIRE.X.Mintos.Import
{
    public class MintosImportProvider : IImportProvider<IImportModel>
    {
        public string GetName() => "Mintos";

        public List<IImportModel> GetRecords<IImportModel>(Stream file)
        {
            using (var reader = new StreamReader(file))
            using (var csvReader = new CsvReader(reader))
            {
                return csvReader.GetRecords<IImportModel>().ToList();
            }
        }
    }
}
