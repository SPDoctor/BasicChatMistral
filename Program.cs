using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Plugins;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var builder = Kernel.CreateBuilder();

string deploymentName = "Mistral-Large-3";
string endpoint = "https://billa-workshop-resource.services.ai.azure.com/openai/v1";
string apiKey = Environment.GetEnvironmentVariable("apiKey_mistral", EnvironmentVariableTarget.User)!;

builder.Services.AddOpenAIChatCompletion(
    modelId: deploymentName,
    endpoint: new Uri(endpoint),
    apiKey: apiKey);

builder.Plugins.AddFromType<TimePlugin>();
var kernel = builder.Build();

var chat = kernel.GetRequiredService<IChatCompletionService>();

// Create chat history
var history = new ChatHistory();
history.AddSystemMessage("You are a helpful assistant who always answers in rhyme.");

// Get chat completion service
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
  // enable auto function calling
  ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
};

// Start the conversation
while (true)
{
  // Get user input
  Console.Write("User > ");
  var userMessage = Console.ReadLine()!;
  if(userMessage == "exit" || userMessage == "quit") break;
  if(userMessage == "") continue;
  history.AddUserMessage(userMessage);

  // Get the response from the AI
  var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
      history,
      executionSettings: openAIPromptExecutionSettings,
      kernel: kernel);

  // Stream the results
  string fullMessage = "";
  var first = true;
  await foreach (var content in result)
  {
    if (content.Role.HasValue && first)
    {
      Console.Write("Assistant > ");
      first = false;
    }
    Console.Write(content.Content);
    fullMessage += content.Content;
  }
  Console.WriteLine();

  // Add the message from the agent to the chat history
  history.AddAssistantMessage(fullMessage);
}