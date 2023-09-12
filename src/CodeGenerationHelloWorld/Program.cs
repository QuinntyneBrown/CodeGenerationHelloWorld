var data = File.ReadAllText($@"..\..\..\..\..\data\definitions.json");

var definitions = JsonSerializer.Deserialize<JsonElement>(data);

foreach(var item in definitions.GetProperty("simpleTypes").EnumerateArray())
{
    await Generate(item.GetProperty("name").GetString()!);
}

async Task Generate(string name)
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
