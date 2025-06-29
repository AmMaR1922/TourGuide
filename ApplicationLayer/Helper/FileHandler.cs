using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Helper
{
    public static class FileHandler
    {
        public static async Task<string?> SaveFileAsync(string FolderName, IFormFile File)
        {

            if (File is null || File.Length == 0)
            {
                return null;
            }
            var FileName = FileNameConstructor(File.FileName);
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "ProtectedFiles", FolderName, FileName);

            var directoryPath = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var FS = new FileStream(FilePath, FileMode.Create))
            {
                await File.CopyToAsync(FS);
            }

            return $"{FolderName}/{FileName}";
        }

        public static bool DeleteFile(string FilePath)
        {
            if (FilePath.IsNullOrEmpty())
            {
                return false;
            }
            var FullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "ProtectedFiles", FilePath);

            if (File.Exists(FullFilePath))
            {
                try
                {
                    string TempFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}_{Path.GetFileName(FilePath)}");
                    File.Move(FullFilePath, TempFilePath);
                    File.Delete(TempFilePath);
                    return true;
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error deleting File: {ex.Message}");
                    return false;
                }
            }

            else { return false; }

        }

        private static string FileNameConstructor(string FileName)
            => $"{DateTime.UtcNow.ToString("ddMMyy")}-{Guid.NewGuid()}-{FileName}";

    }
}
