using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers
{
	/// <summary>
	/// Markup parser
	/// </summary>
	internal abstract class MarkupParserBase
	{
		/// <summary>
		/// Name of the ignoring comment tag
		/// </summary>
		const string IGNORING_COMMENT_TAG_NAME = "wmm:ignore";

		/// <summary>
		/// String representation of the start ignoring comment tag
		/// </summary>
		const string START_IGNORING_COMMENT_TAG = "<!--" + IGNORING_COMMENT_TAG_NAME + "-->";

		/// <summary>
		/// String representation of the end ignoring comment tag
		/// </summary>
		const string END_IGNORING_COMMENT_TAG = "<!--/" + IGNORING_COMMENT_TAG_NAME + "-->";

		/// <summary>
		/// Regular expression for working with the doctype declaration
		/// </summary>
		private static readonly Regex _doctypeRegex = new Regex(@"^<!DOCTYPE\s[^>]+?>", RegexOptions.IgnoreCase);

		/// <summary>
		/// Inner markup parsing context
		/// </summary>
		protected InnerMarkupParsingContext _innerContext;

		/// <summary>
		/// Markup parsing context
		/// </summary>
		protected MarkupParsingContext _context;

		/// <summary>
		/// Synchronizer of parsing
		/// </summary>
		protected readonly object _parsingSynchronizer = new object();

		/// <summary>
		/// Gets a common markup parsing handlers
		/// </summary>
		protected abstract MarkupParsingHandlersBase CommonHandlers
		{
			get;
		}


		#region Processing methods

		/// <summary>
		/// Process a doctype declaration
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		protected bool ProcessDoctype()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int contentRemainderLength = _innerContext.RemainderLength;

			var match = _doctypeRegex.Match(content, _innerContext.Position, contentRemainderLength);
			if (match.Success)
			{
				string doctype = match.Value;

				var doctypeHandler = CommonHandlers.Doctype;
				if (doctypeHandler != null)
				{
					doctypeHandler(_context, doctype);
				}

				_innerContext.IncreasePosition(doctype.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a comments
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		protected bool ProcessComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			int commentStartPosition = _innerContext.Position;
			int commentEndPosition = content.IndexOf("-->", commentStartPosition, StringComparison.Ordinal);
			int commentPositionDifference = commentEndPosition - commentStartPosition;

			if (commentPositionDifference >= 4)
			{
				string commentText = commentPositionDifference > 4 ?
					content.Substring(commentStartPosition + 4, commentPositionDifference - 4) : string.Empty;

				switch (commentText)
				{
					case IGNORING_COMMENT_TAG_NAME:
						isProcessed = ProcessStartIgnoringCommentTag();
						break;
					case "/" + IGNORING_COMMENT_TAG_NAME:
						isProcessed = ProcessEndIgnoringCommentTag();
						break;
					default:
						var commentHandler = CommonHandlers.Comment;
						if (commentHandler != null)
						{
							commentHandler(_context, commentText);
						}

						_innerContext.IncreasePosition(commentEndPosition + 3 - commentStartPosition);
						isProcessed = true;
						break;
				}
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a start ignoring comment tags
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		protected bool ProcessStartIgnoringCommentTag()
		{
			string content = _innerContext.SourceCode;
			int startTagLength = START_IGNORING_COMMENT_TAG.Length;
			int endTagLength = END_IGNORING_COMMENT_TAG.Length;
			int startTagPosition = _innerContext.Position;
			int endTagPosition = content.IndexOf(END_IGNORING_COMMENT_TAG,
				startTagPosition + startTagLength, StringComparison.Ordinal);

			if (endTagPosition != -1)
			{
				string fragment = content.Substring(startTagPosition + startTagLength,
					endTagPosition - startTagPosition - startTagLength);

				var ignoredFragmentHandler = CommonHandlers.IgnoredFragment;
				if (ignoredFragmentHandler != null)
				{
					ignoredFragmentHandler(_context, fragment);
				}

				_innerContext.IncreasePosition(endTagPosition + endTagLength - startTagPosition);
				return true;
			}

			throw new MarkupParsingException(
				Strings.ErrorMessage_NotClosedIgnoringCommentTag,
				_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
		}

		/// <summary>
		/// Process a end ignoring comment tags
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		protected bool ProcessEndIgnoringCommentTag()
		{
			throw new MarkupParsingException(
				Strings.ErrorMessage_StartIgnoringCommentTagNotDeclared,
				_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
		}

		/// <summary>
		/// Process a CDATA sections
		/// </summary>
		/// <returns>Result of processing (true - is processed; false - is not processed)</returns>
		protected bool ProcessCdataSection()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			if (content.CustomStartsWith("<![CDATA[", _innerContext.Position, StringComparison.Ordinal))
			{
				int cdataStartPosition = _innerContext.Position;
				int cdataEndPosition = content.IndexOf("]]>", cdataStartPosition, StringComparison.Ordinal);

				if (cdataEndPosition > cdataStartPosition)
				{
					string cdataText = content.Substring(cdataStartPosition + 9,
						cdataEndPosition - cdataStartPosition - 9);

					var cdataSectionHandler = CommonHandlers.CdataSection;
					if (cdataSectionHandler != null)
					{
						cdataSectionHandler(_context, cdataText);
					}

					_innerContext.IncreasePosition(cdataEndPosition + 3 - cdataStartPosition);
					isProcessed = true;
				}
			}

			return isProcessed;
		}

		#endregion
	}
}