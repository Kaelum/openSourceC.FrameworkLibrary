﻿using System;

namespace openSourceC.FrameworkLibrary.Common
{
	/// <summary>
	///		Summary description for StringExtensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		///		Returns the left part of a character string with the specified number of characters.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="maxLength">Is a positive integer that specifies how many characters of the
		///		source will be returned. If maxLength is negative, an error is returned.</param>
		/// <returns>
		///		Returns a string of the maximum specified length.
		/// </returns>
		public static string Left(this string source, int maxLength)
		{
			if (source.Length <= maxLength)
			{
				return source;
			}

			return source.Substring(0, maxLength);
		}

		/// <summary>
		///		Returns the right part of a character string with the specified number of characters.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="maxLength">Is a positive integer that specifies how many characters of the
		///		source will be returned. If maxLength is negative, an error is returned.</param>
		/// <returns>
		///		Returns a string of the maximum specified length.
		/// </returns>
		public static string Right(this string source, int maxLength)
		{
			if (source.Length <= maxLength)
			{
				return source;
			}

			return source.Substring(maxLength - source.Length, maxLength);
		}
	}
}
