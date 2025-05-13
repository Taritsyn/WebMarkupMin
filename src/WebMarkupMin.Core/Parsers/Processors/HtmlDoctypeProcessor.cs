using System;
using System.Text.RegularExpressions;

using WebMarkupMin.Core.Resources;
using WebMarkupMin.Core.Utilities;

namespace WebMarkupMin.Core.Parsers.Processors
{
	/// <summary>
	/// HTML document type processor
	/// </summary>
	internal sealed class HtmlDoctypeProcessor : IDisposable
	{
		const string DOCTYPE_BEGIN_PART = "<!DOCTYPE";
		const string DOCTYPE_END_PART = ">";
		const string PUBLICITY_PUBLIC = "PUBLIC";
		const string PUBLICITY_SYSTEM = "SYSTEM";
		const string PUBLIC_NAME_PATTERN = @"[a-zA-Z][a-zA-Z0-9\s._:/'""+-]*";

		private static readonly Regex _rootElementRegex = new Regex(@"^" + CommonRegExps.HtmlTagNamePattern,
			TargetFrameworkShortcuts.PerformanceRegexOptions);
		private static readonly string[] _publicities = { PUBLICITY_PUBLIC, PUBLICITY_SYSTEM };
		private static readonly Regex _publicIdRegex = new Regex("^" +
			"(?:" +
				@"(?:(?<registration>[-+])//(?<organization>" + PUBLIC_NAME_PATTERN + "))" +
				"|" +
				@"(?<organization>ISO/" + PUBLIC_NAME_PATTERN + ")" +
			")" +
			@"//(?<type>[A-Z]+)" +
			@"\s+(?<name>" + PUBLIC_NAME_PATTERN + ")//(?<language>[A-Z]{2})" +
			@"(?://(?<version>\d+\.\d+)?)?" +
			"$",
			TargetFrameworkShortcuts.PerformanceRegexOptions)
			;

		private InnerMarkupParsingContext _innerContext;


		/// <summary>
		/// Constructs an instance of the HTML document type processor
		/// </summary>
		/// <param name="innerContext">Inner markup parsing context</param>
		public HtmlDoctypeProcessor(InnerMarkupParsingContext innerContext)
		{
			_innerContext = innerContext;
		}


		/// <summary>
		/// Process a document type declaration
		/// </summary>
		/// <param name="doctype">Document type declaration</param>
		/// <returns>Result of processing (<c>true</c> - is processed; <c>false</c> - is not processed)</returns>
		public bool Process(out HtmlDoctype doctype)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			doctype = null;

			if (ProcessDoctypeBeginPart(out string instruction))
			{
				bool spaceBeforeRootElement = ProcessWhitespace();
				SourceCodeNodeCoordinates rootElementCoordinates = _innerContext.NodeCoordinates;

				if (ProcessRootElement(out string rootElement))
				{
					ProcessWhitespace();
					isProcessed = ProcessDoctypeEndPart();
					if (isProcessed)
					{
						doctype = new HtmlDoctype(instruction, spaceBeforeRootElement, rootElement);

						return isProcessed;
					}

					if (!spaceBeforeRootElement)
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_MissingSpaceBeforeDoctypeRootElement,
							rootElementCoordinates,
							SourceCodeNavigator.GetSourceFragment(content, rootElementCoordinates)
						);
					}
				}
				else
				{
					throw new MarkupParsingException(
						Strings.ErrorMessage_DoctypeNotContainRootElement,
						_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
					);
				}

				ProcessWhitespace();
				if (ProcessPublicity(out string publicity))
				{
					HtmlFormalPublicId publicId = null;
					HtmlSystemId systemId = null;

					if (publicity.IgnoreCaseEquals(PUBLICITY_PUBLIC))
					{
						bool spaceBeforePublicId = ProcessWhitespace();
						SourceCodeNodeCoordinates publicIdCoordinates = _innerContext.NodeCoordinates;

						if (ProcessPublicId(out publicId))
						{
							if (!spaceBeforePublicId)
							{
								throw new MarkupParsingException(
									string.Format(Strings.ErrorMessage_NoSpaceBetweenDoctypePublicityAndId, publicity),
									publicIdCoordinates,
									SourceCodeNavigator.GetSourceFragment(content, publicIdCoordinates)
								);
							}
						}
						else
						{
							throw new MarkupParsingException(
								Strings.ErrorMessage_FormalPublicIdNotFound,
								_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
							);
						}
					}

					bool spaceBeforeSystemId = ProcessWhitespace();
					SourceCodeNodeCoordinates systemIdCoordinates = _innerContext.NodeCoordinates;

					if (ProcessSystemId(out systemId))
					{
						if (!spaceBeforeSystemId && publicity.IgnoreCaseEquals(PUBLICITY_SYSTEM))
						{
							throw new MarkupParsingException(
								string.Format(Strings.ErrorMessage_NoSpaceBetweenDoctypePublicityAndId, publicity),
								systemIdCoordinates,
								SourceCodeNavigator.GetSourceFragment(content, systemIdCoordinates)
							);
						}
					}
					else if (publicity.IgnoreCaseEquals(PUBLICITY_SYSTEM))
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_SystemIdNotFound,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
						);
					}

					ProcessWhitespace();
					isProcessed = ProcessDoctypeEndPart();
					if (isProcessed)
					{
						doctype = new HtmlDoctype(instruction, spaceBeforeRootElement, rootElement, publicity,
							publicId, systemId);

						return isProcessed;
					}
				}

				throw new MarkupParsingException(
					Strings.ErrorMessage_BogusDoctype,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
				);
			}

			return isProcessed;
		}

		private bool ProcessDoctypeBeginPart(out string instruction)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;
			instruction = string.Empty;

			int doctypeBeginPartLength = DOCTYPE_BEGIN_PART.Length;
			int doctypeBeginPartPosition = content.IndexOf(DOCTYPE_BEGIN_PART, currentPosition, doctypeBeginPartLength,
				StringComparison.OrdinalIgnoreCase);

			if (doctypeBeginPartPosition != -1)
			{
				const int angleBracketAndExclamationLength = 2;
				int instructionPosition = currentPosition + angleBracketAndExclamationLength;
				int instructionLength = doctypeBeginPartLength - angleBracketAndExclamationLength;
				instruction = content.Substring(instructionPosition, instructionLength);

				_innerContext.IncreasePosition(doctypeBeginPartLength);
				isProcessed = true;
			}

			return isProcessed;
		}

		private bool ProcessWhitespace()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;

			int nonWhitespaceCharPosition = content.IndexOfNonWhitespace(currentPosition,
				_innerContext.RemainderLength);
			if (nonWhitespaceCharPosition != -1)
			{
				int whitespaceLength = nonWhitespaceCharPosition - currentPosition;
				if (whitespaceLength > 0)
				{
					_innerContext.IncreasePosition(whitespaceLength);
					isProcessed = true;
				}
			}

			return isProcessed;
		}

		private bool ProcessDoctypeEndPart()
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;

			int doctypeEndPartLength = DOCTYPE_END_PART.Length;
			int doctypeEndPartPosition = content.IndexOf(DOCTYPE_END_PART, currentPosition, doctypeEndPartLength);

			if (doctypeEndPartPosition != -1)
			{
				_innerContext.IncreasePosition(doctypeEndPartLength);
				isProcessed = true;
			}

			return isProcessed;
		}

		private bool ProcessRootElement(out string rootElement)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			rootElement = string.Empty;

			Match rootElementMatch = _rootElementRegex.Match(content, _innerContext.Position,
				_innerContext.RemainderLength);
			if (rootElementMatch.Success)
			{
				rootElement = rootElementMatch.Value;

				_innerContext.IncreasePosition(rootElementMatch.Length);
				isProcessed = true;
			}

			return isProcessed;
		}

		private bool ProcessPublicity(out string publicity)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;
			publicity = string.Empty;

			foreach (string publicitySample in _publicities)
			{
				int publicityLength = publicitySample.Length;
				int publicityPosition = content.IndexOf(publicitySample, currentPosition, publicityLength,
					StringComparison.OrdinalIgnoreCase);

				if (publicityPosition != -1)
				{
					publicity = content.Substring(publicityPosition, publicityLength);

					_innerContext.IncreasePosition(publicityLength);
					isProcessed = true;

					break;
				}
			}

			return isProcessed;
		}

		private bool ProcessPublicId(out HtmlFormalPublicId publicId)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;
			publicId = null;

			GetQuotePositions(content, currentPosition, out int beginQuoteCharPosition, out int endQuoteCharPosition);

			if (beginQuoteCharPosition != -1)
			{
				const int quoteCharLength = 1;
				_innerContext.IncreasePosition(quoteCharLength);

				if (endQuoteCharPosition != -1)
				{
					int valuePosition = beginQuoteCharPosition + quoteCharLength;
					int valueLength = endQuoteCharPosition - beginQuoteCharPosition - quoteCharLength;
					if (valueLength == 0)
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_EmptyFormalPublicId,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
						);
					}

					Match publicIdMatch = _publicIdRegex.Match(content, valuePosition, valueLength);
					if (publicIdMatch.Success)
					{
						var publicIdGroups = publicIdMatch.Groups;
						string registration = publicIdGroups["registration"].Value;
						string organization = publicIdGroups["organization"].Value;
						string type = publicIdGroups["type"].Value;
						string name = publicIdGroups["name"].Value;
						string language = publicIdGroups["language"].Value;
						Group versionGroup = publicIdGroups["version"];
						string version = versionGroup.Success ? versionGroup.Value : string.Empty;
						char quoteChar = content[beginQuoteCharPosition];

						publicId = new HtmlFormalPublicId(registration, organization, type, name, language,
							version, quoteChar);

						int publicIdRemainderLength = valuePosition + valueLength + quoteCharLength -
							_innerContext.Position;
						_innerContext.IncreasePosition(publicIdRemainderLength);
						isProcessed = true;

						return isProcessed;
					}
				}

				throw new MarkupParsingException(
					Strings.ErrorMessage_InvalidFormalPublicId,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
				);
			}

			return isProcessed;
		}

		private bool ProcessSystemId(out HtmlSystemId systemId)
		{
			bool isProcessed = false;
			string content = _innerContext.SourceCode;
			int currentPosition = _innerContext.Position;
			systemId = null;

			GetQuotePositions(content, currentPosition, out int beginQuoteCharPosition, out int endQuoteCharPosition);

			if (beginQuoteCharPosition != -1)
			{
				const int quoteCharLength = 1;
				_innerContext.IncreasePosition(quoteCharLength);

				if (endQuoteCharPosition != -1)
				{
					int valuePosition = beginQuoteCharPosition + quoteCharLength;
					int valueLength = endQuoteCharPosition - beginQuoteCharPosition - quoteCharLength;
					if (valueLength == 0)
					{
						throw new MarkupParsingException(
							Strings.ErrorMessage_EmptySystemId,
							_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
						);
					}

					string url = content.Substring(valuePosition, valueLength);
					if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
					{
						char quoteChar = content[beginQuoteCharPosition];
						systemId = new HtmlSystemId(url, quoteChar);

						int identifierRemainderLength = valuePosition + valueLength + quoteCharLength -
							_innerContext.Position;
						_innerContext.IncreasePosition(identifierRemainderLength);
						isProcessed = true;

						return isProcessed;
					}
				}

				throw new MarkupParsingException(
					Strings.ErrorMessage_InvalidSystemId,
					_innerContext.NodeCoordinates, _innerContext.GetSourceFragment()
				);
			}

			return isProcessed;
		}

		private static void GetQuotePositions(string sourceCode, int position,
			out int beginQuotePosition, out int endQuotePosition)
		{
			char currentCharValue;
			beginQuotePosition = -1;
			endQuotePosition = -1;

			if (sourceCode.TryGetChar(position, out currentCharValue))
			{
				char quoteChar = currentCharValue == '"' || currentCharValue == '\'' ?
					currentCharValue : '\0';

				if (quoteChar != '\0')
				{
					beginQuotePosition = position;
					endQuotePosition = sourceCode.IndexOf(quoteChar, position + 1);
				}
			}
		}


		#region IDisposable implementation

		/// <summary>
		/// Disposes the HTML document type processor
		/// </summary>
		public void Dispose()
		{
			_innerContext = null;
		}

		#endregion
	}
}