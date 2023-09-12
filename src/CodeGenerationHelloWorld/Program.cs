using CodeGenerationHelloWorld;
using System.Reflection;
using System.Text;
using System.Text.Json;

var data = File.ReadAllText($@"..\..\..\..\..\data\definitions.json");

var input = JsonSerializer.Deserialize<DefinitionsFile>(data, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

foreach(var definition in input.SimpleTypes)
{
    await Generate(definition);
}

async Task Generate(Definition model)
{
    var template = GetTemplate("Record");

    var templateProcessor = new RazorTemplateProcessor();

    var result = await templateProcessor.ProcessAsync(template, model);

    File.WriteAllText($@"..\..\..\..\Target\{model.Name}.cs", result);
}

string GetTemplate(string name)
{
    var assembly = Assembly.GetExecutingAssembly();

    var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith($"{name}.txt"));

    var stringBuilder = new StringBuilder();

    using(var stream = assembly.GetManifestResourceStream(resourceName))
    {
        using (var streamReader = new StreamReader(stream))
        {
            string line;
            while((line = streamReader.ReadLine()) != null)
            {
                stringBuilder.AppendLine(line);
            }
        }
    }

    
    return stringBuilder.ToString();
}

public class DefinitionsFile
{
    public List<Definition> SimpleTypes { get; set; }
};


public class Definition
{
    public string Name { get; set; }
}