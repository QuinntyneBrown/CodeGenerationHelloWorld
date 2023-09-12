// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using RazorEngineCore;

namespace CodeGenerationHelloWorld;

public class RazorTemplateProcessor : RazorEngine { 

    public async Task<string> ProcessAsync<T>(string template, T model)
    {
        var compiledTemplate = await CompileAsync(template);

        return await compiledTemplate.RunAsync(model);
    }
}
