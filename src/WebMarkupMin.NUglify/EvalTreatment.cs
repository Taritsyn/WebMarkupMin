namespace WebMarkupMin.NUglify
{
	public enum EvalTreatment
	{
		/// <summary>
		/// Ignore all eval statements. This assumes that code that is eval'd will not attempt
		/// to access any local variables or functions, as those variables and function may be renamed.
		/// </summary>
		Ignore = 0,

		/// <summary>
		/// Assume any code that is eval'd will attempt to access local variables and functions declared
		/// in the same scope as the eval statement. This will turn off local variable and function renaming
		/// in any scope that contains an eval statement.
		/// </summary>
		MakeImmediateSafe,

		/// <summary>
		/// Assume that any local variable or function in any accessible scope chain may be referenced by
		/// code that is eval'd. This will turn off local variable and function renaming for all scopes that
		/// contain an eval statement, and all their parent scopes up the chain to the global scope.
		/// </summary>
		MakeAllSafe
	}
}