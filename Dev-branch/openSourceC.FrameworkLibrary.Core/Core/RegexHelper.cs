using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for RegexHelper.
	/// </summary>
	public static class RegexHelper
	{
		/// <summary>
		///		Use this to parse fully qualified type names.
		/// </summary>
		public static Regex ParseType = new Regex(@"^(?<type>.*?(?:\[.*?\])*?),\s*(?<assembly>.*?)\s*$", RegexOptions.None);
	}
}
