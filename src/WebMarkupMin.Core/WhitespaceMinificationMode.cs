namespace WebMarkupMin.Core
{
	/// <summary>
	/// Mode of whitespace minification
	/// </summary>
	public enum WhitespaceMinificationMode
	{
		/// <summary>
		/// Keep whitespace
		/// </summary>
		None = 0,

		/// <summary>
		/// Safe whitespace minification
		/// </summary>
		Safe = 1,

		/// <summary>
		/// Medium whitespace minification
		/// </summary>
		Medium = 2,

		/// <summary>
		/// Aggressive whitespace minification
		/// </summary>
		Aggressive = 3
	}
}