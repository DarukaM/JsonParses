using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide a file path as an argument: ");

            string filePathNoArgs = Console.ReadLine()
                ?? throw new ArgumentNullException("File path was null");
            string fileContents = File.ReadAllText(filePathNoArgs);
            JsonDocument doc = JsonDocument.Parse(fileContents);

            foreach (JsonProperty properties in doc.RootElement.EnumerateObject())
            {
                JsonKeyValue(properties.Name, properties.Value);
            }
            Console.ReadLine();
        }

        else
        {
            string filePath = args[0];

            string fileContents = File.ReadAllText(filePath);
            JsonDocument doc = JsonDocument.Parse(fileContents);

            foreach (JsonProperty properties in doc.RootElement.EnumerateObject())
            {
                JsonKeyValue(properties.Name, properties.Value);
            }

            Console.ReadLine();
        }

    }

    static void JsonKeyValue(string path , JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (JsonProperty properties in element.EnumerateObject())
                {
                    if (path.Contains("Secured"))
                    {
                        string finalPathSecured = path + "--" + properties.Name;
                        JsonKeyValue(finalPathSecured[9..], properties.Value);
                    }
                    else
                    {
                        string finalPath = path + "__" + properties.Name;
                        JsonKeyValue(finalPath, properties.Value);
                    }
                }
                break;
            case JsonValueKind.Array:
                int arrayIndex = 0;
                foreach (JsonElement arrayElement in element.EnumerateArray())
                {
                    string arrayPath = path + $"({arrayIndex})";
                    JsonKeyValue(arrayPath, arrayElement);
                    arrayIndex++;
                }
                break;

            default:
                Console.WriteLine($"{path}: {element}");
                break;
        }
    }
}
