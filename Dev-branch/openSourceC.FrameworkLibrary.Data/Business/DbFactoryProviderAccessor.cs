using System;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		Summary description for DbFactoryProviderAccessor&lt;TDataFactoryProxyInterface&gt;.
	/// </summary>
	/// <typeparam name="TDataFactoryProxyInterface"></typeparam>
	public abstract class DbFactoryProviderAccessor<TDataFactoryProxyInterface> : IDisposable
		where TDataFactoryProxyInterface : class
	{
		private DbFactorySection _configurationSection;
		private readonly string _providerName;

		private DbFactoryProvider _factoryProvider;
		private object _factoryProviderLock = new object();

		private bool _disposed = false;


		#region Contructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProviderAccessor&lt;TDataFactoryProxyInterface&gt;"/> class.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySection"/> object.</param>
		/// <param name="providerName">The name of the provider to create a data factory instance of.</param>
		public DbFactoryProviderAccessor(DbFactorySection configurationSection, string providerName)
		{
			_configurationSection = configurationSection;
			_providerName = providerName;
		}

		#endregion

		#region Destructor

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~DbFactoryProviderAccessor()
		{
			Dispose(false);
		}

		#endregion

		#region IDisposable Implmentation

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

		#region Protected Properties

		/// <summary>
		///		Gets the <typeparamref name="TDataFactoryProxyInterface"/> data factory proxy interface of the current
		///		<see cref="DbFactoryProvider&lt;TRequestContext&gt;"/> instance.
		/// </summary>
		protected TDataFactoryProxyInterface DataFactory
		{
			get
			{
				if (_factoryProvider == null)
				{
					lock (_factoryProviderLock)
					{
						if (_factoryProvider == null)
						{
							_factoryProvider = DbFactoryProvider.CreateInstance(_configurationSection, _providerName);
						}
					}
				}

				return _factoryProvider as TDataFactoryProxyInterface;
			}
		}

		#endregion

		#region Abstract Methods

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

	/// <summary>
	///		Summary description for DbFactoryProviderAccessor&lt;TRequestContext, TDataFactoryProxyInterface&gt;.
	/// </summary>
	/// <typeparam name="TRequestContext"></typeparam>
	/// <typeparam name="TDataFactoryProxyInterface"></typeparam>
	public abstract class DbFactoryProviderAccessor<TRequestContext, TDataFactoryProxyInterface> : IDisposable
		where TRequestContext : struct
		where TDataFactoryProxyInterface : class
	{
		private DbFactorySection _configurationSection;
		private readonly string _providerName;
		private TRequestContext _requestContext;

		private DbFactoryProvider<TRequestContext> _factoryProvider;
		private object _factoryProviderLock = new object();

		private bool _disposed = false;


		#region Contructors

		/// <summary>
		///		Initializes a new instance of the <see cref="DbFactoryProviderAccessor&lt;TRequestContext, TDataFactoryProxyInterface&gt;"/> class.
		/// </summary>
		/// <param name="configurationSection">The <see cref="DbFactorySection"/> object.</param>
		/// <param name="providerName">The name of the provider to create a data factory instance of.</param>
		/// <param name="requestContext">The current <typeparamref name="TRequestContext"/> object.</param>
		public DbFactoryProviderAccessor(DbFactorySection configurationSection, string providerName, TRequestContext requestContext)
		{
			_configurationSection = configurationSection;
			_providerName = providerName;
			_requestContext = requestContext;
		}

		#endregion

		#region Destructor

		/// <summary>
		///		This destructor will run only if the Dispose method does not get called.
		/// </summary>
		/// <remarks>Do not provide destructors in types derived from this class.</remarks>
		~DbFactoryProviderAccessor()
		{
			Dispose(false);
		}

		#endregion

		#region IDisposable Implmentation

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

		#region Protected Properties

		/// <summary>
		///		Gets the <typeparamref name="TDataFactoryProxyInterface"/> data factory interface of the current
		///		<see cref="DbFactoryProvider&lt;TRequestContext&gt;"/> instance.
		/// </summary>
		protected TDataFactoryProxyInterface DataFactory
		{
			get
			{
				if (_factoryProvider == null)
				{
					lock (_factoryProviderLock)
					{
						if (_factoryProvider == null)
						{
							_factoryProvider = DbFactoryProvider<TRequestContext>.CreateInstance(_configurationSection, _providerName, _requestContext);
						}
					}
				}

				return _factoryProvider as TDataFactoryProxyInterface;
			}
		}

		#endregion

		#region Abstract Methods

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
