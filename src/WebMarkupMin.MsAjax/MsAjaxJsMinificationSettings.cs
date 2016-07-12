namespace WebMarkupMin.MsAjax
{
	/// <summary>
	/// Microsoft Ajax JS Minifier settings
	/// </summary>
	public sealed class MsAjaxJsMinificationSettings : MsAjaxCommonMinificationSettingsBase
	{
		/// <summary>
		/// Gets or sets a flag indicating whether to always escape non-ASCII characters as <code>\uXXXX</code>
		/// or to let the output encoding object handle that via the <code>JsEncoderFallback</code> object for the
		/// specified output encoding format
		/// </summary>
		public bool AlwaysEscapeNonAscii
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to perform extra tasks on AMD-style defines
		/// </summary>
		public bool AmdSupport
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to collapse <code>new Array()</code>
		/// to <code>[]</code> and <code>new Object()</code> to <code>{}</code>
		/// (true) or leave as-is (false)
		/// </summary>
		public bool CollapseToLiteral
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether to use old-style
		/// const statements (just var-statements that define unchangeable fields)
		/// or new EcmaScript 6 lexical declarations
		/// </summary>
		public bool ConstStatementsMozilla
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a string representation of the list of debug
		/// lookups (comma-separated)
		/// </summary>
		public string DebugLookupList
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to throw an error
		/// if a source string is not safe for inclusion in an
		/// HTML inline script block
		/// </summary>
		public bool ErrorIfNotInlineSafe
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to evaluate expressions containing
		/// only literal bool, string, numeric, or null values (true).
		/// Leave literal expressions alone and do not evaluate them (false).
		/// </summary>
		public bool EvalLiteralExpressions
		{
			get;
			set;
		}

		/// <summary>
		/// EvalTreatment setting
		/// </summary>
		public EvalTreatment EvalTreatment
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether or not to ignore conditional-compilation
		/// comment syntax (true) or to try to retain the comments in the output (false)
		/// </summary>
		public bool IgnoreConditionalCompilation
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether or not to ignore preprocessor
		/// defines comment syntax (true) or to evaluate them (false)
		/// </summary>
		public bool IgnorePreprocessorDefines
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag for whether to break up string literals containing
		/// <code>&lt;/script&gt;</code> so inline code won't break (true).
		/// Leave string literals as-is (false).
		/// </summary>
		public bool InlineSafeStrings
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the known global names list as a single comma-separated string
		/// </summary>
		public string KnownGlobalNamesList
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to how to rename local variables and functions:
		/// <code>KeepAll</code> - do not rename local variables and functions;
		/// <code>CrunchAll</code> - rename all local variables and functions to shorter names;
		/// <code>KeepLocalizationVars</code> - rename all local variables and functions that do NOT start with L_
		/// </summary>
		public LocalRenaming LocalRenaming
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to add characters to the output
		/// to make sure Mac Safari bugs are not generated (true).
		/// Disregard potential Mac Safari bugs (false).
		/// </summary>
		public bool MacSafariQuirks
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether object property
		/// names with the specified "from" names will get renamed to
		/// the corresponding "to" names (true) when using
		/// the manual-rename feature, or left alone (false)
		/// </summary>
		public bool ManualRenamesProperties
		{
			get;
			set;
		}

		/// <summary>
		/// Get or sets the no-automatic-renaming list as a single string of
		/// comma-separated identifiers
		/// </summary>
		public string NoAutoRenameList
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether all function names
		/// must be preserved and remain as-named
		/// </summary>
		public bool PreserveFunctionNames
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to preserve important
		/// comments in the output
		/// </summary>
		public bool PreserveImportantComments
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to always quote object literal property names
		/// </summary>
		public bool QuoteObjectLiteralProperties
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not to remove
		/// unreferenced function expression names
		/// </summary>
		public bool RemoveFunctionExpressionNames
		{
			get;
			set;
		}

		/// <summary>
		/// Remove unneeded code, like uncalled local functions (true).
		/// Keep all code (false).
		/// </summary>
		public bool RemoveUnneededCode
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a string representation of all the indentifier
		/// replacements as a comma-separated list of "source=target" identifiers
		/// </summary>
		public string RenamePairs
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether or not to reorder function and variable
		/// declarations within scopes (true), or to leave the order as specified in
		/// the original source
		/// </summary>
		public bool ReorderScopeDeclarations
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether or not to force
		/// the input code into strict mode (can still specify strict-mode in
		/// the sources if this value is false)
		/// </summary>
		public bool StrictMode
		{
			get;
			set;
		}

		/// <summary>
		/// Strip debug statements (true).
		/// Leave debug statements (false).
		/// </summary>
		public bool StripDebugStatements
		{
			get;
			set;
		}


		/// <summary>
		/// Constructs an instance of the Microsoft Ajax JS Minifier settings
		/// </summary>
		public MsAjaxJsMinificationSettings()
		{
			AlwaysEscapeNonAscii = false;
			AmdSupport = false;
			CollapseToLiteral = true;
			ConstStatementsMozilla = false;
			DebugLookupList = "Debug,$Debug,WAssert,Msn.Debug,Web.Debug";
			ErrorIfNotInlineSafe = false;
			EvalLiteralExpressions = true;
			EvalTreatment = EvalTreatment.Ignore;
			IgnoreConditionalCompilation = false;
			IgnorePreprocessorDefines = false;
			InlineSafeStrings = true;
			KnownGlobalNamesList = string.Empty;
			LocalRenaming = LocalRenaming.CrunchAll;
			MacSafariQuirks = true;
			ManualRenamesProperties = true;
			NoAutoRenameList = "$super";
			PreserveFunctionNames = false;
			PreserveImportantComments = true;
			QuoteObjectLiteralProperties = false;
			RemoveFunctionExpressionNames = true;
			RemoveUnneededCode = true;
			RenamePairs = string.Empty;
			ReorderScopeDeclarations = true;
			StrictMode = false;
			StripDebugStatements = true;
		}
	}
}