using System;
using System.IO;
using System.Text;

namespace Student_Planner.Services.Implementations
{
    public class ReadTxtFile
    {
        public static string ReadTextFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
                else
                {
                    return "The file does not exist.";
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }
    }
}
