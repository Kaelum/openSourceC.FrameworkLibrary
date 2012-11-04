using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for CryptoStreamWriteDelegate.
	/// </summary>
	/// <param name="cryptoStream">The stream on which to write.</param>
	public delegate void CryptoStreamWriteDelegate(CryptoStream cryptoStream);

	/// <summary>
	///		Summary description for Encryption.
	/// </summary>
	public static class Encryption
	{
		/// <summary>
		///		Class constructor.
		/// </summary>
		static Encryption()
		{
			byte[] activationKey = {
					0x2A, 0x22, 0xB2, 0x21, 0xC2, 0xA4, 0x47, 0xCF, 0x9E, 0x18, 0xD6, 0xDF, 0x6E, 0xC3, 0xFE, 0xCD,
					0x43, 0x7D, 0xDC, 0xD5, 0xB8, 0xEE, 0x46, 0x2B, 0x93, 0x23, 0x7C, 0x62, 0x32, 0x14, 0x4A, 0xC2
				};
			byte[] activationIV = {
					0xB1, 0x63, 0x96, 0x17, 0x59, 0x15, 0x40, 0x39, 0x84, 0xE7, 0x8F, 0x14, 0x1D, 0x4A, 0x66, 0xB5
				};

			const string testString = "This is the end of the world.";

			string encrypted;
			string decrypted;


			try
			{
				// First encryption/decryption is always bad.  This is a workaround for a .NET framework bug.
				encrypted = EncryptToBase64String(testString, activationKey, activationIV);
				decrypted = DecryptFromBase64String(encrypted, activationKey, activationIV);

				// If the bug exists, the exception will be thrown.
				if (!testString.Equals(decrypted))
					throw new ApplicationException(".NET Framework encryption bug detected and handled.");
			}
			catch (ApplicationException ex)
			{
				// The bug has been detected and handled.
				Debug.WriteLine(ex);
			}
			catch (OscException) { }
			catch (Exception ex)
			{
				// An unexpected exception has occured.
				Debug.WriteLine(Format.Exception(ex, "An unexpected exception has occured."));
			}
		}

		/// <summary>
		///		Decrypts the data written to the <see cref="CryptoStream"/> within the delegate
		///		using the specified key and initialization vector.  If the specified <see cref="Stream"/>
		///		is not null, the decrypted data is written to it.  If it is null, a byte array
		///		is returned.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="stream">The stream on which to write the decrypted data.</param>
		/// <param name="key">The decryption key.</param>
		/// <param name="iv">The decryption initialization vector.</param>
		/// <param name="cryptoStreamWriteDelegate">The delegate that writes to the
		///		<see cref="CryptoStream"/>.</param>
		/// <returns>
		///		If the specified <see cref="Stream"/> is null, a byte array with the decrypted
		///		data; otherwise, null.
		///	</returns>
		public static byte[] Decrypt<TSymmetricAlgorithm>(Stream stream, byte[] key, byte[] iv, CryptoStreamWriteDelegate cryptoStreamWriteDelegate)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				using (SymmetricAlgorithm algorithm = new TSymmetricAlgorithm())
				using (ICryptoTransform cryptoTransform = algorithm.CreateDecryptor(key, iv))
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
				{
					cryptoStreamWriteDelegate(cryptoStream);

					cryptoStream.FlushFinalBlock();
					algorithm.Clear();

					if (stream != null)
					{
						memoryStream.WriteTo(stream);
						return null;
					}
					else
					{
						return memoryStream.ToArray();
					}
				}
			}
			catch (OscException) { throw; }
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw new OscErrorException("An unexpected exception has occured.", ex);
			}
		}

		/// <summary>
		///		Decrypts the specified base 64 encoded string with the specified key and
		///		initialization vector and returns the decrypted data as a string.
		/// </summary>
		/// <param name="buffer">The string to decrypt.</param>
		/// <param name="key">The decryption key.</param>
		/// <param name="iv">The decryption initialization vector.</param>
		/// <returns>
		///		Decrypted string.
		///	</returns>
		public static string DecryptFromBase64String(string buffer, byte[] key, byte[] iv)
		{
			// RijndaelManaged
			// DESCryptoServiceProvider
			// RC2CryptoServiceProvider
			// TripleDESCryptoServiceProvider
			return DecryptFromBase64String<RijndaelManaged>(buffer, key, iv);
		}

		/// <summary>
		///		Decrypts the specified base 64 encoded string with the specified key and
		///		initialization vector and returns the decrypted data as a string.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="buffer">The string to decrypt.</param>
		/// <param name="key">The decryption key.</param>
		/// <param name="iv">The decryption initialization vector.</param>
		/// <returns>
		///		Decrypted string.
		///	</returns>
		public static string DecryptFromBase64String<TSymmetricAlgorithm>(string buffer, byte[] key, byte[] iv)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			return (new UTF8Encoding()).GetString(DecryptFromBase64String<TSymmetricAlgorithm>(null, buffer, key, iv));
		}

		/// <summary>
		///		Decrypts the specified base 64 encoded string using the specified key and
		///		initialization vector.  If the specified <see cref="Stream"/> is not null, the
		///		decrypted data is written to it.  If it is null, a byte array is returned.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="stream">The stream on which to write the decrypted data.</param>
		/// <param name="buffer">The string to decrypt.</param>
		/// <param name="key">The decryption key.</param>
		/// <param name="iv">The decryption initialization vector.</param>
		/// <returns>
		///		If the specified <see cref="Stream"/> is null, a byte array with the decrypted
		///		data; otherwise, null.
		///	</returns>
		public static byte[] DecryptFromBase64String<TSymmetricAlgorithm>(Stream stream, string buffer, byte[] key, byte[] iv)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				int mod = buffer.Length % 4;
				byte[] byteArray = Convert.FromBase64String(buffer.PadRight(buffer.Length + (mod == 0 ? 0 : 4 - mod), '='));

				return Decrypt<TSymmetricAlgorithm>(stream, key, iv,
					delegate(CryptoStream cryptoStream)
					{
						cryptoStream.Write(byteArray, 0, byteArray.Length);
					}
				);
			}
			catch (OscException) { throw; }
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw new OscErrorException("An unexpected exception has occured.", ex);
			}
		}

		/// <summary>
		///		Encrypts the specified string with the specified key and initialization vector.
		///		If the specified <see cref="Stream"/> is not null, the encrypted data is written to
		///		it.  If it is null, a byte array is returned.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="stream">The stream on which to write the encrypted data.</param>
		/// <param name="buffer">The string to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryption initialization vector.</param>
		/// <returns>
		///		If the specified <see cref="Stream"/> is null, a byte array with the encrypted
		///		data; otherwise, null.
		///	</returns>
		public static byte[] Encrypt<TSymmetricAlgorithm>(Stream stream, string buffer, byte[] key, byte[] iv)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				return Encrypt<TSymmetricAlgorithm>(stream, key, iv,
					delegate(CryptoStream cryptoStream)
					{
						WriteStringToCryptoStream(cryptoStream, buffer);
					}
				);
			}
			catch (OscException) { throw; }
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw new OscErrorException("An unexpected exception has occured.", ex);
			}
		}

		/// <summary>
		///		Encrypts the data written to the <see cref="CryptoStream"/> within the delegate
		///		using the specified key and initialization vector.  If the specified <see cref="Stream"/>
		///		is not null, the encrypted data is written to it.  If it is null, a byte array
		///		is returned.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="stream">The stream on which to write the encrypted data.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryption initialization vector.</param>
		/// <param name="cryptoStreamWriteDelegate">The delegate that writes to the
		///		<see cref="CryptoStream"/>.</param>
		/// <returns>
		///		If the specified <see cref="Stream"/> is null, a byte array with the encrypted
		///		data; otherwise, null.
		///	</returns>
		public static byte[] Encrypt<TSymmetricAlgorithm>(Stream stream, byte[] key, byte[] iv, CryptoStreamWriteDelegate cryptoStreamWriteDelegate)
				where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				using (SymmetricAlgorithm algorithm = new TSymmetricAlgorithm())
				using (ICryptoTransform cryptoTransform = algorithm.CreateEncryptor(key, iv))
				using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
				{
					cryptoStreamWriteDelegate(cryptoStream);

					cryptoStream.FlushFinalBlock();
					algorithm.Clear();

					if (stream != null)
					{
						memoryStream.WriteTo(stream);
						return null;
					}
					else
					{
						return memoryStream.ToArray();
					}
				}
			}
			catch (OscException) { throw; }
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw new OscErrorException("An unexpected exception has occured.", ex);
			}
		}

		/// <summary>
		///		Encrypts the specified string with the specified key and initialization vector and
		///		returns a base 64 encoded string.
		/// </summary>
		/// <param name="buffer">The string to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryption initialization vector.</param>
		/// <returns>Encrypted string.</returns>
		public static string EncryptToBase64String(string buffer, byte[] key, byte[] iv)
		{
			// RijndaelManaged
			// DESCryptoServiceProvider
			// RC2CryptoServiceProvider
			// TripleDESCryptoServiceProvider
			return EncryptToBase64String<RijndaelManaged>(buffer, key, iv);
		}

		/// <summary>
		///		Encrypts the specified string with the specified key and initialization vector and
		///		returns a base 64 encoded string.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="buffer">The string to encrypt.</param>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryption initialization vector.</param>
		/// <returns>Encrypted string.</returns>
		public static string EncryptToBase64String<TSymmetricAlgorithm>(string buffer, byte[] key, byte[] iv)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				return EncryptToBase64String<TSymmetricAlgorithm>(key, iv,
					delegate(CryptoStream cryptoStream)
					{
						WriteStringToCryptoStream(cryptoStream, buffer);
					}
				);
			}
			catch (OscException) { throw; }
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw new OscErrorException("An unexpected exception has occured.", ex);
			}
		}

		/// <summary>
		///		Encrypts the data written to the <see cref="CryptoStream"/> within the delegate
		///		using the specified key and initialization vector and returns a base 64 encoded
		///		string.
		/// </summary>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="key">The encryption key.</param>
		/// <param name="iv">The encryption initialization vector.</param>
		/// <param name="cryptoStreamWriteDelegate">The delegate that writes to the
		///		<see cref="CryptoStream"/>.</param>
		/// <returns>
		///		Encrypted string.
		///	</returns>
		public static string EncryptToBase64String<TSymmetricAlgorithm>(byte[] key, byte[] iv, CryptoStreamWriteDelegate cryptoStreamWriteDelegate)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				byte[] buffer = Encrypt<TSymmetricAlgorithm>(null, key, iv, cryptoStreamWriteDelegate);
				return Convert.ToBase64String(buffer).TrimEnd("=".ToCharArray());
			}
			catch (OscException) { throw; }
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				throw new OscErrorException("An unexpected exception has occured.", ex);
			}
		}

		#region Private Methods

		private static void WriteStringToCryptoStream(CryptoStream cryptoStream, string buffer)
		{
			byte[] byteArray = (new UTF8Encoding()).GetBytes(buffer);
			cryptoStream.Write(byteArray, 0, byteArray.Length);
		}

		#endregion
	}
}
