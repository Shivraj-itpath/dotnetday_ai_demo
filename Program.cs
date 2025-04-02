using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using OpenAI;

var builder = Host.CreateApplicationBuilder();

var app = builder.Build();

string systemPrompt = "You are Arlo, a helpful and friendly assistant created by xAI. Introduce yourself by name when responding to users, and provide accurate, clear, and polite answers to their questions.";

var credential = new ApiKeyCredential("");
var openAiOption = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.inference.ai.azure.com"),
};

var openAiClient = new OpenAIClient(credential, openAiOption);

var chatClient = openAiClient.AsChatClient("gpt-4o-mini");

// Countinuous chat
var chatHistory = new List<ChatMessage>()
{
    new ChatMessage(ChatRole.System, systemPrompt),
};

while (true)
{
    Console.WriteLine("User -");
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
    {
        break;
    }

    chatHistory.Add(new ChatMessage(ChatRole.User, input));
    Console.WriteLine("AI -");

    var chatResponse = "";

    //await foreach(var item in chatClient.GetStreamingResponseAsync(chatHistory))
    //{
    //    Console.Write(item.Text);
    //    chatResponse += item.Text;
    //}

    var response = await chatClient.GetResponseAsync(chatHistory);
    Console.WriteLine(response.Text);

    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response.Text));
    Console.WriteLine();
}
