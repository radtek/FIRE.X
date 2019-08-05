using FIRE.X.DL;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FIRE.X
{
    public interface IImportProvider
    {
        string GetName();
        TransactionSource GetTransactionSource();
        Task<ImportResult<T>> GetRecords<T>(Stream file, Action<ImportResult<T>> done, Action<int> progress) where T : IImportModel;
    }
}
