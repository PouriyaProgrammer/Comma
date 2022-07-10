using Microsoft.AspNetCore.Http;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace Application
{
    public static class Tools
    {
        public static string Upload(this IFormFile? file, string path, string fileToRemove = "")
        {
            if (file == null)
                return "";

            string fileName = $"{DateTime.Now.ToFileName()}-{file.FileName}";
            string directoryPath = Path.Combine("wwwroot", path);
            
            if(!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, fileName);

            using (var stream = File.Create(filePath))
            {
                file.CopyTo(stream);
            }

            if(!string.IsNullOrWhiteSpace(fileToRemove) 
                || !string.IsNullOrEmpty(fileToRemove))
            {
                string fileToRemovePath = Path.Combine("wwwroot", fileToRemove);
                if(File.Exists(fileToRemovePath))
                    File.Delete(fileToRemovePath);
            }

            return Path.Combine(path, fileName);
        }

        public static string ToFileName(this DateTime dateTime)
        {
            return $"{dateTime.Year}-{dateTime.Month}-{dateTime.Day}-" +
                $"{dateTime.Hour}-{dateTime.Minute}-{dateTime.Second}-" +
                $"{dateTime.Millisecond}";
        }
        

        public static string ToPersianDate(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            return $"{pc.GetYear(date):0000}/" +
                $"{pc.GetMonth(date):00}/" +
                $"{pc.GetDayOfMonth(date):00}";
        }
    }
}
