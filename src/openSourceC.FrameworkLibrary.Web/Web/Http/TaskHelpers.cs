using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace openSourceC.FrameworkLibrary.Web.Http
{
	/// <summary>
	///		
	/// </summary>
	public static class TaskHelpers
	{
		private static readonly Task _defaultCompleted = FromResult<AsyncVoid>(new AsyncVoid());

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct AsyncVoid { }


		#region Public Static Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="action"></param>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Task RunSynchronously(Action action, CancellationToken token = new CancellationToken())
		{
			if (token.IsCancellationRequested)
			{
				return Canceled();
			}
			try
			{
				action();
				return Completed();
			}
			catch (Exception exception)
			{
				return FromError(exception);
			}
		}

		#endregion

		#region Internal Static Methods

		internal static Task Canceled()
		{
			return CancelCache<AsyncVoid>.Canceled;
		}

		internal static Task Completed()
		{
			return _defaultCompleted;
		}

		internal static Task FromError(Exception exception)
		{
			return FromError<AsyncVoid>(exception);
		}

		internal static Task<TResult> FromError<TResult>(Exception exception)
		{
			TaskCompletionSource<TResult> source = new TaskCompletionSource<TResult>();
			source.SetException(exception);
			return source.Task;
		}

		internal static Task<TResult> FromResult<TResult>(TResult result)
		{
			TaskCompletionSource<TResult> source = new TaskCompletionSource<TResult>();
			source.SetResult(result);
			return source.Task;
		}

		#endregion

		#region Private Classes

		private static class CancelCache<TResult>
		{
			public static readonly Task<TResult> Canceled;

			static CancelCache()
			{
				Canceled = GetCancelledTask();
			}

			private static Task<TResult> GetCancelledTask()
			{
				TaskCompletionSource<TResult> source = new TaskCompletionSource<TResult>();
				source.SetCanceled();
				return source.Task;
			}
		}

		#endregion
	}
}
