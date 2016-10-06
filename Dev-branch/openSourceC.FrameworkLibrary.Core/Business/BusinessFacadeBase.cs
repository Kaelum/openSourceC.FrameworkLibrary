using System;

namespace openSourceC.FrameworkLibrary.Business
{
	/// <summary>
	///		Summary description for BusinessFacadeBase.
	/// </summary>
	public abstract class BusinessFacadeBase : IDisposable
	{
		private bool _disposed = false;


		#region IDisposable Implementation

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~BusinessFacadeBase()
		{
			Dispose(false);
		}

		/// <summary>
		///		Dispose of 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the
		// runtime from inside the finalizer and you should not reference
		// other objects. Only unmanaged resources can be disposed.
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose of managed resources.
				if (disposing)
				{
					DisposeManagedResources();
				}

				DisposeUnmanagedResources();

				_disposed = true;
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		///		Disposes of managed resources.
		/// </summary>
		protected abstract void DisposeManagedResources();

		/// <summary>
		///		Disposes of unmanaged resources.
		/// </summary>
		protected abstract void DisposeUnmanagedResources();

		#endregion
	}
}
