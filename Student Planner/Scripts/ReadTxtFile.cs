using System;
using System.IO;

namespace Student_Planner.Scripts
{
    public class ReadTxtFile
    {
        public static string ReadTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "The file does not exist.";
            }
        }
    }
}
