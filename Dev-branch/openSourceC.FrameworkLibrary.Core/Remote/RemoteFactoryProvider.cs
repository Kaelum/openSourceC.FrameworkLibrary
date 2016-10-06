using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Reflection;

using openSourceC.FrameworkLibrary.Configuration;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for RemoteFactoryProvider.
	/// </summary>
	[Obsolete("This object is now obsolete.  Replace all uses with DbAbstractProvider.")]
	public abstract class RemoteFactoryProvider : RemoteFactoryProviderBase
	{
		#region Constructors

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider"/> using the default
		///		logger.
		/// </summary>
		/// <param name="settingSettings">The <see cref="T:SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The setting name.  If null, the <see cref="P:Settings"/>,
		///		<see cref="P:ApplicationName"/>, and <see cref="P:ConnectionStringName"/> properties
		///		will not be usable until the <see cref="P:SettingName"/> property is set.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		protected RemoteFactoryProvider(SettingSettingsCollection settingSettings, string settingName, string nameSuffix)
			: base(OscLog.Instance, settingSettings, settingName, nameSuffix) { }

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider"/>.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="settingSettings">The <see cref="T:SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The setting name.  If null, the <see cref="P:Settings"/>,
		///		<see cref="P:ApplicationName"/>, and <see cref="P:ConnectionStringName"/> properties
		///		will not be usable until the <see cref="P:SettingName"/> property is set.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		protected RemoteFactoryProvider(OscLog log, SettingSettingsCollection settingSettings, string settingName, string nameSuffix)
			: base(log, settingSettings, settingName, nameSuffix) { }

		#endregion

		#region CreateInstance (static)

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider"/> that implements
		///		<typeparamref name="TInterface"/> using the default logger.
		/// </summary>
		/// <typeparam name="TInterface">The <typeparamref name="TInterface"/> type.</typeparam>
		/// <param name="settingsElements">The <see cref="NamedProviderElementCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		/// <returns>
		///		A <see cref="T:RemoteFactoryProvider"/> instance casted as <typeparamref name="TInterface"/>.
		/// </returns>
		public static TInterface CreateInstance<TInterface>(NamedProviderElementCollection settingsElements, string providerName, string nameSuffix)
			where TInterface : class
		{
			return CreateInstance<TInterface>(OscLog.Instance, settingsElements, providerName, nameSuffix);
		}

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider"/> that implements
		///		<typeparamref name="TInterface"/>.
		/// </summary>
		/// <typeparam name="TInterface">The <typeparamref name="TInterface"/> type.</typeparam>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="settingsElements">The <see cref="NamedProviderElementCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		/// <returns>
		///		A <see cref="T:RemoteFactoryProvider"/> instance casted as <typeparamref name="TInterface"/>.
		/// </returns>
		public static TInterface CreateInstance<TInterface>(OscLog log, NamedProviderElementCollection settingsElements, string providerName, string nameSuffix)
			where TInterface : class
		{
			try
			{
				NamedProviderElement settings = GetSettings(settingsElements, providerName, nameSuffix);
				Type type = Type.GetType(settings.Type, true);
				RemoteFactoryProvider instance;

				ConstructorInfo constructor;

				if ((constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(OscLog), typeof(string), }, null)) != null)
				{
					instance = constructor.Invoke(new object[] { log, nameSuffix, }) as RemoteFactoryProvider;
				}
				else if (string.IsNullOrEmpty(nameSuffix) && (constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(OscLog), }, null)) != null)
				{
					instance = constructor.Invoke(new object[] { log, }) as RemoteFactoryProvider;
				}
				else
				{
					throw new OscErrorException(string.Format("Compatible constructor not found to create an instance of \"{0}\".", settings.Type));
				}

				if (instance == null)
				{
					throw new OscErrorException(string.Format("{0} does not derive from RemoteFactoryProviderBase.", settings.Name));
				}

				if (!(instance is TInterface))
				{
					throw new OscErrorException(string.Format("{0} does not derive from {1}.", settings.Name, typeof(TInterface).ToString()));
				}

				instance.Initialize(settings.Name, settings.Parameters);

				return instance as TInterface;
			}
			catch (Exception ex)
			{
				string exceptionMessage = string.Format("Unable to create an instance of {0} from the '{1}{2}' provider.", typeof(TInterface), providerName, nameSuffix);

				log.Error(exceptionMessage, ex);

				throw new OscErrorException(exceptionMessage, ex)
				{
					IsLogged = true,
				};
			}
		}

		#endregion
	}

	/// <summary>
	///		Summary description for RemoteFactoryProvider&lt;TRequestContext&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext">The <typeparamref name="TRequestContext"/> type.</typeparam>
	[Obsolete("This object is now obsolete.  Replace all uses with DbAbstractProvider.")]
	public abstract class RemoteFactoryProvider<TRequestContext> : RemoteFactoryProviderBase
		where TRequestContext : struct
	{
		#region Constructors

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider&lt;TRequestContext&gt;"/>
		///		using the default logger. 
		/// </summary>
		/// <param name="requestContext">The <see cref="T:TRequestContext"/> object.</param>
		/// <param name="settingSettings">The <see cref="T:SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The setting name.  If null, the <see cref="P:Settings"/>,
		///		<see cref="P:ApplicationName"/>, and <see cref="P:ConnectionStringName"/> properties
		///		will not be usable until the <see cref="P:SettingName"/> property is set.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		protected RemoteFactoryProvider(TRequestContext requestContext, SettingSettingsCollection settingSettings, string settingName, string nameSuffix)
			: base(OscLog.Instance, settingSettings, settingName, nameSuffix)
		{
			RequestContext = requestContext;
		}

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider&lt;TRequestContext&gt;"/>.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="requestContext">The <see cref="T:TRequestContext"/> object.</param>
		/// <param name="settingSettings">The <see cref="T:SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The setting name.  If null, the <see cref="P:Settings"/>,
		///		<see cref="P:ApplicationName"/>, and <see cref="P:ConnectionStringName"/> properties
		///		will not be usable until the <see cref="P:SettingName"/> property is set.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		protected RemoteFactoryProvider(OscLog log, TRequestContext requestContext, SettingSettingsCollection settingSettings, string settingName, string nameSuffix)
			: base(log, settingSettings, settingName, nameSuffix)
		{
			RequestContext = requestContext;
		}

		#endregion

		#region CreateInstance (static)

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider&lt;TRequestContext&gt;"/>
		///		that implements <typeparamref name="TInterface"/> using the default logger.
		/// </summary>
		/// <typeparam name="TInterface">The <typeparamref name="TInterface"/> type.</typeparam>
		/// <param name="requestContext">The <typeparamref name="TRequestContext"/> object.</param>
		/// <param name="settingsElements">The <see cref="NamedProviderElementCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		/// <returns>
		///		A <see cref="T:RemoteFactoryProvider&lt;TRequestContext&gt;"/> instance casted as
		///		<typeparamref name="TInterface"/>.
		/// </returns>
		public static TInterface CreateInstance<TInterface>(TRequestContext requestContext, NamedProviderElementCollection settingsElements, string providerName, string nameSuffix)
			where TInterface : class
		{
			return CreateInstance<TInterface>(OscLog.Instance, requestContext, settingsElements, providerName, nameSuffix);
		}

		/// <summary>
		///		Creates an instance of <see cref="T:RemoteFactoryProvider&lt;TRequestContext&gt;"/>
		///		that implements <typeparamref name="TInterface"/>.
		/// </summary>
		/// <typeparam name="TInterface">The <typeparamref name="TInterface"/> type.</typeparam>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="requestContext">The <typeparamref name="TRequestContext"/> object.</param>
		/// <param name="settingsElements">The <see cref="NamedProviderElementCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		/// <returns>
		///		A <see cref="T:RemoteFactoryProvider&lt;TRequestContext&gt;"/> instance casted as
		///		<typeparamref name="TInterface"/>.
		/// </returns>
		public static TInterface CreateInstance<TInterface>(OscLog log, TRequestContext requestContext, NamedProviderElementCollection settingsElements, string providerName, string nameSuffix)
			where TInterface : class
		{
			try
			{
				NamedProviderElement settings = GetSettings(settingsElements, providerName, nameSuffix);
				Type type = Type.GetType(settings.Type, true);
				RemoteFactoryProvider<TRequestContext> instance;

				ConstructorInfo constructor;

				if ((constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(OscLog), typeof(TRequestContext), typeof(string), }, null)) != null)
				{
					instance = constructor.Invoke(new object[] { log, requestContext, nameSuffix, }) as RemoteFactoryProvider<TRequestContext>;
				}
				else if (string.IsNullOrEmpty(nameSuffix) && (constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(OscLog), typeof(TRequestContext), }, null)) != null)
				{
					instance = constructor.Invoke(new object[] { log, requestContext, }) as RemoteFactoryProvider<TRequestContext>;
				}
				else
				{
					throw new OscErrorException(string.Format("Compatible constructor not found to create an instance of \"{0}\".", settings.Type));
				}

				if (instance == null)
				{
					throw new OscErrorException(string.Format("{0} does not derive from RemoteFactoryProvider<{1}>.", settings.Name, typeof(TRequestContext).ToString()));
				}

				if (!(instance is TInterface))
				{
					throw new OscErrorException(string.Format("{0} does not derive from {1}.", settings.Name, typeof(TInterface).ToString()));
				}

				instance.Initialize(settings.Name, settings.Parameters);

				return instance as TInterface;
			}
			catch (Exception ex)
			{
				string exceptionMessage = string.Format("Unable to create an instance of {0} from the '{1}{2}' provider.", typeof(TInterface), providerName, nameSuffix);

				log.Error(exceptionMessage, ex);

				throw new OscErrorException(exceptionMessage, ex)
				{
					IsLogged = true,
				};
			}
		}

		#endregion

		#region Protected Properties

		/// <summary>Gets the current <see cref="T:TRequestContext"/> object.</summary>
		protected TRequestContext RequestContext { get; private set; }

		#endregion
	}

	/// <summary>
	///		Summary description for RemoteFactoryProviderBase.
	/// </summary>
	[Obsolete("Replace this with the Abstraction system.")]
	public abstract class RemoteFactoryProviderBase : ProviderBase, IDisposable
	{
		private OscLog _log;
		private SettingSettingsCollection _settingSettings;
		private string _settingName;
		private string _nameSuffix;
		private string _fullSettingName;


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <see cref="T:RemoteFactoryProviderBase"/> class.
		/// </summary>
		/// <param name="log">The <see cref="T:OscLog"/> object.</param>
		/// <param name="settingSettings">The <see cref="T:SettingSettingsCollection"/> object.</param>
		/// <param name="settingName">The setting name.  If null, the <see cref="P:Settings"/>,
		///		<see cref="P:ApplicationName"/>, and <see cref="P:ConnectionStringName"/> properties
		///		will not be usable until the <see cref="P:SettingName"/> property is set.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		protected RemoteFactoryProviderBase(OscLog log, SettingSettingsCollection settingSettings, string settingName, string nameSuffix)
		{
			if (log == null)
			{
				throw new ArgumentNullException("log");
			}

			if (settingSettings == null)
			{
				throw new ArgumentNullException("settingSettings");
			}

			_log = log;
			_settingSettings = settingSettings;
			_settingName = settingName;
			_nameSuffix = nameSuffix;
			_fullSettingName = settingName + nameSuffix;

			if (_settingName != null)
			{
				if (Settings == null)
				{
					throw new OscErrorException(string.Format("Provider {0}{1} not found.", _settingName, _nameSuffix));
				}

				Debug.WriteLine(string.Format("DataFactorySettings: Name: {0}, ConnectionStringName: {1}", Settings.Name, Settings.ConnectionStringName));

				if (Settings.ElementInformation != null && Settings.ElementInformation.Properties != null)
				{
					foreach (PropertyInformation pi in Settings.ElementInformation.Properties)
					{
						try { Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value)); }
						catch { }
					}
				}
			}
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~RemoteFactoryProviderBase()
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
				}

				Disposed = true;
			}
		}

		#endregion

		#region Initialize

		/// <summary>
		///		Initializes the factory with the property values specified in application's
		///		configuration file. This method is not intended to be used directly from your code.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="config"></param>
		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
		}

		#endregion

		#region GetSettings (static)

		/// <summary>
		///		Gets the <see cref="T:NamedProviderElement"/> for specified provider.
		/// </summary>
		/// <param name="settingsElements">The <see cref="NamedProviderElementCollection"/> object.</param>
		/// <param name="providerName">The provider name.</param>
		/// <param name="nameSuffix">The name suffix to use, or null is not used.</param>
		/// <returns>
		///		A <see cref="T:NamedProviderElement"/> object.
		/// </returns>
		protected static NamedProviderElement GetSettings(NamedProviderElementCollection settingsElements, string providerName, string nameSuffix)
		{
			if (settingsElements == null)
			{
				throw new ArgumentNullException("settingsElements");
			}

			if (providerName == null)
			{
				throw new ArgumentNullException("providerName");
			}

			NamedProviderElement settings = settingsElements[providerName + nameSuffix];

			if (settings == null)
			{
				throw new OscErrorException(string.Format("Provider '{0}{1}' not found.", providerName, nameSuffix));
			}

			Debug.WriteLine(string.Format("Provider: Name: {0}, Type: {1}", settings.Name, settings.Type));

			if (settings.ElementInformation != null && settings.ElementInformation.Properties != null)
			{
				foreach (PropertyInformation pi in settings.ElementInformation.Properties)
				{
					try { Debug.WriteLine(string.Format("\tProperty: {0} = {1}", pi.Name, pi.Value)); }
					catch { }
				}
			}

			return settings;
		}

		#endregion

		#region Public Properties

		/// <summary>Gets the <see cref="T:SettingSettingsCollection"/> object.</summary>
		public SettingSettingsCollection SettingSettings { get { return _settingSettings; } }

		#endregion

		#region Protected Properties

		/// <summary>Gets the application name.</summary>
		protected string ApplicationName { get { return Settings.ApplicationName; } }

		/// <summary>Gets the connection string name.</summary>
		protected string ConnectionStringName { get { return Settings.ConnectionStringName; } }

		/// <summary>Gets the <see cref="T:OscLog"/> object.</summary>
		protected OscLog Log { get { return _log; } }

		/// <summary>Gets the name suffix.</summary>
		protected string NameSuffix { get { return _nameSuffix; } }

		/// <summary>Gets or sets the setting name.</summary>
		protected string SettingName { get { return _settingName; } }

		/// <summary>Gets the <see cref="T:SettingSettings"/> object.</summary>
		protected SettingSettings Settings { get { return _settingSettings[_fullSettingName]; } }

		#endregion
	}
}
