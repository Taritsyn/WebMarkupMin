using System;
using System.Text;
using System.Threading;

namespace WebMarkupMin.Core.Utilities
{
	/// <summary>
	/// Pool of string builders
	/// </summary>
	public static class StringBuilderPool
	{
		/// <summary>
		/// Maximum capacity of builder
		/// </summary>
		const int MAX_BUILDER_CAPACITY = 8 * 1024;

		/// <summary>
		/// Number of builders per processor
		/// </summary>
		const int BUILDER_COUNT_PER_PROCESSOR = 4;

		/// <summary>
		/// First builder
		/// </summary>
		/// <remarks>The first builder is stored in a dedicated field, because we expect
		/// to be able to satisfy most requests from it.</remarks>
		private static StringBuilder _firstBuilder;

		/// <summary>
		/// List of the remaining builders
		/// </summary>
		private static readonly StringBuilder[] _builders;


		/// <summary>
		/// Static constructor
		/// </summary>
		static StringBuilderPool()
		{
			int builderCount = Environment.ProcessorCount * BUILDER_COUNT_PER_PROCESSOR;
			if (builderCount < 5)
			{
				builderCount = 5;
			}

			_builders = new StringBuilder[builderCount - 1];
		}


		/// <summary>
		/// Gets a instance of string builder from the pool
		/// </summary>
		/// <returns>Instance of string builder</returns>
		public static StringBuilder GetBuilder()
		{
			// Examine the first builder.
			// If that fails, then `GetBuilderSlow` method will look at the remaining builders.
			StringBuilder builder = _firstBuilder;
			if (builder == null || builder != Interlocked.CompareExchange(ref _firstBuilder, null, builder))
			{
				builder = GetBuilderSlow();
			}

			return builder;
		}

		/// <summary>
		/// Gets a instance of string builder with at least the given capacity from the pool
		/// </summary>
		/// <remarks>If the capacity is less than or equal to our maximum capacity, then return builder from the pool.
		/// Otherwise create a new string builder, that will just get discarded when released.</remarks>
		/// <param name="capacity">Capacity of string builder</param>
		/// <returns>Instance of string builder</returns>
		public static StringBuilder GetBuilder(int capacity)
		{
			if (capacity <= MAX_BUILDER_CAPACITY)
			{
				return GetBuilder();
			}

			return new StringBuilder(capacity);
		}

		private static StringBuilder GetBuilderSlow()
		{
			StringBuilder[] builders = _builders;
			int builderCount = builders.Length;

			for (int builderIndex = 0; builderIndex < builderCount; builderIndex++)
			{
				StringBuilder builder = builders[builderIndex];
				if (builder != null)
				{
					if (builder == Interlocked.CompareExchange(ref builders[builderIndex], null, builder))
					{
						return builder;
					}
				}
			}

			return new StringBuilder(MAX_BUILDER_CAPACITY);
		}

		/// <summary>
		/// Returns a instance of string builder to the pool
		/// </summary>
		/// <param name="builder">Instance of string builder</param>
		public static void ReleaseBuilder(StringBuilder builder)
		{
			if (builder == null || builder.Capacity > MAX_BUILDER_CAPACITY)
			{
				return;
			}

			if (_firstBuilder == null)
			{
				builder.Clear();
				_firstBuilder = builder;
			}
			else
			{
				ReleaseBuilderSlow(builder);
			}
		}

		private static void ReleaseBuilderSlow(StringBuilder builder)
		{
			StringBuilder[] builders = _builders;
			int builderCount = builders.Length;

			for (int builderIndex = 0; builderIndex < builderCount; builderIndex++)
			{
				if (builders[builderIndex] == null)
				{
					builder.Clear();
					builders[builderIndex] = builder;
					break;
				}
			}
		}
	}
}