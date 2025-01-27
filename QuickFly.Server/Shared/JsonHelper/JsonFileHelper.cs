using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QuickFly.Server.Shared.JsonHelper;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

public class JsonFileHelper: IJsonFileHelper
{
    private  readonly string JsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "reservations.json");

    public  List<T> ReadFromJsonFile<T>()
    {
        using StreamReader file = File.OpenText(JsonFilePath);
        JsonSerializer serializer = new JsonSerializer();
        return (List<T>)serializer.Deserialize(file, typeof(List<T>));
    }

    public void WriteToJsonFile<T>(List<T> data)
    {
        using StreamWriter file = File.CreateText(JsonFilePath);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, data);
    }

   
}