using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace openSourceC.FrameworkLibrary.Web.Util
{
	/// <summary>
	///		
	/// </summary>
	public class HashCodeCombiner
	{
		// Fields
		private long _combinedHash;


		#region Constructors

		/// <summary>
		///		Class constructor.
		/// </summary>
		public HashCodeCombiner()
		{
			_combinedHash = 0x1505L;
		}

		/// <summary>
		///		Class constructor.
		/// </summary>
		public HashCodeCombiner(long initialCombinedHash)
		{
			_combinedHash = initialCombinedHash;
		}

		#endregion

		#region Properties

		/// <summary>
		///		
		/// </summary>
		public long CombinedHash
		{
			get { return _combinedHash; }
		}

		/// <summary>
		///		
		/// </summary>
		public int CombinedHash32
		{
			get { return _combinedHash.GetHashCode(); }
		}

		/// <summary>
		///		
		/// </summary>
		public string CombinedHashString
		{
			get { return _combinedHash.ToString("x", CultureInfo.InvariantCulture); }
		}

		#endregion

		#region Methods

		/// <summary>
		///		
		/// </summary>
		/// <param name="a"></param>
		public void AddArray(string[] a)
		{
			if (a != null)
			{
				int length = a.Length;

				for (int i = 0; i < length; i++)
				{
					this.AddObject(a[i]);
				}
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		public void AddCaseInsensitiveString(string s)
		{
			if (s != null)
			{
				AddInt(StringComparer.InvariantCultureIgnoreCase.GetHashCode(s));
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="dt"></param>
		public void AddDateTime(DateTime dt)
		{
			AddInt(dt.GetHashCode());
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="directoryName"></param>
		internal void AddDirectory(string directoryName)
		{
			DirectoryInfo info = new DirectoryInfo(directoryName);
			if (info.Exists)
			{
				this.AddObject(directoryName);
				foreach (FileData data in (IEnumerable)FileEnumerator.Create(directoryName))
				{
					if (data.IsDirectory)
					{
						this.AddDirectory(data.FullName);
					}
					else
					{
						this.AddExistingFile(data.FullName);
					}
				}
				this.AddDateTime(info.CreationTimeUtc);
				this.AddDateTime(info.LastWriteTimeUtc);
			}
		}

		private void AddExistingFile(string fileName)
		{
			this.AddInt(StringUtil.GetStringHashCode(fileName));
			FileInfo info = new FileInfo(fileName);
			this.AddDateTime(info.CreationTimeUtc);
			this.AddDateTime(info.LastWriteTimeUtc);
			this.AddFileSize(info.Length);
		}

		//public void AddFile(string fileName)
		//{
		//    if (!FileUtil.FileExists(fileName))
		//    {
		//        if (FileUtil.DirectoryExists(fileName))
		//        {
		//            this.AddDirectory(fileName);
		//        }
		//    }
		//    else
		//    {
		//        this.AddExistingFile(fileName);
		//    }
		//}

		private void AddFileSize(long fileSize)
		{
			AddInt(fileSize.GetHashCode());
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="n"></param>
		public void AddInt(int n)
		{
			_combinedHash = ((this._combinedHash << 5) + this._combinedHash) ^ n;
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="b"></param>
		public void AddObject(bool b)
		{
			AddInt(b.GetHashCode());
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="b"></param>
		public void AddObject(byte b)
		{
			AddInt(b.GetHashCode());
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="n"></param>
		public void AddObject(int n)
		{
			AddInt(n);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="l"></param>
		public void AddObject(long l)
		{
			AddInt(l.GetHashCode());
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="o"></param>
		public void AddObject(object o)
		{
			if (o != null)
			{
				AddInt(o.GetHashCode());
			}
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="s"></param>
		public void AddObject(string s)
		{
			if (s != null)
			{
				AddInt(s.GetHashCode());
			}
		}

		//public void AddResourcesDirectory(string directoryName)
		//{
		//    DirectoryInfo info = new DirectoryInfo(directoryName);
		//    if (info.Exists)
		//    {
		//        this.AddObject(directoryName);
		//        foreach (FileData data in (IEnumerable)FileEnumerator.Create(directoryName))
		//        {
		//            if (data.IsDirectory)
		//            {
		//                this.AddResourcesDirectory(data.FullName);
		//            }
		//            else
		//            {
		//                string fullName = data.FullName;
		//                if (Util.GetCultureName(fullName) == null)
		//                {
		//                    this.AddExistingFile(fullName);
		//                }
		//            }
		//        }
		//        this.AddDateTime(info.CreationTimeUtc);
		//    }
		//}

		/// <summary>
		///		
		/// </summary>
		/// <param name="h1"></param>
		/// <param name="h2"></param>
		/// <returns></returns>
		public static int CombineHashCodes(int h1, int h2)
		{
			return (((h1 << 5) + h1) ^ h2);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="h1"></param>
		/// <param name="h2"></param>
		/// <param name="h3"></param>
		/// <returns></returns>
		public static int CombineHashCodes(int h1, int h2, int h3)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2), h3);
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="h1"></param>
		/// <param name="h2"></param>
		/// <param name="h3"></param>
		/// <param name="h4"></param>
		/// <returns></returns>
		public static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2), CombineHashCodes(h3, h4));
		}

		/// <summary>
		///		
		/// </summary>
		/// <param name="h1"></param>
		/// <param name="h2"></param>
		/// <param name="h3"></param>
		/// <param name="h4"></param>
		/// <param name="h5"></param>
		/// <returns></returns>
		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return CombineHashCodes(CombineHashCodes(h1, h2, h3, h4), h5);
		}

		//public static string GetDirectoryHash(VirtualPath virtualDir)
		//{
		//    HashCodeCombiner combiner = new HashCodeCombiner();
		//    combiner.AddDirectory(virtualDir.MapPathInternal());
		//    return combiner.CombinedHashString;
		//}

		#endregion
	}
}
