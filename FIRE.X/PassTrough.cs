using System.IO;

namespace FIRE.X
{
    struct PassThrough
    {
        public string ImportProvider { get; set; }
        public Stream File { get; set; }
    }
}
