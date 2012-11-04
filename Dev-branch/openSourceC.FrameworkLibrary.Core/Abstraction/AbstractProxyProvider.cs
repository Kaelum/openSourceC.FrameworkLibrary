using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;

namespace openSourceC.FrameworkLibrary.Abstraction
{
	/// <summary>
	///		Summary description for AbstractProxyProvider&lt;TProxyInterface&gt;.
	/// </summary>
	/// <typeparam name="TProxyInterface">The <typeparamref name="TProxyInterface"/> type.</typeparam>
	public abstract class AbstractProxyProvider<TProxyInterface> : AbstractProxyProviderBase
		where TProxyInterface : class
	{
		private TProxyInterface _provider;
		private object _providerLock = new object();


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractProxyProvider&lt;TProxyInterface&gt;"/> class.
		/// </summary>
		/// <param name="settings">The <see cref="ProviderSettings"/> object.</param>
		protected AbstractProxyProvider(ProviderSettings settings)
			: base(settings) { }

		#endregion

		#region Protected Properties

		/// <summary>
		///		Gets the <typeparamref name="TProxyInterface"/> data factory interface of the current
		///		<see cref="AbstractProxyProvider&lt;TConfiguration, TProxyInterface&gt;"/> instance.
		/// </summary>
		protected TProxyInterface Provider
		{
			get
			{
				if (_provider == null)
				{
					lock (_providerLock)
					{
						if (_provider == null)
						{
							_provider = AbstractProvider.Create<TProxyInterface>(Settings);
						}
					}
				}

				return _provider as TProxyInterface;
			}
		}

		#endregion
	}

	/// <summary>
	///		Summary description for AbstractProxyProvider&lt;TUserRequestContext, TProxyInterface&gt;.
	/// </summary>
	/// <typeparam name="TUserRequestContext">The <typeparamref name="TUserRequestContext"/> type.</typeparam>
	/// <typeparam name="TProxyInterface">The <typeparamref name="TProxyInterface"/> type.</typeparam>
	public abstract class AbstractProxyProvider<TUserRequestContext, TProxyInterface> : AbstractProxyProviderBase
		where TUserRequestContext : struct
		where TProxyInterface : class
	{
		private TProxyInterface _provider;
		private object _providerLock = new object();


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractProxyProvider&lt;TUserRequestContext, TProxyInterface&gt;"/>
		///		class. 
		/// </summary>
		/// <param name="settings">The <see cref="ProviderSettings"/> object.</param>
		/// <param name="userRequestContext">The current <typeparamref name="TUserRequestContext"/> object.</param>
		protected AbstractProxyProvider(ProviderSettings settings, TUserRequestContext userRequestContext)
			: base(settings) { UserRequestContext = userRequestContext; }

		#endregion

		#region Protected Properties

		/// <summary>
		///		Gets the <typeparamref name="TProxyInterface"/> data factory interface of the current
		///		<see cref="AbstractProxyProvider&lt;TConfiguration, TProxyInterface&gt;"/> instance.
		/// </summary>
		protected TProxyInterface Provider
		{
			get
			{
				if (_provider == null)
				{
					lock (_providerLock)
					{
						if (_provider == null)
						{
							_provider = AbstractProvider.Create<TProxyInterface>(Settings, UserRequestContext);
						}
					}
				}

				return _provider as TProxyInterface;
			}
		}

		/// <summary>Gets the current <see cref="T:TUserRequestContext"/> object.</summary>
		protected TUserRequestContext UserRequestContext { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for AbstractProxyProviderBase.
	/// </summary>
	public abstract class AbstractProxyProviderBase : ProviderBase, IDisposable
	{
		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		protected AbstractProxyProviderBase()
		{
			Disposed = false;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractProviderBase"/> class.
		/// </summary>
		/// <param name="settings">The <see cref="ProviderSettings"/> object.</param>
		protected AbstractProxyProviderBase(ProviderSettings settings)
			: base()
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			Settings = settings;

			Debug.WriteLine(string.Format("Provider Type: ", Settings.GetType().FullName));

			if (Settings.ElementInformation != null && Settings.ElementInformation.Properties != null)
			{
				foreach (string key in Settings.ElementInformation.Properties.Keys)
				{
					Debug.WriteLine(string.Format("\tProperty: {0} = {1}", key, Settings.ElementInformation.Properties[key]));
				}
			}
		}

		#endregion

		#region Destructor

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~AbstractProxyProviderBase()
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

		#region Protected Properties

		/// <summary>Gets a collection of user-defined parameters.</summary>
		protected NameValueCollection Parameters { get { return Settings.Parameters; } }

		/// <summary>Gets or sets the setting name.</summary>
		protected string SettingName { get { return Settings.Name; } }

		/// <summary>Gets the <see cref="ProviderSettings"/> object.</summary>
		protected ProviderSettings Settings { get; private set; }

		#endregion
	}
}
