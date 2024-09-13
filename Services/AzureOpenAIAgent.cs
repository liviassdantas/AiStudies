    using Azure;
    using Azure.AI.OpenAI;
    using Azure.AI.OpenAI.Assistants;
    using Microsoft.SemanticKernel.ChatCompletion;
    using System;
    using static System.Environment;
    namespace EstudosIA.Services
    {
        public class AzureOpenAIAgent
        {

            string endpoint = GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT", EnvironmentVariableTarget.User) ?? "";
            string key = GetEnvironmentVariable("AZURE_OPENAI_API_KEY", EnvironmentVariableTarget.User) ?? "";
            const string deploymentName = "EstudosIAGPT4";

            public async Task<string> GetContent(string prompt)
            {
                AssistantsClient client = new AssistantsClient(new Uri(endpoint), new AzureKeyCredential(key));
                Response<Assistant> assistantResponse = await client.CreateAssistantAsync(
                new AssistantCreationOptions("gpt-4-1106-preview")
                {
                    Name = "Programador Amarelo",
                    Instructions = "Você será um agente de apoio emocional para programadores. Sempre que um programador reclamar, você dirá que está tudo bem e passará uma frase motivacional.",
                    Tools = { new CodeInterpreterToolDefinition() }
                });
                Assistant assistant = assistantResponse.Value;

                Response<AssistantThread> threadResponse = await client.CreateThreadAsync();
                AssistantThread thread = threadResponse.Value;
                
                Response<ThreadMessage> messageResponse = await client.CreateMessageAsync(
                thread.Id,
                MessageRole.User,
                prompt);
                ThreadMessage message = messageResponse.Value;
                Response<ThreadRun> runResponse = await client.CreateRunAsync(
                thread.Id,
        new CreateRunOptions(assistant.Id)
        {
            AdditionalInstructions = "Caso ele esteja mal, sugira contatar um psicólogo.",
            });
                ThreadRun run = runResponse.Value;
                do
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500));
        runResponse = await client.GetRunAsync(thread.Id, runResponse.Value.Id);
    }
    while (runResponse.Value.Status == RunStatus.Queued || runResponse.Value.Status == RunStatus.InProgress);}

        }
    }


