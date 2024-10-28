using System;
using System.IO;

namespace OneTrack.PM.APIs.Helpers
{
    public class Upload
    {
        public static string UploadFiles(string ext, string base64String, string folderName)
        {
            base64String = base64String.IndexOf(";base64") >= 0 ? base64String.Split(',')[1] : base64String;
            string filename = Guid.NewGuid().ToString().Substring(0, 10) + ext;
            byte[] bytes = Convert.FromBase64String(base64String);
            string path = Path.Combine("Resources", folderName);
            path = Path.Combine(path, filename);
            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, bytes);
            }
            return path.Replace('\\', '/');
        }
        public static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "jpg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }
        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return    Guid.NewGuid().ToString().Substring(0, 10)
                      + Path.GetExtension(fileName);
        }
    }
}
