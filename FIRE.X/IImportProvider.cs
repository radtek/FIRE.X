using System.Collections.Generic;
using System.IO;

namespace FIRE.X
{
    public interface IImportProvider<T>
    {
        string GetName();
        List<T> GetRecords<T>(Stream file);
    }
}
