﻿using System.Collections.Generic;

using WebMarkupMin.Core;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Defines a interface of markup minification manager
	/// </summary>
	public interface IMarkupMinificationManager : IContentProcessingManager
	{
		/// <summary>
		/// Gets or sets a list of supported media-types
		/// </summary>
		ISet<string> SupportedMediaTypes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a <c>*-Minification-Powered-By</c> HTTP-header
		/// </summary>
		KeyValuePair<string, string> PoweredByHttpHeader
		{
			get;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow generate minification statistics
		/// (available through the logger)
		/// </summary>
		bool GenerateStatistics
		{
			get;
			set;
		}


		/// <summary>
		/// Creates a instance of markup minifier
		/// </summary>
		/// <returns>Instance of markup minifier</returns>
		IMarkupMinifier CreateMinifier();
	}

	/// <summary>
	/// Defines a interface of generic markup minification manager
	/// </summary>
	/// <typeparam name="TSettings">The type of markup minification settings</typeparam>
	public interface IMarkupMinificationManager<TSettings> : IMarkupMinificationManager
		where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a markup minification settings used to configure the HTML minifier
		/// </summary>
		TSettings MinificationSettings
		{
			get;
			set;
		}
	}
}