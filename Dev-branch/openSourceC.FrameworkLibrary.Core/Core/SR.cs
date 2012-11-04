using System;
using System.Globalization;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for SR.
	/// </summary>
	public partial class SR
	{
		/// <summary>
		///		
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static object GetObject(string name)
		{
			return ResourceManager.GetObject(name, Culture);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetString(string name)
		{
			return ResourceManager.GetString(name, Culture);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="name"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string GetString(string name, params object[] args)
		{
			string format = ResourceManager.GetString(name, Culture);

			if ((args == null) || (args.Length <= 0))
			{
				return format;
			}

			for (int i = 0; i < args.Length; i++)
			{
				string str2 = args[i] as string;

				if ((str2 != null) && (str2.Length > 0x400))
				{
					args[i] = str2.Substring(0, 0x3fd) + "...";
				}
			}

			return string.Format(CultureInfo.CurrentCulture, format, args);
		}
	}
}
