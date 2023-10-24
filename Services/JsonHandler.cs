using Microsoft.AspNetCore.Mvc.ModelBinding;
using Student_Planner.Models;
using System.Text.Json;
namespace Student_Planner.Services
{
    public class JsonHandler<T>
    {
        public List<T>? SerializableData { get; set; }
        public string? DataFilePath { get; set; }
        public JsonHandler() { }
        public JsonHandler(string dataFilePath, List<T> list)
        {
            SerializableData = list;
            DataFilePath = dataFilePath;
        }

        //Method for serializing data into a JSON file in the EventData folder
        public void SerializeToJson(string? DataFilePath, List<T> listOfObjects)
        {
            // Serialize the list of events to JSON
            string jsonData = JsonSerializer.Serialize(listOfObjects);
            // Write the JSON data to the file
            try
            {
                if (DataFilePath != null)
                {
                    File.WriteAllText(DataFilePath, jsonData);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Deserialize the JSON file data back into an events list if the file exists
        public List<T> DeserializeFromJSON(string? DataFilePath)
        {
            List<T>? listOfObjects = new List<T>();
            try
            {
                if (File.Exists(DataFilePath))
                {
                    string jsonData = File.ReadAllText(DataFilePath);

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        listOfObjects = JsonSerializer.Deserialize<List<T>>(jsonData);
                    }
                    else
                    {
                        // Handle the case where jsonData is empty
                        listOfObjects.Clear();
                    }
                }
                return listOfObjects;
            } 
            catch (Exception)
            {
                throw;
            }
        }
    }
}
