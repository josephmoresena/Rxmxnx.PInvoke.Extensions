using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Routing.Constraints;

using Rxmxnx.PInvoke;
#if OPEN_API_2
using Microsoft.OpenApi;

#else
using Microsoft.OpenApi.Models;
#endif

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
#if OPEN_API_2
	const JsonSchemaType stringType = JsonSchemaType.String;
	const JsonSchemaType arrayType = JsonSchemaType.Array;
#else
	const String stringType = "string";
	const String arrayType = "array";
	// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident
#endif
	c.MapType<CString>(() => new OpenApiSchema { Type = stringType, });
	c.MapType<CStringSequence>(() => new OpenApiSchema
	{
		Type = arrayType, Items = new OpenApiSchema { Type = stringType, },
	});
#if !OPEN_API_2
	// ReSharper restore ArrangeObjectCreationWhenTypeNotEvident
#endif
});
builder.Services.AddOpenApi();
builder.Services.Configure<RouteOptions>(options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));
builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(
		0, AppJsonSerializerContext.Default);
});

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapSwagger();

RouteGroupBuilder todosApi = app.MapGroup("/todos");

todosApi.MapGet("/", static () => SampleCollections.Todos);
todosApi.MapGet("/{id:int}",
                static (Int32 id) => SampleCollections.Todos.FirstOrDefault(a => a.Id == id) is { } todo ?
	                Results.Ok(todo) :
	                Results.NotFound());

RouteGroupBuilder fruitsApi = app.MapGroup("/fruits");
fruitsApi.MapGet("/", static () => SampleCollections.Fruits);
fruitsApi.MapGet("/{id:int}",
                 static (Int32 id) => SampleCollections.Fruits.Skip(id - 1).FirstOrDefault() is { } fruit ?
	                 Results.Ok(fruit) :
	                 Results.NotFound());

app.Run();

#pragma warning disable CA1050
public record Todo(Int32 Id, CString? Title, DateOnly? DueBy = null, Boolean IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(CStringSequence))]
[JsonSerializable(typeof(CString))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;

internal static class SampleCollections
{
	private const String fruitsData = "鿰躍䄠灰敬\uf000趟\u208f片敥\u206e灁汰e鿰貍䈠湡湡a鿰認传慲杮e鿰讍䰠浥湯\uf000" +
		"趟ₐ敐牡\uf000趟ₑ敐捡h鿰銍䌠敨牲y鿰鎍匠牴睡敢牲y鿰邫䈠畬扥牥祲\uf000趟\u2087片灡獥\uf000趟\u2089慗整浲汥湯\uf000" +
		"趟₍楐敮灡汰e鿰궥䴠湡潧\uf000ꖟ\u209d楋楷\uf000趟\u2088敍潬n鿰薍吠浯瑡o鿰ꖥ䌠捯湯瑵\uf000ꖟₑ癁捯摡o鿰銫传楬敶\0\0";

	public static readonly Todo[] Todos =
	[
		new(1, new(() => "Walk the dog"u8)), new(2, new(() => "Do the dishes"u8), DateOnly.FromDateTime(DateTime.Now)),
		new(3, new(() => "Do the laundry"u8), DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
		new(4, new(() => "Clean the bathroom"u8)),
		new(5, new(() => "Clean the car"u8), DateOnly.FromDateTime(DateTime.Now.AddDays(2))),
	];
	public static readonly CStringSequence Fruits = CStringSequence.Parse(SampleCollections.fruitsData);
}
#pragma warning restore CA1050