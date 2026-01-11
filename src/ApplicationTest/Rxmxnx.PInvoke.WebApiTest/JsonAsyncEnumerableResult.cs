using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Rxmxnx.PInvoke.WebApiTest;

public abstract class JsonAsyncEnumerableResult
{
	/// <inheritdoc cref="IEndpointMetadataProvider.PopulateMetadata(MethodInfo, EndpointBuilder)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void PopulateMetadata<TProvider>(MethodInfo method, EndpointBuilder builder)
		where TProvider : IEndpointMetadataProvider
		=> TProvider.PopulateMetadata(method, builder);

	[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Uses ASP .NET serialization context.")]
	[UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Uses ASP .NET serialization context.")]
	protected sealed class ResponseWriter : IAsyncDisposable
	{
		private readonly HttpResponse _response;
		private readonly JsonSerializerOptions _serializerOptions;
		private readonly Utf8JsonWriter _writer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ResponseWriter(HttpContext httpContext) : this(httpContext.Response, httpContext.RequestServices) { }
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ResponseWriter(HttpResponse response, IServiceProvider serviceProvider) : this(
			response, serviceProvider.GetService<IOptions<JsonOptions>>()?.Value) { }
		private ResponseWriter(HttpResponse response, JsonOptions? jsonOptions)
		{
			this._response = response;
			this._serializerOptions =
				ResponseWriter.GetSerializerOptions(jsonOptions, out JsonWriterOptions writerOptions);
			this._writer = new(response.BodyWriter, writerOptions);
		}

		public async ValueTask DisposeAsync()
		{
			await this._writer.DisposeAsync();
			await this._response.CompleteAsync();
		}

		public async ValueTask WriteEmptyArray(Boolean isHttps)
		{
			this.WriteArrayBegin(isHttps);
			await this.WriteArrayEnd();
		}
		public void WriteArrayBegin(Boolean isHttps)
		{
			this._response.Headers.Append(HeaderNames.ContentType, "application/json; charset=utf-8");
			if (!isHttps)
				this._response.Headers.Append(HeaderNames.Connection, "keep-alive");
			this._writer.WriteStartArray();
		}
		public async ValueTask WriteElement<T>(T value, Boolean hasNext)
		{
			await Task.Yield();
			if (hasNext)
			{
				await this._writer.FlushAsync();
				await this._response.Body.FlushAsync();
			}
			JsonSerializer.Serialize(this._writer, value, this._serializerOptions);
		}
		public async ValueTask WriteArrayEnd()
		{
			this._writer.WriteEndArray();
			await this._writer.FlushAsync();
			await this._response.Body.FlushAsync();
		}

		private static JsonSerializerOptions GetSerializerOptions(JsonOptions? jsonOptions,
			out JsonWriterOptions writerOptions)
		{
			if (jsonOptions is null)
			{
				writerOptions = new();
				return new();
			}
			writerOptions = new()
			{
				IndentCharacter = jsonOptions.SerializerOptions.IndentCharacter,
				Indented = jsonOptions.SerializerOptions.WriteIndented,
				IndentSize = jsonOptions.SerializerOptions.IndentSize,
				MaxDepth = jsonOptions.SerializerOptions.MaxDepth,
				Encoder = jsonOptions.SerializerOptions.Encoder,
				NewLine = jsonOptions.SerializerOptions.NewLine,
				SkipValidation = true,
			};
			return jsonOptions.SerializerOptions;
		}
	}
}

public sealed class JsonAsyncEnumerableResult<T>(IAsyncEnumerable<T> asyncEnumerable) : JsonAsyncEnumerableResult,
	IResult, IEndpointMetadataProvider, IValueHttpResult<IAsyncEnumerable<T>>, IValueHttpResult
{
	/// <inheritdoc/>
	public async Task ExecuteAsync(HttpContext httpContext)
	{
		await using IAsyncEnumerator<T> enumerator = asyncEnumerable.GetAsyncEnumerator();
		await using ResponseWriter writer = new(httpContext);
		if (!await enumerator.MoveNextAsync())
		{
			await writer.WriteEmptyArray(httpContext.Request.IsHttps);
			return;
		}

		T current = enumerator.Current;
		writer.WriteArrayBegin(httpContext.Request.IsHttps);
		while (await enumerator.MoveNextAsync())
		{
			await writer.WriteElement(current, true);
			current = enumerator.Current;
		}
		await writer.WriteElement(current, false);
		await writer.WriteArrayEnd();
	}
	Object IValueHttpResult.Value => asyncEnumerable;
	IAsyncEnumerable<T> IValueHttpResult<IAsyncEnumerable<T>>.Value => asyncEnumerable;

	static void IEndpointMetadataProvider.PopulateMetadata(MethodInfo method, EndpointBuilder builder)
		=> JsonAsyncEnumerableResult.PopulateMetadata<Ok<T[]>>(method, builder);
}