using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

using openSourceC.FrameworkLibrary.Web.Util;

namespace openSourceC.FrameworkLibrary.Web
{
	/// <summary>
	///		Summary description for MachineKeyEncryption.
	/// </summary>
	public class MachineKeyEncryption
	{
		/// <summary></summary>
		public const string SectionName = "system.web/machineKey";

		private static MachineKeySection _sMachineKeySection;
		private static object _sMachineKeySectionLock = new object();

		private static byte[] _sValidationKey;

		private static byte[] _sInner = null;
		private static byte[] _sOuter = null;

		private static Stack _sDecryptorStackDecryption;
		private static Stack _sDecryptorStackValidation;
		private static Stack _sEncryptorStackDecryption;
		private static Stack _sEncryptorStackValidation;

		private static SymmetricAlgorithm _sSymAlgoDecryption;
		private static SymmetricAlgorithm _sSymAlgoValidation;

		private static RNGCryptoServiceProvider _sRandomNumberGeneratorProvider;

		private bool _dataInitialized;

		private byte[] _decryptionKey;
		private byte[] _validationKey;


		#region Public Static Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string DecryptString(string s)
		{
			return DecryptStringWithIV(s, IVType.Hash);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		/// <param name="ivType"></param>
		/// <returns></returns>
		public static string DecryptStringWithIV(string s, IVType ivType)
		{
			if (s == null)
			{
				return null;
			}

			byte[] buf = HttpServerUtility.UrlTokenDecode(s);

			if (buf != null)
			{
				buf = EncryptOrDecryptData(false, buf, null, 0, buf.Length, ivType, false);
			}

			if (buf == null)
			{
				throw new HttpException(SR.GetString("ViewState_InvalidViewState"));
			}

			return Encoding.UTF8.GetString(buf);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="buf"></param>
		public static void DestroyByteArray(byte[] buf)
		{
			if ((buf != null) && (buf.Length >= 1))
			{
				for (int i = 0; i < buf.Length; i++)
				{
					buf[i] = 0;
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="fEncrypt"></param>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length)
		{
			return EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, IVType.Hash, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fEncrypt"></param>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="useValidationSymAlgo"></param>
		/// <returns></returns>
		public static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, bool useValidationSymAlgo)
		{
			return EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, IVType.Hash, useValidationSymAlgo);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="fEncrypt"></param>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="ivType"></param>
		/// <returns></returns>
		public static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, IVType ivType)
		{
			return EncryptOrDecryptData(fEncrypt, buf, modifier, start, length, ivType, false);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="fEncrypt"></param>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="ivType"></param>
		/// <param name="useValidationSymAlgo"></param>
		/// <returns></returns>
		public static byte[] EncryptOrDecryptData(bool fEncrypt, byte[] buf, byte[] modifier, int start, int length, IVType ivType, bool useValidationSymAlgo)
		{
			byte[] buffer3;
			EnsureConfig();
			MemoryStream stream = new MemoryStream();
			ICryptoTransform cryptoTransform = GetCryptoTransform(fEncrypt, useValidationSymAlgo);
			CryptoStream stream2 = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);
			bool flag = (ivType != IVType.None);

			if (fEncrypt && flag)
			{
				byte[] iVRandom = null;
				switch (ivType)
				{
					case IVType.Random:
					iVRandom = GetIVRandom(useValidationSymAlgo);
					break;

					case IVType.Hash:
					iVRandom = GetIVHash(buf, useValidationSymAlgo);
					break;
				}
				stream2.Write(iVRandom, 0, iVRandom.Length);
			}

			stream2.Write(buf, start, length);

			if (fEncrypt && (modifier != null))
			{
				stream2.Write(modifier, 0, modifier.Length);
			}

			stream2.FlushFinalBlock();
			byte[] src = stream.ToArray();
			stream2.Close();
			ReturnCryptoTransform(fEncrypt, cryptoTransform, useValidationSymAlgo);

			if (!fEncrypt && flag)
			{
				int iVLength = GetIVLength(useValidationSymAlgo);
				int count = src.Length - iVLength;

				if (count >= 0)
				{
					buffer3 = new byte[count];

					if (count > 0)
					{
						Buffer.BlockCopy(src, iVLength, buffer3, 0, count);
					}
				}
				else
				{
					buffer3 = src;
				}
			}
			else
			{
				buffer3 = src;
			}

			if ((fEncrypt || (modifier == null)) || (modifier.Length <= 0))
			{
				return buffer3;
			}

			for (int i = 0; i < modifier.Length; i++)
			{
				if (buffer3[(buffer3.Length - modifier.Length) + i] != modifier[i])
				{
					throw new HttpException(SR.GetString("Unable_to_validate_data"));
				}
			}

			byte[] dst = new byte[buffer3.Length - modifier.Length];
			Buffer.BlockCopy(buffer3, 0, dst, 0, dst.Length);

			return dst;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string EncryptString(string s)
		{
			return EncryptStringWithIV(s, IVType.Hash);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		/// <param name="ivType"></param>
		/// <returns></returns>
		public static string EncryptStringWithIV(string s, IVType ivType)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);

			return HttpServerUtility.UrlTokenEncode(EncryptOrDecryptData(true, bytes, null, 0, bytes.Length, ivType, false));
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="dataLength"></param>
		/// <returns></returns>
		public static byte[] GetDecodedData(byte[] buf, byte[] modifier, int start, int length, ref int dataLength)
		{
			EnsureConfig();
			if ((_sMachineKeySection.Validation == MachineKeyValidation.TripleDES) || (_sMachineKeySection.Validation == MachineKeyValidation.AES))
			{
				buf = EncryptOrDecryptData(false, buf, modifier, start, length, true);

				if ((buf == null) || (buf.Length < 20))
				{
					throw new HttpException(SR.Unable_to_validate_data);
				}

				length = buf.Length;
				start = 0;
			}

			if (((length < 20) || (start < 0)) || (start >= length))
			{
				throw new HttpException(SR.Unable_to_validate_data);
			}

			byte[] buffer = HashData(buf, modifier, start, length - 20);

			for (int i = 0; i < buffer.Length; i++)
			{
				if (buffer[i] != buf[((start + length) - 20) + i])
				{
					throw new HttpException(SR.Unable_to_validate_data);
				}
			}

			dataLength = length - 20;
			return buf;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte[] GetEncodedData(byte[] buf, byte[] modifier, int start, ref int length)
		{
			byte[] buffer2;
			EnsureConfig();
			byte[] src = HashData(buf, modifier, start, length);

			if (((buf.Length - start) - length) >= src.Length)
			{
				Buffer.BlockCopy(src, 0, buf, start + length, src.Length);
				buffer2 = buf;
			}
			else
			{
				buffer2 = new byte[length + src.Length];
				Buffer.BlockCopy(buf, start, buffer2, 0, length);
				Buffer.BlockCopy(src, 0, buffer2, length, src.Length);
				start = 0;
			}

			length += src.Length;

			if ((_sMachineKeySection.Validation == MachineKeyValidation.TripleDES) || (_sMachineKeySection.Validation == MachineKeyValidation.AES))
			{
				buffer2 = EncryptOrDecryptData(true, buffer2, modifier, start, length, true);
				length = buffer2.Length;
			}

			return buffer2;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string HashAndBase64EncodeString(string s)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(s);

			return Convert.ToBase64String(HashData(bytes, null, 0, bytes.Length));
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="buf"></param>
		/// <param name="modifier"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte[] HashData(byte[] buf, byte[] modifier, int start, int length)
		{
			EnsureConfig();
			byte[] src = null;

			if (_sMachineKeySection.Validation == MachineKeyValidation.MD5)
			{
				src = MD5HashForData(buf, modifier, start, length);
			}
			else
			{
				src = GetHMACSHA1Hash(buf, modifier, start, length);
			}

			if (src.Length < 20)
			{
				byte[] dst = new byte[20];
				Buffer.BlockCopy(src, 0, dst, 0, src.Length);
				src = dst;
			}

			return src;
		}

		#endregion

		#region Private Static Methods

		private static void EnsureConfig()
		{
			if (_sMachineKeySection == null)
			{
				lock (_sMachineKeySectionLock)
				{
					if (_sMachineKeySection == null)
					{
						MachineKeySection machineKey = (MachineKeySection)WebConfigurationManager.GetSection(SectionName);
						MachineKeyEncryption encryption = new MachineKeyEncryption();
						encryption.ConfigureEncryptionObject();

						_sMachineKeySection = machineKey;
					}
				}
			}
		}

		private static ICryptoTransform GetCryptoTransform(bool fEncrypt, bool useValidationSymAlgo)
		{
			Stack stack;


			if (useValidationSymAlgo)
			{
				stack = fEncrypt ? _sEncryptorStackValidation : _sDecryptorStackValidation;
			}
			else
			{
				stack = fEncrypt ? _sEncryptorStackDecryption : _sDecryptorStackDecryption;
			}

			lock (stack)
			{
				if (stack.Count > 0)
				{
					return (ICryptoTransform)stack.Pop();
				}
			}

			if (useValidationSymAlgo)
			{
				lock (_sSymAlgoValidation)
				{
					return (fEncrypt ? _sSymAlgoValidation.CreateEncryptor() : _sSymAlgoValidation.CreateDecryptor());
				}
			}

			lock (_sSymAlgoDecryption)
			{
				return (fEncrypt ? _sSymAlgoDecryption.CreateEncryptor() : _sSymAlgoDecryption.CreateDecryptor());
			}
		}

		private static byte[] GetHMACSHA1Hash(byte[] buf, byte[] modifier, int start, int length)
		{
			if (((length < 0) || (buf == null)) || (length > buf.Length))
			{
				throw new ArgumentException(SR.GetString(SR.InvalidArgumentValue, new object[] { "length" }));
			}

			if ((start < 0) || (start >= length))
			{
				throw new ArgumentException(SR.GetString(SR.InvalidArgumentValue, new object[] { "start" }));
			}

			byte[] hash = new byte[20];
			Marshal.ThrowExceptionForHR(UnsafeNativeMethods.GetHMACSHA1Hash(buf, start, length, modifier, (modifier == null) ? 0 : modifier.Length, _sInner, _sInner.Length, _sOuter, _sOuter.Length, hash, hash.Length));

			return hash;
		}

		private static byte[] GetIVHash(byte[] buf, bool useValidationSymAlgo)
		{
			int iVLength = GetIVLength(useValidationSymAlgo);
			int num2 = iVLength;
			int dstOffset = 0;
			byte[] dst = new byte[iVLength];
			byte[] data = buf;

			while (dstOffset < iVLength)
			{
				byte[] hash = new byte[20];
				Marshal.ThrowExceptionForHR(UnsafeNativeMethods.GetSHA1Hash(data, data.Length, hash, hash.Length));
				data = hash;
				int count = Math.Min(20, num2);
				Buffer.BlockCopy(data, 0, dst, dstOffset, count);
				dstOffset += count;
				num2 -= count;
			}

			return dst;
		}

		private static int GetIVLength(bool useValidationSymAlgo)
		{
			SymmetricAlgorithm algorithm = useValidationSymAlgo ? _sSymAlgoValidation : _sSymAlgoDecryption;
			int keySize = algorithm.KeySize;

			if ((keySize % 8) != 0)
			{
				keySize += 8 - (keySize % 8);
			}

			return (keySize / 8);
		}

		private static byte[] GetIVRandom(bool useValidationSymAlgo)
		{
			byte[] data = new byte[GetIVLength(useValidationSymAlgo)];

			if (_sRandomNumberGeneratorProvider == null)
			{
				_sRandomNumberGeneratorProvider = new RNGCryptoServiceProvider();
			}

			_sRandomNumberGeneratorProvider.GetBytes(data);
			return data;
		}

		private static byte[] MD5HashForData(byte[] buf, byte[] modifier, int start, int length)
		{
			MD5 md = MD5.Create();
			int num = length + _sValidationKey.Length;

			if (modifier != null)
			{
				num += modifier.Length;
			}

			byte[] dst = new byte[num];
			Buffer.BlockCopy(buf, start, dst, 0, length);

			if (modifier != null)
			{
				Buffer.BlockCopy(modifier, 0, dst, length, modifier.Length);
				length += modifier.Length;
			}

			Buffer.BlockCopy(_sValidationKey, 0, dst, length, _sValidationKey.Length);

			return md.ComputeHash(dst);
		}

		private static void ReturnCryptoTransform(bool fEncrypt, ICryptoTransform ct, bool useValidationSymAlgo)
		{
			Stack stack;

			if (useValidationSymAlgo)
			{
				stack = fEncrypt ? _sEncryptorStackValidation : _sDecryptorStackValidation;
			}
			else
			{
				stack = fEncrypt ? _sEncryptorStackDecryption : _sDecryptorStackDecryption;
			}

			lock (stack)
			{
				if (stack.Count <= 100)
				{
					stack.Push(ct);
				}
			}
		}

		private static void SetInnerOuterKeys(byte[] validationKey)
		{
			const int keyLength = 64;

			byte[] hash = null;
			int num2;


			if (validationKey.Length > keyLength)
			{
				hash = new byte[20];
				Marshal.ThrowExceptionForHR(UnsafeNativeMethods.GetSHA1Hash(validationKey, validationKey.Length, hash, hash.Length));
			}

			if (_sInner == null)
			{
				_sInner = new byte[keyLength];
			}

			if (_sOuter == null)
			{
				_sOuter = new byte[keyLength];
			}

			for (num2 = 0; num2 < keyLength; num2++)
			{
				_sInner[num2] = 0x36;
				_sOuter[num2] = 0x5c;
			}

			for (num2 = 0; num2 < validationKey.Length; num2++)
			{
				_sInner[num2] = (byte)(_sInner[num2] ^ validationKey[num2]);
				_sOuter[num2] = (byte)(_sOuter[num2] ^ validationKey[num2]);
			}
		}

		#endregion

		#region Private Methods

		private void ConfigureEncryptionObject()
		{
			RuntimeDataInitialize();

			_sValidationKey = (byte[])_validationKey.Clone();
			byte[] decryptionKey = (byte[])_decryptionKey.Clone();

			SetInnerOuterKeys(_sValidationKey);
			DestroyKeys();

			string decryption = _sMachineKeySection.Decryption;

			switch (decryption)
			{
				case "3DES":
				_sSymAlgoDecryption = new TripleDESCryptoServiceProvider();
				break;

				case "DES":
				_sSymAlgoDecryption = new DESCryptoServiceProvider();
				break;

				case "AES":
				_sSymAlgoDecryption = new RijndaelManaged();
				break;

				default:
				if (decryptionKey.Length == 8)
				{
					_sSymAlgoDecryption = new DESCryptoServiceProvider();
				}
				else
				{
					_sSymAlgoDecryption = new RijndaelManaged();
				}
				break;
			}

			switch (_sMachineKeySection.Validation)
			{
				case MachineKeyValidation.TripleDES:
				if (decryptionKey.Length != 8)
				{
					_sSymAlgoValidation = new TripleDESCryptoServiceProvider();
				}
				else
				{
					_sSymAlgoValidation = new DESCryptoServiceProvider();
				}
				break;

				case MachineKeyValidation.AES:
				_sSymAlgoValidation = new RijndaelManaged();
				break;

				default:
				break;
			}

			if (_sSymAlgoValidation != null)
			{
				SetKeyOnSymAlgorithm(_sSymAlgoValidation, decryptionKey);
				_sEncryptorStackValidation = new Stack();
				_sDecryptorStackValidation = new Stack();
			}

			SetKeyOnSymAlgorithm(_sSymAlgoDecryption, decryptionKey);
			_sEncryptorStackDecryption = new Stack();
			_sDecryptorStackDecryption = new Stack();

			DestroyByteArray(decryptionKey);
		}

		private void DestroyKeys()
		{
			DestroyByteArray(_decryptionKey);
			DestroyByteArray(_validationKey);
		}

		private void RuntimeDataInitialize()
		{
			if (!_dataInitialized)
			{
				RNGCryptoServiceProvider autogenKeyProvider = null;
				byte[] autogenKey = null;

				string appDomainAppVirtualPath = HttpRuntime.AppDomainAppVirtualPath;

				if (appDomainAppVirtualPath == null)
				{
					appDomainAppVirtualPath = Process.GetCurrentProcess().MainModule.ModuleName;
				}

				if (_sMachineKeySection.ValidationKey.Contains("AutoGenerate") || _sMachineKeySection.DecryptionKey.Contains("AutoGenerate"))
				{
					autogenKeyProvider = new RNGCryptoServiceProvider();
					autogenKey = new byte[88];
				}

				string key = _sMachineKeySection.ValidationKey;
				bool isolateAppsFlag = StringUtil.StringEndsWith(key, ",IsolateApps");

				if (isolateAppsFlag)
				{
					key = key.Substring(0, key.Length - ",IsolateApps".Length);
				}

				if (key == "AutoGenerate")
				{
					_validationKey = new byte[64];
					autogenKeyProvider.GetBytes(_validationKey);
				}
				else
				{
					if ((key.Length > 0x80) || (key.Length < 40))
					{
						throw new ConfigurationErrorsException(SR.GetString("Unable_to_get_cookie_authentication_validation_key", new object[] { key.Length.ToString(CultureInfo.InvariantCulture) }), _sMachineKeySection.ElementInformation.Properties["validationKey"].Source, _sMachineKeySection.ElementInformation.Properties["validationKey"].LineNumber);
					}

					_validationKey = HexConvert.StringToByteArray(key);

					if (_validationKey == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_validation_key"), _sMachineKeySection.ElementInformation.Properties["validationKey"].Source, _sMachineKeySection.ElementInformation.Properties["validationKey"].LineNumber);
					}
				}

				if (isolateAppsFlag)
				{
					int hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(appDomainAppVirtualPath);

					_validationKey[0] = (byte)(hashCode & 0xff);
					_validationKey[1] = (byte)((hashCode & 0xff00) >> 8);
					_validationKey[2] = (byte)((hashCode & 0xff0000) >> 16);
					_validationKey[3] = (byte)((hashCode & 0xff000000L) >> 24);
				}

				key = _sMachineKeySection.DecryptionKey;
				isolateAppsFlag = StringUtil.StringEndsWith(key, ",IsolateApps");

				if (isolateAppsFlag)
				{
					key = key.Substring(0, key.Length - ",IsolateApps".Length);
				}

				if (key == "AutoGenerate")
				{
					_decryptionKey = new byte[24];
					autogenKeyProvider.GetBytes(_decryptionKey);
				}
				else
				{
					if ((key.Length % 2) != 0)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_decryption_key"), _sMachineKeySection.ElementInformation.Properties["decryptionKey"].Source, _sMachineKeySection.ElementInformation.Properties["decryptionKey"].LineNumber);
					}

					_decryptionKey = HexConvert.StringToByteArray(key);

					if (_decryptionKey == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Invalid_decryption_key"), _sMachineKeySection.ElementInformation.Properties["decryptionKey"].Source, _sMachineKeySection.ElementInformation.Properties["decryptionKey"].LineNumber);
					}
				}

				if (isolateAppsFlag)
				{
					int hashCode = StringComparer.InvariantCultureIgnoreCase.GetHashCode(appDomainAppVirtualPath);

					_decryptionKey[0] = (byte)(hashCode & 0xff);
					_decryptionKey[1] = (byte)((hashCode & 0xff00) >> 8);
					_decryptionKey[2] = (byte)((hashCode & 0xff0000) >> 16);
					_decryptionKey[3] = (byte)((hashCode & 0xff000000L) >> 24);
				}

				_dataInitialized = true;
			}
		}

		private void SetKeyOnSymAlgorithm(SymmetricAlgorithm symAlgo, byte[] dKey)
		{
			try
			{
				if ((dKey.Length == 0x18) && (symAlgo is DESCryptoServiceProvider))
				{
					byte[] dst = new byte[8];
					Buffer.BlockCopy(dKey, 0, dst, 0, 8);
					symAlgo.Key = dst;
					DestroyByteArray(dst);
				}
				else
				{
					symAlgo.Key = dKey;
				}
				symAlgo.GenerateIV();
				symAlgo.IV = new byte[symAlgo.IV.Length];
			}
			catch (Exception exception)
			{
				throw new ConfigurationErrorsException(SR.GetString("Bad_machine_key", new object[] { exception.Message }), _sMachineKeySection.ElementInformation.Properties["decryptionKey"].Source, _sMachineKeySection.ElementInformation.Properties["decryptionKey"].LineNumber);
			}
		}

		#endregion
	}
}
