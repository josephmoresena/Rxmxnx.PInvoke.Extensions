using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;

using Rxmxnx.PInvoke;
using Rxmxnx.PInvoke.WebApiTest;

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
todosApi.MapGet("/{id:int}", static async Task<Results<Ok<Todo>, NotFound>> (Int32 id) =>
{
	await Task.Yield();
	Todo? todo = SampleCollections.Todos.FirstOrDefault(a => a.Id == id);
	await Task.Delay(Random.Shared.Next(10, 200));
	return todo is not null ? TypedResults.Ok(todo) : TypedResults.NotFound();
});

RouteGroupBuilder fruitsApi = app.MapGroup("/fruits");
fruitsApi.MapGet("/", static () => SampleCollections.Fruits);
fruitsApi.MapGet("/{id:int}",
                 static (Int32 id) => SampleCollections.Fruits.Skip(id - 1).FirstOrDefault() is { } fruit ?
	                 Results.Ok(fruit) :
	                 Results.NotFound());
fruitsApi.MapGet("random/{count:int}", (Int32 count, CancellationToken cancellationToken) =>
{
	return new JsonAsyncEnumerableResult<CString>(EnumerateRandomFruits(count, cancellationToken));
	static async IAsyncEnumerable<CString> EnumerateRandomFruits(Int32 count,
		[EnumeratorCancellation] CancellationToken ct)
	{
		await Task.Yield();
		while (count > 0 && !ct.IsCancellationRequested)
		{
			Int32 index = Random.Shared.Next(0, SampleCollections.Fruits.Count);
			yield return SampleCollections.Fruits[index];
			await Task.Delay(Random.Shared.Next(10, 200));
			count--;
		}
	}
});

RouteGroupBuilder textApi = app.MapGroup("/text");
textApi.MapPost("/concat", static ([FromBody] CStringSequence sequence) => sequence.ToCString(false));
textApi.MapPost("/getPrint",
                static ([FromBody] CStringSequence sequence) => PrintSequenceBuffer(sequence.ToString().AsSpan()));
app.Run();
return;

static CString PrintSequenceBuffer(ReadOnlySpan<Char> buffer)
{
	CStringBuilder cstrBuild = new();
	foreach (Rune rune in buffer.EnumerateRunes())
	{
		if (rune.Value == 0)
		{
			cstrBuild.Append("\\0"u8);
			continue;
		}

		UnicodeCategory cat = Rune.GetUnicodeCategory(rune);

		Boolean printable = cat != UnicodeCategory.Control && cat != UnicodeCategory.Format &&
			cat != UnicodeCategory.PrivateUse && cat != UnicodeCategory.OtherNotAssigned;

		if (!printable || rune.Value == '"' || rune.Value == '\\' || rune.Value is >= 0x9FE0 and <= 0x9FFF)
			AppendHex(cstrBuild, rune);
		else
			cstrBuild.Append(rune);
	}
	return cstrBuild.ToCString(false);
	static void AppendHex(CStringBuilder cstrBuild, Rune rune)
	{
		if (rune.Value <= 0xFFFF)
		{
			cstrBuild.Append("\\u"u8);

			Span<Byte> span = stackalloc Byte[4];
			rune.Value.TryFormat(span, out _, "x4");
			cstrBuild.Append(span);
		}
		else
		{
			cstrBuild.Append("\\U"u8);

			Span<Byte> span = stackalloc Byte[8];
			rune.Value.TryFormat(span, out _, "x8");
			cstrBuild.Append(span);
		}
	}
}

#pragma warning disable CA1050
public record Todo(Int32 Id, CString? Title, DateOnly? DueBy = null, Boolean IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
[JsonSerializable(typeof(CStringSequence))]
[JsonSerializable(typeof(CString))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;

internal static class SampleCollections
{
	private const String fruitsData = "\u9ff0躍䄠灰敬\uf000趟\u208f片敥\u206e灁汰e\u9ff0貍䈠湡湡a\u9ff0認传慲杮e\u9ff0讍䰠浥湯\uf000" +
		"趟ₐ敐牡\uf000趟ₑ敐捡h\u9ff0銍䌠敨牲y\u9ff0鎍匠牴睡敢牲y\u9ff0邫䈠畬扥牥祲\uf000趟₇片灡獥\uf000趟₉慗整浲汥湯\uf000" +
		"趟₍楐敮灡汰e\u9ff0궥䴠湡潧\uf000ꖟ\u209d楋楷\uf000趟₈敍潬n\u9ff0薍吠浯瑡o\u9ff0ꖥ䌠捯湯瑵\uf000ꖟₑ癁捯摡o\u9ff0銫传楬敶\0";

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