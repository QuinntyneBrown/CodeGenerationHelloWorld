var data = File.ReadAllText($@"..\..\..\..\..\data\definitions.json");

var definitions = JsonSerializer.Deserialize<JsonElement>(data);

foreach(var item in definitions.GetProperty("simpleTypes").EnumerateArray())
{
    await GenerateAsync(item);
}

async Task GenerateAsync(JsonElement model)
{
    var template = GetTemplate("Record");

    var templateProcessor = new RazorTemplateProcessor();

    var result = await templateProcessor.ProcessAsync(template, new { Name = model.GetProperty("name").GetString() });

    File.WriteAllText($@"..\..\..\..\Target\{model.GetProperty("name").GetString()}.g.cs", result);
}

string GetTemplate(string name)
{
    var assembly = Assembly.GetExecutingAssembly();

    var resourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith($"{name}.txt"));

    using(var stream = assembly.GetManifestResourceStream(resourceName))
    {
        using (var streamReader = new StreamReader(stream))
        {
            return streamReader.ReadToEnd();
        }
    }
}