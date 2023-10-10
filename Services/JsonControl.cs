using Microsoft.AspNetCore.Mvc.ModelBinding;
using Student_Planner.Models;
using System.Text.Json;
namespace Student_Planner.Services
{
    public class JsonControl<T>
    {
        public List<T>? SerializableData { get; set; }
        public string? DataFilePath { get; set; }
        public JsonControl() { }
        public JsonControl(string dataFilePath, List<T> list)
        {
            SerializableData = list;
            DataFilePath = dataFilePath;
        }

        //Method for serializing data into a JSON file in the EventData folder
        public void SerializeToJson(string jsonData, string? DataFilePath)
        {
            // Serialize the list of events to JSON
            jsonData = JsonSerializer.Serialize(SerializableData);
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
        public List<T> DeserializeFromJSON(string jsonData, string? DataFilePath , List<T> listOfObjects)
        {
            try
            {
                if (File.Exists(DataFilePath))
                {
                    jsonData = File.ReadAllText(DataFilePath);

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
