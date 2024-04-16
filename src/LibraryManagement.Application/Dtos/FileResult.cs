using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Dtos
{
    public class  FileResult
    {
        public FileResult(MemoryStream file, string mimeType, string fileName)
        {
            MemoryStream = file;
            MimeType = mimeType;
            FileName = fileName;
        }

        public MemoryStream MemoryStream { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }
}
