// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET451 || NETSTANDARD2_0 || NETCOREAPP3_1_OR_GREATER
using System;
using System.Threading.Tasks;

#if ASPNETCORE1
namespace WebMarkupMin.AspNetCore1.Helpers
#elif ASPNETCORE2
namespace WebMarkupMin.AspNetCore2.Helpers
#elif ASPNETCORE3
namespace WebMarkupMin.AspNetCore3.Helpers
#elif ASPNETCORE5
namespace WebMarkupMin.AspNetCore5.Helpers
#elif ASPNETCORE6
namespace WebMarkupMin.AspNetCore6.Helpers
#elif ASPNETCORE7
namespace WebMarkupMin.AspNetCore7.Helpers
#else
#error No implementation for this target
#endif
{
	/// <summary>
	/// Helper methods for using Tasks to implement the APM pattern
	/// </summary>
	internal static class TaskToApmHelpers
	{
		/// <summary>
		/// Marshals the Task as an IAsyncResult, using the supplied callback and state
		/// to implement the APM pattern
		/// </summary>
		/// <param name="task">The Task to be marshaled</param>
		/// <param name="callback">The callback to be invoked upon completion</param>
		/// <param name="state">The state to be stored in the IAsyncResult</param>
		/// <returns>An IAsyncResult to represent the task's asynchronous operation</returns>
		public static IAsyncResult Begin(Task task, AsyncCallback callback, object state) =>
			new TaskAsyncResult(task, state, callback);

		/// <summary>
		/// Processes an IAsyncResult returned by Begin
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult to unwrap</param>
		public static void End(IAsyncResult asyncResult)
		{
			if (asyncResult is TaskAsyncResult twar)
			{
				twar._task.GetAwaiter().GetResult();
				return;
			}

			throw new ArgumentNullException(nameof(asyncResult));
		}

		/// <summary>
		/// Processes an IAsyncResult returned by Begin
		/// </summary>
		/// <param name="asyncResult">The IAsyncResult to unwrap</param>
		public static TResult End<TResult>(IAsyncResult asyncResult)
		{
			if (asyncResult is TaskAsyncResult twar && twar._task is Task<TResult> task)
			{
				return task.GetAwaiter().GetResult();
			}

			throw new ArgumentNullException(nameof(asyncResult));
		}
	}
}
#endif