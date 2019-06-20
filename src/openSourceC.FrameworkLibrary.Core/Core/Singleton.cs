using System;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///     Summary description for Singleton.
	/// </summary>
	/// <typeparam name="T">The class being wrapped.</typeparam>
	public static class Singleton<T>
		where T : class, new()
	{
		///// <summary>Singleton instance.</summary>
		//public static readonly T Instance = new T();

		private static object _singletonLock = new object();
		private static T _instance;


		/// <summary>
		///     Singleton instance.
		/// </summary>
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_singletonLock)
					{
						if (_instance == null)
						{
							_instance = new T();
						}
					}
				}

				return _instance;
			}
		}
	}
}
