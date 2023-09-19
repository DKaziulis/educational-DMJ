using System;

namespace StudentPlanner
{
    public class InputFile
    {
        public static string? FileName { get; set; }
        public long Size { get; set; }
        public DateTime UploadTime { get; set; }
        public string Extension = Path.GetExtension(FileName);
    }
}