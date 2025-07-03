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
        public static async Task<string?> SaveFileAsync(string FolderName, IFormFile? File)
        {

            if (File is null || File.Length == 0)
            {
                return null;
            }
            var extention = Path.GetExtension(File.FileName);
            //if(extention != ".png" && extention != ".jpg" && extention != ".jpeg")
            //{
            //    return null;
            //}
            await using var stream = File.OpenReadStream();
            if (!await ValidateByMagicNumberAsync(stream))
            {
                return null;
            }
            var FileName = FileNameConstructor(File.FileName);
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName);

            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);

            FilePath = Path.Combine(FilePath, FileName);



            using (var FS = new FileStream(FilePath, FileMode.Create,FileAccess.Write,FileShare.None,4096,useAsync:true))
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
            var FullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FilePath);

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

        private static async Task<bool> ValidateByMagicNumberAsync(Stream stream)
        {
            
            byte[] header = new byte[8];
            int bytesRead = await stream.ReadAsync(header, 0, header.Length);
            stream.Seek(0, SeekOrigin.Begin); 

           
            if (bytesRead >= 8 &&
                header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
            {
                return true;
            }
            if (bytesRead >= 3 &&
                header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            {
                return true;
            }

            return false;
           
        }

        private static string FileNameConstructor(string FileName)
            => $"{DateTime.UtcNow.ToString("ddMMyy")}-{Guid.NewGuid()}-{FileName}";

    }
}
