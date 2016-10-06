using System;
using System.Configuration;

using openSourceC.FrameworkLibrary.Configuration;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for RemoteFactoryProviderAccessor.
	/// </summary>
	[Obsolete("This object is now obsolete.  Replace all uses with DbProxyProvider.")]
	public abstract class RemoteFactoryProviderAccessor : IDisposable
	{
		#region Constructors

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProviderAccessor"/> using the
		///		default logger.
		/// </summary>
		/// <param name="settingsElements">The <see cref="T:NamedProviderElementCollection"/> object.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		public RemoteFactoryProviderAccessor(NamedProviderElementCollection settingsElements, string nameSuffix)
			: this(OscLog.Instance, settingsElements, nameSuffix) { }

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProviderAccessor"/>.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="settingsElements">The <see cref="T:NamedProviderElementCollection"/> object.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		public RemoteFactoryProviderAccessor(OscLog log, NamedProviderElementCollection settingsElements, string nameSuffix)
		{
			if (settingsElements == null)
			{
				throw new ArgumentNullException("settingsElements");
			}

			Disposed = false;

			Log = log;
			SettingsElements = settingsElements;
			NameSuffix = nameSuffix;
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~RemoteFactoryProviderAccessor()
		{
			Dispose(false);
		}

		/// <summary>Gets a value indicating that the object has been disposed.</summary>
		protected bool Disposed { get; private set; }

		/// <summary>
		///		Dispose of object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		/// <summary>
		///		Dispose(bool disposing) executes in two distinct scenarios.
		/// </summary>
		/// <param name="disposing"><b>true</b> indicates that the method has been called directly
		///		or indirectly by a user's code and that both managed and unmanaged resources can be
		///		disposed; otherwise, <b>false</b> indicates that the method has been called by the
		///		runtime from inside the finalizer and that only unmanaged resources can be
		///		disposed.</param>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!Disposed)
			{
				// If disposing equals true, dispose of managed resources.
				if (disposing)
				{
					SettingsElements = null;
				}

				Disposed = true;
			}
		}

		#endregion

		#region Protected Properties

		/// <summary>Gets the <see cref="T:OscLog"/> object.</summary>
		protected OscLog Log { get; private set; }

		/// <summary>Gets the name suffix.</summary>
		protected string NameSuffix { get; private set; }

		/// <summary>Gets the <see cref="T:NamedProviderElementCollection"/> object.</summary>
		protected NamedProviderElementCollection SettingsElements { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for RemoteFactoryProviderAccessor&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext">The <typeparamref name="TRequestContext"/> type.</typeparam>
	[Obsolete("This object is now obsolete.  Replace all uses with DbProxyProvider.")]
	public abstract class RemoteFactoryProviderAccessor<TRequestContext> : RemoteFactoryProviderAccessor
		where TRequestContext : struct
	{
		#region Constructors

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProviderAccessor&lt;TRequestContext&gt;"/>
		///		using the default logger..
		/// </summary>
		/// <param name="requestContext">The <see cref="T:TRequestContext"/> object.</param>
		/// <param name="settingsElements">The <see cref="T:NamedProviderElementCollection"/> object.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		public RemoteFactoryProviderAccessor(TRequestContext requestContext, NamedProviderElementCollection settingsElements, string nameSuffix)
			: base(settingsElements, nameSuffix)
		{
			RequestContext = requestContext;
		}

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProviderAccessor&lt;TRequestContext&gt;"/>.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="requestContext">The <see cref="T:TRequestContext"/> object.</param>
		/// <param name="settingsElements">The <see cref="T:NamedProviderElementCollection"/> object.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		public RemoteFactoryProviderAccessor(OscLog log, TRequestContext requestContext, NamedProviderElementCollection settingsElements, string nameSuffix)
			: base(log, settingsElements, nameSuffix)
		{
			RequestContext = requestContext;
		}

		#endregion

		#region Protected Properties

		/// <summary>Gets the current <see cref="T:TRequestContext"/> object.</summary>
		protected TRequestContext RequestContext { get; private set; }

		#endregion
	}
}
