using System;
using System.Configuration.Provider;

namespace openSourceC.FrameworkLibrary.Abstraction
{
	/// <summary>
	///		Summary description for AbstractAccessPointProvider.
	/// </summary>
	public abstract class AbstractAccessPointProvider : AbstractAccessPointProviderBase
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractAccessPointProvider"/> class.
		/// </summary>
		protected AbstractAccessPointProvider() : base() { }

		#endregion
	}

	/// <summary>
	///		Summary description for AbstractAccessPointProvider&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext">The <typeparamref name="TRequestContext"/> type.</typeparam>
	public abstract class AbstractAccessPointProvider<TRequestContext> : AbstractAccessPointProviderBase
		where TRequestContext : struct
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractAccessPointProvider&lt;TRequestContext&gt;"/>
		///		class. 
		/// </summary>
		/// <param name="requestContext">The current <typeparamref name="TRequestContext"/> object.</param>
		protected AbstractAccessPointProvider(TRequestContext requestContext)
			: base() { RequestContext = requestContext; }

		#endregion

		#region Protected Properties

		/// <summary>Gets the current <see cref="T:TRequestContext"/> object.</summary>
		protected TRequestContext RequestContext { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for AbstractAccessPointProviderBase.
	/// </summary>
	public abstract class AbstractAccessPointProviderBase : ProviderBase, IDisposable
	{
		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		protected AbstractAccessPointProviderBase()
		{
			Disposed = false;
		}

		#endregion

		#region Destructor

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~AbstractAccessPointProviderBase()
		{
			Dispose(false);
		}

		#endregion

		#region IDisposable Implementation

		/// <summary></summary>
		protected bool Disposed { get; private set; }

		/// <summary>
		///		Dispose of 
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}


		/// <summary>
		///		Dispose(bool disposing) executes in two distinct scenarios.  If disposing equals
		///		<b>true</b>, <see cref="M:Dispose()"/> has been called directly or indirectly
		///		by a user's code.  Managed and unmanaged resources can be disposed.  If disposing
		///		equals <b>false</b>, <see cref="M:Dispose()"/> has been called by the runtime from
		///		inside the finalizer and you should not reference other objects.  Only unmanaged
		///		resources can be disposed.
		/// </summary>
		/// <param name="disposing"><b>true</b> when <see cref="M:Dispose()"/> has been called
		///		directly or indirectly by a user's code.  <b>false</b> when <see cref="M:Dispose()"/>
		///		has been called by the runtime from inside the finalizer and you should not
		///		reference other objects.</param>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!Disposed)
			{
				// If disposing equals true, dispose of managed resources.
				if (disposing)
				{
					// Dispose of managed resources.
				}

				// Dispose of unmanaged resources.

				Disposed = true;
			}
		}

		#endregion
	}
}
