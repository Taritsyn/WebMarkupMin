using System;

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
		/// Begin part of the markup comment
		/// </summary>
		protected const string COMMENT_BEGIN_PART = "<!--";

		/// <summary>
		/// End part of the markup comment
		/// </summary>
		protected const string COMMENT_END_PART = "-->";

		/// <summary>
		/// Begin part of the CDATA section
		/// </summary>
		const string CDATA_SECTION_BEGIN_PART = "<![CDATA[";

		/// <summary>
		/// End part of the CDATA section
		/// </summary>
		const string CDATA_SECTION_END_PART = "]]>";

		/// <summary>
		/// Name of the ignoring comment tag
		/// </summary>
		const string IGNORING_COMMENT_TAG_NAME = "wmm:ignore";

		/// <summary>
		/// String representation of the start ignoring comment tag
		/// </summary>
		const string START_IGNORING_COMMENT_TAG = COMMENT_BEGIN_PART + IGNORING_COMMENT_TAG_NAME + COMMENT_END_PART;

		/// <summary>
		/// String representation of the end ignoring comment tag
		/// </summary>
		const string END_IGNORING_COMMENT_TAG = COMMENT_BEGIN_PART + "/" + IGNORING_COMMENT_TAG_NAME + COMMENT_END_PART;

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
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		protected abstract bool ProcessDoctype();

		/// <summary>
		/// Process a comments
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		protected bool ProcessComment()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			int commentStartPosition = _innerContext.Position;
			int commentTextPosition = commentStartPosition + COMMENT_BEGIN_PART.Length;
			int commentEndPosition = content.IndexOf(COMMENT_END_PART, commentTextPosition, StringComparison.Ordinal);

			if (commentEndPosition == -1)
			{
				throw new MarkupParsingException(
					Strings.ErrorMessage_NotClosedComment,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
			}

			int commentTextLength = commentEndPosition - commentTextPosition;
			string commentText = commentTextLength > 0 ?
				content.Substring(commentTextPosition, commentTextLength) : string.Empty;

			switch (commentText)
			{
				case IGNORING_COMMENT_TAG_NAME:
					isProcessed = ProcessStartIgnoringCommentTag();
					break;
				case "/" + IGNORING_COMMENT_TAG_NAME:
					isProcessed = ProcessEndIgnoringCommentTag();
					break;
				default:
					CommonHandlers.Comment?.Invoke(_context, commentText);

					_innerContext.IncreasePosition(commentEndPosition + COMMENT_END_PART.Length - commentStartPosition);
					isProcessed = true;
					break;
			}

			return isProcessed;
		}

		/// <summary>
		/// Process a start ignoring comment tags
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
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
				CommonHandlers.IgnoredFragment?.Invoke(_context, fragment);

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
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		protected bool ProcessEndIgnoringCommentTag()
		{
			throw new MarkupParsingException(
				Strings.ErrorMessage_StartIgnoringCommentTagNotDeclared,
				_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
		}

		/// <summary>
		/// Process a CDATA sections
		/// </summary>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		protected bool ProcessCdataSection()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;

			if (content.CustomStartsWith(CDATA_SECTION_BEGIN_PART, _innerContext.Position, StringComparison.Ordinal))
			{
				int cdataStartPosition = _innerContext.Position;
				int cdataTextPosition = cdataStartPosition + CDATA_SECTION_BEGIN_PART.Length;
				int cdataEndPosition = content.IndexOf(CDATA_SECTION_END_PART, cdataTextPosition, StringComparison.Ordinal);

				if (cdataEndPosition == -1)
				{
					throw new MarkupParsingException(
						Strings.ErrorMessage_NotClosedCdataSection,
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment());
				}

				int cdataTextLength = cdataEndPosition - cdataTextPosition;
				string cdataText = cdataTextLength > 0 ?
					content.Substring(cdataTextPosition, cdataTextLength) : string.Empty;
				CommonHandlers.CdataSection?.Invoke(_context, cdataText);

				_innerContext.IncreasePosition(cdataEndPosition + CDATA_SECTION_END_PART.Length - cdataStartPosition);
				isProcessed = true;
			}

			return isProcessed;
		}

		#endregion
	}
}