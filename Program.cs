using EstudosIA.Services;
var gptPrompt = "Gere uma frase motivacional para encarar a segunda-feira";

Console.WriteLine($"{await new AzureOpenAIClassService().GetContent(gptPrompt)}");