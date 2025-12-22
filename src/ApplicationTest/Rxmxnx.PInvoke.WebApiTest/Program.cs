using System.Text.Json.Serialization;

using Rxmxnx.PInvoke;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(
		0, AppJsonSerializerContext.Default);
});

WebApplication app = builder.Build();
Todo[] sampleTodos =
[
	new(1, new(() => "Walk the dog"u8)), new(2, new(() => "Do the dishes"u8), DateOnly.FromDateTime(DateTime.Now)),
	new(3, new(() => "Do the laundry"u8), DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
	new(4, new(() => "Clean the bathroom"u8)),
	new(5, new(() => "Clean the car"u8), DateOnly.FromDateTime(DateTime.Now.AddDays(2))),
];
RouteGroupBuilder todosApi = app.MapGroup("/todos");

todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id:int}",
                (Int32 id) => sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo ?
	                Results.Ok(todo) :
	                Results.NotFound());

app.Run();

public record Todo(Int32 Id, CString? Title, DateOnly? DueBy = null, Boolean IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext { }