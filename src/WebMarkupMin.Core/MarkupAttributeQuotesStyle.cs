﻿namespace WebMarkupMin.Core
{
	/// <summary>
	/// Style of the markup attribute quotes
	/// </summary>
	public enum MarkupAttributeQuotesStyle : byte
	{
		/// <summary>
		/// Auto-detect style for attribute quotes based on the source input
		/// </summary>
		Auto = 0,

		/// <summary>
		/// Optimal style for attribute quotes based on the attribute value
		/// </summary>
		Optimal = 1,

		/// <summary>
		/// Single quotes
		/// </summary>
		Single = 2,

		/// <summary>
		/// Double quotes
		/// </summary>
		Double = 3
	}
}