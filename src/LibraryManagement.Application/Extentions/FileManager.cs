using LibraryManagement.Application.Dtos;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.IO.Compression;

namespace LibraryManagement.Infrastructure.Extentions
{
    public static class FileManager
    {
        private const string _basePath = "wwwroot";

        private const string _uploadFolder = "uploads";

        public static string UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return "";
                }
                var directoryPath = Path.Combine(_basePath, _uploadFolder);
                Directory.CreateDirectory(directoryPath);
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(directoryPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var relativePath = filePath.Replace("wwwroot", "").Replace("\\", "/");
                return relativePath;
            }
            catch
            {
                return "";
            }
        }

        public static void RemoveFile(string path)
        {
            string filePath = path.Insert(0,"wwwroot").Replace("/", "\\");
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    return;
                }
            }
            else
            {
               return;
            }
        }

        public static FileResult GetFile(string path)
        {

            string filePath = path.Insert(0, "wwwroot").Replace("/", "\\");
            if (System.IO.File.Exists(filePath))
            {
                MemoryStream memoryStream = new MemoryStream();
                using (ZipArchive archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    ZipArchiveEntry entry = archive.CreateEntry(Path.GetFileName(filePath));

                    using (FileStream originalFileStream = System.IO.File.OpenRead(filePath))
                    using (Stream entryStream = entry.Open())
                    {
                        originalFileStream.CopyTo(entryStream);
                    }
                }

                // Reset the memory stream position to the beginning
                memoryStream.Seek(0, SeekOrigin.Begin);
                string mimeType = "application/zip";
                string fileName = Path.GetFileNameWithoutExtension(filePath) + ".zip";
                // Return the zipped file as a FileStreamResult
                return new FileResult(memoryStream, mimeType, fileName); 
            }
            else
            {
                return null;
            }
        }


    }
}
