﻿using System.Collections.Generic;

using WebMarkupMin.Core;
using WebMarkupMin.Core.Loggers;

namespace WebMarkupMin.AspNet.Common
{
	/// <summary>
	/// Base class of markup minification manager
	/// </summary>
	/// <typeparam name="TSettings">The type of markup minification settings</typeparam>
	public abstract class MarkupMinificationManagerBase<TSettings>
		: ContentProcessingManagerBase, IMarkupMinificationManager<TSettings> where TSettings : class, new()
	{
		/// <summary>
		/// Gets or sets a logger
		/// </summary>
		protected virtual ILogger Logger
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a markup minification settings used to configure the HTML minifier
		/// </summary>
		public TSettings MinificationSettings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a list of supported media-types
		/// </summary>
		public ISet<string> SupportedMediaTypes
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a <c>*-Minification-Powered-By</c> HTTP-header
		/// </summary>
		public KeyValuePair<string, string> PoweredByHttpHeader
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to allow generate minification statistics
		/// (available through the logger)
		/// </summary>
		public bool GenerateStatistics
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs a instance of markup minification manager
		/// </summary>
		protected MarkupMinificationManagerBase()
		{
			GenerateStatistics = false;
		}


		/// <summary>
		/// Creates a instance of markup minifier
		/// </summary>
		/// <returns>Instance of markup minifier</returns>
		public abstract IMarkupMinifier CreateMinifier();
	}
}