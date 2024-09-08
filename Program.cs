using Azure;
using Azure.AI.OpenAI;
using System;
using static System.Environment;

string endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", EnvironmentVariableTarget.User) ?? "";
string key = GetEnvironmentVariable("AZURE_OPENAI_API_KEY", EnvironmentVariableTarget.User) ?? "";
const string deploymentOrModelName = "EstudosIAGPT4";
 async Task<string> GetContent(string prompt)
{
    var client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));

    var chatCompletionsOptions = new ChatCompletionsOptions
    {
        DeploymentName = deploymentOrModelName,
        Temperature = (float)0.5,
        MaxTokens = 800,
        NucleusSamplingFactor = (float)0.95,
        FrequencyPenalty = 0,
        PresencePenalty = 0,
    };

    chatCompletionsOptions.Messages.Add(new ChatRequestUserMessage(prompt));
    var response = await client.GetChatCompletionsAsync(chatCompletionsOptions);

    return response.Value.Choices[0].Message.Content;
}

var gptPrompt = "Gere uma frase motivacional para encarar a segunda-feira";

Console.WriteLine($"{await GetContent(gptPrompt)}");