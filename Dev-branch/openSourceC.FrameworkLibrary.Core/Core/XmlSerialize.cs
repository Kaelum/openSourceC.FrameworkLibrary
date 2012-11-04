using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for XmlSerialize.
	/// </summary>
	public static class XmlSerialize
	{
		/// <summary>
		///		Deserializes the XML document contained by the specified <see cref="Stream"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="stream">The <see cref="Stream"/> that contains the XML document to
		///		deserialize.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject Deserialize<TObject>(Stream stream)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(TObject));
				return (TObject)serializer.Deserialize(stream);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Deserializes the XML document contained by the specified string.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="xmlString">The string to be deserialized.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject Deserialize<TObject>(string xmlString)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(xmlString)) { return default(TObject); }

				//XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				//xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;

				//using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
				using (StringReader stringReader = new StringReader(xmlString))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(TObject));
					return (TObject)serializer.Deserialize(stringReader);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Deserializes the XML document contained by the specified <see cref="TextReader"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="textReader">The <see cref="TextReader"/> that contains the XML document
		///		to deserialize.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject Deserialize<TObject>(TextReader textReader)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(TObject));
				return (TObject)serializer.Deserialize(textReader);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Decrypts and deserializes the specified object using the specified key and
		///		initialization vector and returns a base 64 encoded string of the encrypted data.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="base64XmlString">The string to be decrypted and deserialized.</param>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Encryption initialization vector.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject DecryptFromBase64String<TObject>(string base64XmlString, byte[] key, byte[] iv)
		{
			// RijndaelManaged
			// DESCryptoServiceProvider
			// RC2CryptoServiceProvider
			// TripleDESCryptoServiceProvider
			return DecryptFromBase64String<TObject, RijndaelManaged>(base64XmlString, key, iv);
		}

		/// <summary>
		///		Decrypts and deserializes the specified object using the specified key and
		///		initialization vector and returns a base 64 encoded string of the encrypted data.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="base64XmlString">The string to be decrypted and deserialized.</param>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Encryption initialization vector.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject DecryptFromBase64String<TObject, TSymmetricAlgorithm>(string base64XmlString, byte[] key, byte[] iv)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				using (MemoryStream stream = new MemoryStream())
				{
					Encryption.DecryptFromBase64String<TSymmetricAlgorithm>(stream, base64XmlString, key, iv);

					Debug.Print("Decrypted XML: {0}\n", (new UTF8Encoding()).GetString(stream.ToArray()));

					stream.Seek(0, SeekOrigin.Begin);

					XmlSerializer serializer = new XmlSerializer(typeof(TObject));
					return (TObject)serializer.Deserialize(stream);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Encrypts and serializes the specified object using the specified key and
		///		initialization vector and returns a base 64 encoded string of the encrypted data.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Encryption initialization vector.</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string EncryptToBase64String<TObject>(TObject obj, byte[] key, byte[] iv)
		{
			// RijndaelManaged
			// DESCryptoServiceProvider
			// RC2CryptoServiceProvider
			// TripleDESCryptoServiceProvider
			return EncryptToBase64String<TObject, RijndaelManaged>(obj, key, iv);
		}

		/// <summary>
		///		Encrypts and serializes the specified object using the specified key and
		///		initialization vector and returns a base 64 encoded string of the encrypted data.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Encryption initialization vector.</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string EncryptToBase64String<TObject, TSymmetricAlgorithm>(TObject obj, byte[] key, byte[] iv)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				return Encryption.EncryptToBase64String<TSymmetricAlgorithm>(key, iv,
					delegate(CryptoStream cryptoStream)
					{
						Serialize<TObject>(obj, cryptoStream);
					}
				);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML document to the specified
		///		<see cref="Stream"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="Stream"/> of which to write the XML document.</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize<TObject>(TObject obj, Stream output, bool expandForReadability = false)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(output, GetNewXmlWriterSettingsObject((expandForReadability))))
			{
				Serialize<TObject>(obj, xmlWriter);
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML document to the specified
		///		<see cref="StringBuilder"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="StringBuilder"/> of which to write the XML document.</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize<TObject>(TObject obj, StringBuilder output, bool expandForReadability = false)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(output, GetNewXmlWriterSettingsObject(expandForReadability)))
			{
				Serialize<TObject>(obj, xmlWriter);
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML document to the specified
		///		<see cref="TextWriter"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="TextWriter"/> of which to write the XML document.</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize<TObject>(TObject obj, TextWriter output, bool expandForReadability = false)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(output, GetNewXmlWriterSettingsObject(expandForReadability)))
			{
				Serialize<TObject>(obj, xmlWriter);
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML document to the specified
		///		<see cref="XmlWriter"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="XmlWriter"/> of which to write the XML document.</param>
		public static void Serialize<TObject>(TObject obj, XmlWriter output)
		{
			try
			{
				// This object removes the default namespaces created by the XmlSerializer.
				XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
				xmlnsEmpty.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
				//xmlnsEmpty.Add("", "");

				XmlSerializer serializer = new XmlSerializer(typeof(TObject));
				serializer.Serialize(output, obj, xmlnsEmpty);

				//// TEST CODE - BEGIN
				//StringBuilder xmlStringBuilder = new StringBuilder();
				//using (XmlWriter testWriter = XmlWriter.Create(xmlStringBuilder, GetNewXmlWriterSettingsObject()))
				//{
				//    serializer.Serialize(testWriter, obj, xmlnsEmpty);
				//    Debug.Print("XML Object: {0}\n", xmlStringBuilder.ToString());
				//}
				//// TEST CODE - END
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML document to a string.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string Serialize<TObject>(TObject obj, bool expandForReadability = false)
		{
			try
			{
				// This TextWriter ensures that the XML is formatted correctly.
				using (StringWriter stringWriter = new StringWriter())
				{
					Serialize<TObject>(obj, stringWriter, expandForReadability);

					return stringWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		#region Private Methods

		/// <summary>
		///		Create a default <see cref="XmlWriterSettings"/> object for formatting the XML output.
		/// </summary>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>An <see cref="XmlWriterSettings"/> object.</returns>
		private static XmlWriterSettings GetNewXmlWriterSettingsObject(bool expandForReadability)
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.ConformanceLevel = ConformanceLevel.Document;
			xmlWriterSettings.OmitXmlDeclaration = true;
			xmlWriterSettings.Encoding = Encoding.UTF8;
			xmlWriterSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;

			if (expandForReadability)
			{
				xmlWriterSettings.Indent = true;
				xmlWriterSettings.IndentChars = "    ";
				//xmlWriterSettings.NewLineOnAttributes = true;
			}

			return xmlWriterSettings;
		}

		#endregion
	}
}
