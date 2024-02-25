using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string folderName)
        {
            // 1. Get Located Folder Path
            //string folderPath = "D:\\Route\\Full stack -Route\\Back-end\\Practice\\07 MVC\\Assignment 05 MVC\\Demo Solution\\Demo.PL\\wwwroot\\files\\" + folderName;
            //string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\files\" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            // 2. Get File Name And Make it UNIQUE
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path
            string filePath =Path.Combine(folderPath, fileName);

            // 4. Save File As Streams [Data Per Time]
            using var fileStrem = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStrem);

            return fileName;
        }

        public static void DeleteFile(string fileName,string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
