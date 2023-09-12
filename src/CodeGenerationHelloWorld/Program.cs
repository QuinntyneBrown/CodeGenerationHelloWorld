var data = File.ReadAllText($@"..\..\..\..\..\data\definitions.json");

var definitions = JsonSerializer.Deserialize<JsonElement>(data);

foreach(var item in definitions.GetProperty("simpleTypes").EnumerateArray())
{
    await GenerateAsync(item.GetProperty("name").GetString()!);
}

async Task GenerateAsync(string name)
{
    var template = GetTemplate("Record");

    var templateProcessor = new RazorTemplateProcessor();

    var result = await templateProcessor.ProcessAsync(template, new { Name = name });

    File.WriteAllText($@"..\..\..\..\Target\{name}.cs", result);
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
