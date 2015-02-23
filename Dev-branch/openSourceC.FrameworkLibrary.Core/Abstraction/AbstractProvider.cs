using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;

using openSourceC.FrameworkLibrary.Configuration;

namespace openSourceC.FrameworkLibrary.Abstraction
{
	/// <summary>
	///		Summary description for AbstractProvider.
	/// </summary>
	public abstract class AbstractProvider : AbstractProviderBase
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractProvider"/> class.
		/// </summary>
		/// <param name="settings">The <see cref="SettingSettings"/> object.</param>
		protected AbstractProvider(SettingSettings settings)
			: base(settings) { }

		#endregion
	}

	/// <summary>
	///		Summary description for AbstractProvider&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext">The <typeparamref name="TRequestContext"/> type.</typeparam>
	public abstract class AbstractProvider<TRequestContext> : AbstractProviderBase
		where TRequestContext : struct
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractProvider&lt;TRequestContext&gt;"/>
		///		class. 
		/// </summary>
		/// <param name="settings">The <see cref="SettingSettings"/> object.</param>
		/// <param name="requestContext">The current <typeparamref name="TRequestContext"/> object.</param>
		protected AbstractProvider(SettingSettings settings, TRequestContext requestContext)
			: base(settings) { RequestContext = requestContext; }

		#endregion

		#region Protected Properties

		/// <summary>Gets the current <see cref="T:TRequestContext"/> object.</summary>
		protected TRequestContext RequestContext { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for AbstractProviderBase.
	/// </summary>
	public abstract class AbstractProviderBase : ProviderBase, IDisposable
	{
		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		protected AbstractProviderBase()
		{
			Disposed = false;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="AbstractProviderBase"/> class.
		/// </summary>
		/// <param name="settings">The <see cref="SettingSettings"/> object.</param>
		protected AbstractProviderBase(SettingSettings settings)
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
		~AbstractProviderBase()
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

		#region Create

		/// <summary>
		///		Creates an instance defined by the specified provider settings.
		/// </summary>
		/// <param name="providerSettings">The <see cref="ProviderSettings"/> object.</param>
		/// <param name="args">An array of arguments that match in number, order, and type the
		///		parameters of the constructor to invoke. If args is an empty array or <b>null</b>,
		///		the constructor that takes no parameters (the default constructor) is invoked.</param>
		/// <returns>
		///		A <see cref="T:TReturnType"/> object.
		/// </returns>
		public static TReturnType Create<TReturnType>(ProviderSettings providerSettings, params object[] args)
			where TReturnType : class
		{
			if (providerSettings == null)
			{
				throw new ArgumentNullException("providerSettings");
			}

			Debug.WriteLine(string.Format("Create Provider instance: Name: {0}, Type: {1}", providerSettings.Name, providerSettings.Type));

			if (providerSettings.ElementInformation != null && providerSettings.ElementInformation.Properties != null)
			{
				foreach (PropertyInformation pi in providerSettings.ElementInformation.Properties)
				{
					try { Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value)); }
					catch { }
				}
			}

			Type type = Type.GetType(providerSettings.Type, true);
			object obj = Activator.CreateInstance(type, args);

			if (!(obj is TReturnType))
			{
				throw new OscErrorException(string.Format("Provider {0} does not derive from {1}.", providerSettings.Name, typeof(TReturnType).ToString()));
			}

			ProviderBase providerInstance = obj as ProviderBase;

			if (providerInstance != null)
			{
				providerInstance.Initialize(providerSettings.Name, providerSettings.Parameters);
			}

			TReturnType instance = (TReturnType)obj;

			return instance;
		}

		#endregion

		#region Protected Properties

		/// <summary>Gets the application name.</summary>
		protected string ApplicationName { get { return Settings.ApplicationName; } }

		/// <summary>Gets the connection string name.</summary>
		protected string ConnectionStringName { get { return Settings.ConnectionStringName; } }

		/// <summary>Gets a collection of user-defined parameters.</summary>
		protected NameValueCollection Parameters { get { return Settings.Parameters; } }

		/// <summary>Gets or sets the setting name.</summary>
		protected string SettingName { get { return Settings.Name; } }

		/// <summary>Gets the <see cref="SettingSettings"/> object.</summary>
		protected SettingSettings Settings { get; private set; }

		#endregion
	}
}
