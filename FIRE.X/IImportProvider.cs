using FIRE.X.DL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FIRE.X
{
    public abstract class ImportProvider
    {
        public abstract string GetName();
        public abstract TransactionSource GetTransactionSource();
        public abstract Task<ImportResult<IImportModel>> GetRecords(Stream file, Action<ImportResult<IImportModel>> done, Action<int> progress);
    }
}
