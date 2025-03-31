using System.ClientModel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Hosting;
using OpenAI;

var builder = Host.CreateApplicationBuilder();

var app = builder.Build();

var credential = new ApiKeyCredential("");
var openAiOption = new OpenAIClientOptions()
{
    Endpoint = new Uri("https://models.inference.ai.azure.com"),
};

var openAiClient = new OpenAIClient(credential, openAiOption);

var chatClient = openAiClient.AsChatClient("gpt-4o-mini");

// Single response
//var responsee = await chatClient.GetResponseAsync("Hello, how are you?");


// Countinuous chat
var chatHistory = new List<ChatMessage>();

while(true)
{
    Console.WriteLine("User -");
    var input = Console.ReadLine();
    if (string.IsNullOrEmpty(input))
    {
        break;
    }

    chatHistory.Add(new ChatMessage(ChatRole.User, input));

    var response = await chatClient.GetResponseAsync(input);

    Console.WriteLine("AI -");
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response.Text));
}
