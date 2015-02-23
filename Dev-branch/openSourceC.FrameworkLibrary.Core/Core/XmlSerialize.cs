using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Summary description for XmlSerializationWrapper&lt;TItem&gt;.
	/// </summary>
	[XmlRoot("root")]
	[Serializable]
	public class XmlSerializationWrapper<TItem>
	{
		/// <summary></summary>
		[XmlElement("record")]
		public List<TItem> ItemList { get; set; }
	}

	/// <summary>
	///		Summary description for XmlSerialize.
	/// </summary>
	public static class XmlSerialize
	{
		// This object removes the default namespaces created by the XmlSerializer.
		private static readonly XmlSerializerNamespaces _xmlnsEmpty = new XmlSerializerNamespaces(
			new XmlQualifiedName[] {
				new XmlQualifiedName(string.Empty, string.Empty),
			}
		);

		// This object removes the default namespaces created by the XmlSerializer.
		private static readonly XmlSerializerNamespaces _xmlnsNil = new XmlSerializerNamespaces(
			new XmlQualifiedName[] {
				new XmlQualifiedName(string.Empty, string.Empty),
				new XmlQualifiedName("xsi", "http://www.w3.org/2001/XMLSchema-instance"),
			}
		);


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
			return (TObject)(Deserialize(typeof(TObject), stream) ?? default(TObject));
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
			return (TObject)(Deserialize(typeof(TObject), xmlString) ?? default(TObject));
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
			return (TObject)(Deserialize(typeof(TObject), textReader) ?? default(TObject));
		}

		/// <summary>
		///		Deserializes the specified <see cref="T:XElement"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="node">The <see cref="T:XElement"/> to be deserialized.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject Deserialize<TObject>(XElement node)
		{
			return (TObject)(Deserialize(typeof(TObject), node) ?? default(TObject));
		}

		/// <summary>
		///		Deserializes the specified <see cref="T:XmlElement"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="element">The <see cref="T:XmlElement"/> to be deserialized.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject Deserialize<TObject>(XmlElement element)
		{
			return (TObject)(Deserialize(typeof(TObject), element) ?? default(TObject));
		}

		/// <summary>
		///		Deserializes the XML document contained by the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to deserialize.</typeparam>
		/// <param name="xmlReader">The <see cref="XmlReader"/> that contains the XML document
		///		to deserialize.</param>
		/// <returns>
		///		An object of type <typeparamref name="TObject"/>.
		/// </returns>
		public static TObject Deserialize<TObject>(XmlReader xmlReader)
		{
			return (TObject)(Deserialize(typeof(TObject), xmlReader) ?? default(TObject));
		}

		/// <summary>
		///		Deserializes the XML document contained by the specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="stream">The <see cref="Stream"/> that contains the XML document to
		///		deserialize.</param>
		/// <returns>
		///		An object of the specified type.
		/// </returns>
		public static object Deserialize(Type type, Stream stream)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			if (stream == null || (stream.CanSeek && stream.Length == 0L)) { return null; }

			try
			{
				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(stream);
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
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="xmlString">The string to be deserialized.</param>
		/// <returns>
		///		An object of the specified type.
		/// </returns>
		public static object Deserialize(Type type, string xmlString)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			if (string.IsNullOrWhiteSpace(xmlString)) { return null; }

			try
			{
				using (StringReader stringReader = new StringReader(xmlString))
				{
					XmlSerializer serializer = new XmlSerializer(type);
					return serializer.Deserialize(stringReader);
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
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="textReader">The <see cref="TextReader"/> that contains the XML document
		///		to deserialize.</param>
		/// <returns>
		///		An object of the specified type.
		/// </returns>
		public static object Deserialize(Type type, TextReader textReader)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			if (textReader == null) { return null; }

			try
			{
				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(textReader);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Deserializes the specified <see cref="T:XElement"/>.
		/// </summary>
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="node">The <see cref="T:XElement"/> to be deserialized.</param>
		/// <returns>
		///		An object of the specified type.
		/// </returns>
		public static object Deserialize(Type type, XElement node)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			if (node == null) { return null; }

			using (XmlReader xmlReader = node.CreateReader())
			{
				return Deserialize(type, xmlReader);
			}
		}

		/// <summary>
		///		Deserializes the specified <see cref="T:XmlElement"/>.
		/// </summary>
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="element">The <see cref="T:XmlElement"/> to be deserialized.</param>
		/// <returns>
		///		An object of the specified type.
		/// </returns>
		public static object Deserialize(Type type, XmlElement element)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			if (element == null) { return null; }

			try
			{
				XmlReaderSettings readerSettings = new XmlReaderSettings
				{
					ConformanceLevel = ConformanceLevel.Fragment,
				};

				// This XmlWriter ensures that the XML is formatted correctly.
				using (MemoryStream stream = new MemoryStream())
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(stream, GetNewXmlWriterSettingsObject(false)))
					{
						element.WriteTo(xmlWriter);

						xmlWriter.Flush();
						stream.Seek(0L, SeekOrigin.Begin);
					}

					using (XmlReader xmlReader = XmlReader.Create(stream, readerSettings))
					{
						xmlReader.MoveToContent();

						return Deserialize(type, xmlReader);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Deserializes the XML document contained by the specified <see cref="XmlReader"/>.
		/// </summary>
		/// <param name="type">The type of the object to deserialize.</param>
		/// <param name="xmlReader">The <see cref="XmlReader"/> that contains the XML document
		///		to deserialize.</param>
		/// <returns>
		///		An object of the specified type.
		/// </returns>
		public static object Deserialize(Type type, XmlReader xmlReader)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			if (xmlReader == null) { return null; }

			try
			{
				XmlSerializer serializer = new XmlSerializer(type);
				return serializer.Deserialize(xmlReader);
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
		/// <typeparam name="TSymmetricAlgorithm">The type of the cryptographic object used to
		///		perform the symmetric algorithm.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Encryption initialization vector.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string EncryptToBase64String<TObject, TSymmetricAlgorithm>(TObject obj, byte[] key, byte[] iv, bool supportXsiNamespace = false)
			where TSymmetricAlgorithm : SymmetricAlgorithm, new()
		{
			try
			{
				return Encryption.EncryptToBase64String<TSymmetricAlgorithm>(key, iv,
					delegate(CryptoStream cryptoStream)
					{
						Serialize<TObject>(obj, cryptoStream, supportXsiNamespace, false);
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
		///		Encrypts and serializes the specified object using the <see cref="T:RijndaelManaged"/>
		///		algorithm, specified key, and initialization vector, returning a base 64 encoded
		///		string of the encrypted data.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="key">Encryption key.</param>
		/// <param name="iv">Encryption initialization vector.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string EncryptToBase64String<TObject>(TObject obj, byte[] key, byte[] iv, bool supportXsiNamespace = false)
		{
			// RijndaelManaged
			// DESCryptoServiceProvider
			// RC2CryptoServiceProvider
			// TripleDESCryptoServiceProvider
			return EncryptToBase64String<TObject, RijndaelManaged>(obj, key, iv, supportXsiNamespace);
		}

		/// <summary>
		///		Serializes the specified object and returning the XML fragment as a string.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string Serialize<TObject>(TObject obj, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			return Serialize(typeof(TObject), obj, supportXsiNamespace, expandForReadability);
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="Stream"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="Stream"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize<TObject>(TObject obj, Stream output, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			Serialize(typeof(TObject), obj, output, supportXsiNamespace, expandForReadability);
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="StringBuilder"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="StringBuilder"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize<TObject>(TObject obj, StringBuilder output, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			Serialize(typeof(TObject), obj, output, supportXsiNamespace, expandForReadability);
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="TextWriter"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="TextWriter"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize<TObject>(TObject obj, TextWriter output, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			Serialize(typeof(TObject), obj, output, supportXsiNamespace, expandForReadability);
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="XmlWriter"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="XmlWriter"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		public static void Serialize<TObject>(TObject obj, XmlWriter output, bool supportXsiNamespace = false)
		{
			Serialize(typeof(TObject), obj, output, supportXsiNamespace);
		}

		/// <summary>
		///		Serializes the specified object and returning the XML fragment as a string.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An XML formatted string representation of the object.
		/// </returns>
		public static string Serialize(Type type, object obj, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			if (type == null) { throw new ArgumentNullException("type"); }

			try
			{
				// This TextWriter ensures that the XML is formatted correctly.
				using (StringWriter stringWriter = new StringWriter())
				{
					Serialize(type, obj, stringWriter, supportXsiNamespace, expandForReadability);

					return stringWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="Stream"/>.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="Stream"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize(Type type, object obj, Stream output, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(output, GetNewXmlWriterSettingsObject((expandForReadability))))
			{
				Serialize(type, obj, xmlWriter, supportXsiNamespace);
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="StringBuilder"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize(Type type, object obj, StringBuilder output, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(output, GetNewXmlWriterSettingsObject(expandForReadability)))
			{
				Serialize(type, obj, xmlWriter, supportXsiNamespace);
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="TextWriter"/>.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="TextWriter"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		public static void Serialize(Type type, object obj, TextWriter output, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			using (XmlWriter xmlWriter = XmlWriter.Create(output, GetNewXmlWriterSettingsObject(expandForReadability)))
			{
				Serialize(type, obj, xmlWriter, supportXsiNamespace);
			}
		}

		/// <summary>
		///		Serializes the specified object and writes the XML fragment to the specified
		///		<see cref="XmlWriter"/>.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="output">The <see cref="XmlWriter"/> of which to write the XML document.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		public static void Serialize(Type type, object obj, XmlWriter output, bool supportXsiNamespace = false)
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(type);
				serializer.Serialize(output, obj, (supportXsiNamespace ? _xmlnsNil : _xmlnsEmpty));

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
		///		Serializes the specified object and returns the XML fragment as an <see cref="T:XElement"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An <see cref="T:XElement"/> representation of the object.
		/// </returns>
		public static XElement SerializeToXElement<TObject>(TObject obj, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			return SerializeToXElement(typeof(TObject), obj, supportXsiNamespace, expandForReadability);
		}

		/// <summary>
		///		Serializes the specified object and returns the XML fragment as an <see cref="T:XElement"/>.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An <see cref="T:XElement"/> representation of the object.
		/// </returns>
		public static XElement SerializeToXElement(Type type, object obj, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			try
			{
				XmlReaderSettings readerSettings = new XmlReaderSettings
				{
					ConformanceLevel = ConformanceLevel.Fragment,
					IgnoreWhitespace = !expandForReadability,
				};

				using (MemoryStream stream = new MemoryStream())
				// This XmlWriter ensures that the XML is formatted correctly.
				using (XmlWriter xmlWriter = XmlWriter.Create(stream, GetNewXmlWriterSettingsObject(expandForReadability)))
				{
					XmlSerializer serializer = new XmlSerializer(type);
					serializer.Serialize(xmlWriter, obj, (supportXsiNamespace ? _xmlnsNil : _xmlnsEmpty));

					xmlWriter.Flush();
					stream.Seek(0L, SeekOrigin.Begin);

					return XElement.Load(stream, (expandForReadability ? LoadOptions.PreserveWhitespace : LoadOptions.None));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(Format.Exception(ex));
				throw;
			}
		}

		/// <summary>
		///		Serializes the specified object and returns the XML fragment as an <see cref="T:XmlElement"/>.
		/// </summary>
		/// <typeparam name="TObject">The type of the object to serialize.</typeparam>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An <see cref="T:XmlElement"/> representation of the object.
		/// </returns>
		public static XmlElement SerializeToXmlElement<TObject>(TObject obj, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			return SerializeToXmlElement(typeof(TObject), obj, supportXsiNamespace, expandForReadability);
		}

		/// <summary>
		///		Serializes the specified object and returns the XML fragment as an <see cref="T:XmlElement"/>.
		/// </summary>
		/// <param name="type">The type of the object to serialize.</param>
		/// <param name="obj">The <see cref="object"/> to serialize.</param>
		/// <param name="supportXsiNamespace"><b>true</b> to support xsi namespace (i.e. xsi:type,
		///		xsi:nil, xsi:schema, xsi:schemaLocation, and xsi:noNamespaceSchemaLocation).</param>
		/// <param name="expandForReadability"><b>true</b> to expand the XML with line feeds and
		///		spacing, <b>false</b> to remove all readability formatting.</param>
		/// <returns>
		///		An <see cref="T:XmlElement"/> representation of the object.
		/// </returns>
		public static XmlElement SerializeToXmlElement(Type type, object obj, bool supportXsiNamespace = false, bool expandForReadability = false)
		{
			try
			{
				using (MemoryStream stream = new MemoryStream())
				// This XmlWriter ensures that the XML is formatted correctly.
				using (XmlWriter xmlWriter = XmlWriter.Create(stream, GetNewXmlWriterSettingsObject(expandForReadability)))
				{
					XmlSerializer serializer = new XmlSerializer(type);
					serializer.Serialize(xmlWriter, obj, (supportXsiNamespace ? _xmlnsNil : _xmlnsEmpty));

					xmlWriter.Flush();
					stream.Seek(0L, SeekOrigin.Begin);

					XmlDocument doc = new XmlDocument();
					doc.PreserveWhitespace = expandForReadability;
					doc.Load(stream);

					return doc.DocumentElement;
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
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
			{
				ConformanceLevel = ConformanceLevel.Document,
				OmitXmlDeclaration = true,
				Encoding = Encoding.UTF8,
				NamespaceHandling = NamespaceHandling.OmitDuplicates,
			};

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
