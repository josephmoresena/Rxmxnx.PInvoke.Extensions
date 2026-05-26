namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly partial struct Builder
	{
		/// <inheritdoc cref="CStringSequence.Count"/>
		/// <returns>The length of the current <see cref="Builder"/> object.</returns>
		/// <remarks>This operation is thread-safe.</remarks>
		public Int32 ConcurrentCount() => new Concurrent(this._value).Count;
		/// <inheritdoc cref="CStringSequence.Builder.Append(CString?)"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentAppend(CString? value)
		{
			Concurrent concurrent = new(this._value);
			if (value is null || value.IsZero)
				concurrent.AppendNull();
			else
				concurrent.Append(value.AsSpan());
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Append(String?)"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentAppend(String? value)
		{
			Concurrent concurrent = new(this._value);
			if (value is null)
				concurrent.AppendNull();
			else
				concurrent.Append(value.AsSpan(), Encoding.UTF8.GetByteCount(value));
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Append(ReadOnlySpan{Byte})"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentAppend(ReadOnlySpan<Byte> value)
		{
			Concurrent concurrent = new(this._value);
			concurrent.Append(value);
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Append(ReadOnlySequence{Byte})"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentAppend(ReadOnlySequence<Byte> value)
		{
			Concurrent concurrent = new(this._value);
			concurrent.Append(value);
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Append(ReadOnlySpan{Char})"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentAppend(ReadOnlySpan<Char> value)
		{
			Concurrent concurrent = new(this._value);
			concurrent.Append(value, Encoding.UTF8.GetByteCount(value));
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.AppendEscaped(ReadOnlySpan{Byte})"/>
		/// <remarks>This operation is thread-safe.</remarks>
#if NET5_0_OR_GREATER
		[SkipLocalsInit]
#endif
		public Builder ConcurrentAppendEscaped(ReadOnlySpan<Byte> escaped)
		{
			Concurrent concurrent = new(this._value);
			if (escaped.IsEmpty)
			{
				concurrent.Append(ReadOnlySpan<Byte>.Empty);
				return this;
			}

			Byte[]? byteArray = default;
			try
			{
				Span<Byte> bytes = StackAllocationHelper.HasStackBytes(escaped.Length) ?
					stackalloc Byte[escaped.Length] :
					StackAllocationHelper.RentArray(escaped.Length, out byteArray, false);

				escaped.CopyTo(bytes);
				concurrent.Append(bytes[..^TextUnescape.Unescape(bytes)]);
				return this;
			}
			finally
			{
				StackAllocationHelper.ReturnArray(byteArray);
			}
		}
		/// <inheritdoc cref="CStringSequence.Builder.AppendEscaped(ReadOnlySequence{Byte})"/>
		/// <remarks>This operation is thread-safe.</remarks>
#if NET5_0_OR_GREATER
		[SkipLocalsInit]
#endif
		public Builder ConcurrentAppendEscaped(ReadOnlySequence<Byte> escaped)
		{
			Concurrent concurrent = new(this._value);
			if (escaped.IsEmpty)
			{
				concurrent.Append(ReadOnlySpan<Byte>.Empty);
				return this;
			}

			Byte[]? byteArray = default;
			Int32 utf8Length = (Int32)escaped.Length;
			try
			{
				Span<Byte> bytes = StackAllocationHelper.HasStackBytes(utf8Length) ?
					stackalloc Byte[utf8Length] :
					StackAllocationHelper.RentArray(utf8Length, out byteArray, false);

				escaped.CopyTo(bytes);
				concurrent.Append(bytes[..^TextUnescape.Unescape(bytes)]);
				return this;
			}
			finally
			{
				StackAllocationHelper.ReturnArray(byteArray);
			}
		}
		/// <inheritdoc cref="CStringSequence.Builder.Insert(Int32, CString?)"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentInsert(Int32 index, CString? value)
		{
			Concurrent concurrent = new(this._value);
			if (value is null || value.IsZero)
				concurrent.InsertNull(index);
			else
				concurrent.Insert(index, value.AsSpan());
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Insert(Int32, String?)"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentInsert(Int32 index, String? value)
		{
			Concurrent concurrent = new(this._value);
			if (value is null)
				concurrent.InsertNull(index);
			else
				concurrent.Insert(index, value.AsSpan(), Encoding.UTF8.GetByteCount(value));
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Insert(Int32, ReadOnlySpan{Byte})"/>
		/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public Builder ConcurrentInsert(Int32 index, ReadOnlySpan<Byte> value)
		{
			Concurrent concurrent = new(this._value);
			concurrent.Insert(index, value);
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Insert(Int32, ReadOnlySpan{Char})"/>
		/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public Builder ConcurrentInsert(Int32 index, ReadOnlySpan<Char> value)
		{
			Concurrent concurrent = new(this._value);
			concurrent.Insert(index, value, Encoding.UTF8.GetByteCount(value));
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.RemoveAt(Int32)"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentRemoveAt(Int32 index)
		{
			Concurrent concurrent = new(this._value);
			concurrent.RemoveAt(index);
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.ConcurrentClear()"/>
		/// <remarks>This operation is thread-safe.</remarks>
		public Builder ConcurrentClear()
		{
			Concurrent concurrent = new(this._value);
			concurrent.Reset();
			return this;
		}
		/// <inheritdoc cref="CStringSequence.Builder.Build()"/>
		/// <returns>The length of the current <see cref="Builder"/> object.</returns>
		/// <remarks>This operation is thread-safe.</remarks>
		public CStringSequence ConcurrentBuild() => new Concurrent(this._value).CreateSequence();

		/// <summary>
		/// Represents a concurrent <see cref="Value"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="Value"/> instance.</param>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		private readonly struct Concurrent(Value value)
		{
			/// <summary>
			/// Internal value.
			/// </summary>
			private readonly Value _value = value;

			/// <inheritdoc cref="Value.Count"/>
			public Int32 Count
			{
				get
				{
#if NET9_0_OR_GREATER
					using (this._value.GetLock().EnterScope())
#else
					lock (this._value.GetLock())
#endif
						return this._value.Count;
				}
			}

			/// <inheritdoc cref="Value.CreateSequence()"/>
			public CStringSequence CreateSequence()
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					return this._value.CreateSequence();
			}
			/// <inheritdoc cref="Value.AppendNull()"/>
			public void AppendNull()
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.AppendNull();
			}
			/// <inheritdoc cref="Value.InsertNull(Int32)"/>
			public void InsertNull(Int32 index)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.InsertNull(index);
			}
			/// <inheritdoc cref="Value.Append(ReadOnlySpan{Byte})"/>
			public void Append(ReadOnlySpan<Byte> utf8Text)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.Append(utf8Text);
			}
			/// <inheritdoc cref="Value.Append(ReadOnlySequence{Byte})"/>
			public void Append(ReadOnlySequence<Byte> utf8Text)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.Append(utf8Text);
			}
			/// <inheritdoc cref="Value.Append(ReadOnlySpan{Char}, Int32)"/>
			public void Append(ReadOnlySpan<Char> utf16Text, Int32 utf8Length)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.Append(utf16Text, utf8Length);
			}
			/// <inheritdoc cref="Value.Insert(Int32, ReadOnlySpan{Byte})"/>
			public void Insert(Int32 index, ReadOnlySpan<Byte> utf8Text)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.Insert(index, utf8Text);
			}
			/// <inheritdoc cref="Value.Insert(Int32, ReadOnlySpan{Char}, Int32)"/>
			public void Insert(Int32 index, ReadOnlySpan<Char> utf16Text, Int32 utf8Length)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.Insert(index, utf16Text, utf8Length);
			}
			/// <inheritdoc cref="Value.RemoveAt(Int32)"/>
			public void RemoveAt(Int32 index)
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.RemoveAt(index);
			}
			/// <inheritdoc cref="Value.Reset()"/>
			public void Reset()
			{
#if NET9_0_OR_GREATER
				using (this._value.GetLock().EnterScope())
#else
				lock (this._value.GetLock())
#endif
					this._value.Reset();
			}
		}
	}
}