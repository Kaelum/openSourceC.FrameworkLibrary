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
	///		Summary description for AbstractAccessPointProvider&lt;TUserRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TUserRequestContext">The <typeparamref name="TUserRequestContext"/> type.</typeparam>
	public abstract class AbstractAccessPointProvider<TUserRequestContext> : AbstractAccessPointProviderBase
		where TUserRequestContext : struct
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractAccessPointProvider&lt;TUserRequestContext&gt;"/>
		///		class. 
		/// </summary>
		/// <param name="userRequestContext">The current <typeparamref name="TUserRequestContext"/> object.</param>
		protected AbstractAccessPointProvider(TUserRequestContext userRequestContext)
			: base() { UserRequestContext = userRequestContext; }

		#endregion

		#region Protected Properties

		/// <summary>Gets the current <see cref="T:TUserRequestContext"/> object.</summary>
		protected TUserRequestContext UserRequestContext { get; private set; }

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

		#region IDisposable Implmentation

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
