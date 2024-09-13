using EstudosIA.Services;
var gptPrompt = "Estou triste porque meu código não funciona";

Console.WriteLine($"{await new AzureOpenAIAgent().GetContent(gptPrompt)}");