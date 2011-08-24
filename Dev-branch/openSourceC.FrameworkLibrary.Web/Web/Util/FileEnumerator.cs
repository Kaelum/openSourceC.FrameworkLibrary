using System;
using System.Collections;
using System.IO;
using System.Web;

namespace openSourceC.FrameworkLibrary.Web.Util
{
	internal class FileEnumerator : FileData, IEnumerable, IEnumerator, IDisposable
	{
		private IntPtr _hFindFile = UnsafeNativeMethods.INVALID_HANDLE_VALUE;


		#region Constructors

		private FileEnumerator(string path)
		{
			base._path = Path.GetFullPath(path);
		}

		~FileEnumerator()
		{
			((IDisposable)this).Dispose();
		}

		#endregion

		#region Methods

		internal static FileEnumerator Create(string path)
		{
			return new FileEnumerator(path);
		}

		#endregion

		private bool SkipCurrent()
		{
			if (!(this._wfd.cFileName == ".") && !(this._wfd.cFileName == ".."))
			{
				return false;
			}
			return true;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		bool IEnumerator.MoveNext()
		{
			do
			{
				if (this._hFindFile == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
				{
					this._hFindFile = UnsafeNativeMethods.FindFirstFile(base._path + @"\*.*", out this._wfd);
					if (this._hFindFile == UnsafeNativeMethods.INVALID_HANDLE_VALUE)
					{
						return false;
					}
				}
				else if (!UnsafeNativeMethods.FindNextFile(this._hFindFile, out this._wfd))
				{
					return false;
				}
			}
			while (this.SkipCurrent());
			return true;
		}

		void IEnumerator.Reset()
		{
			throw new InvalidOperationException();
		}

		void IDisposable.Dispose()
		{
			if (this._hFindFile != UnsafeNativeMethods.INVALID_HANDLE_VALUE)
			{
				UnsafeNativeMethods.FindClose(this._hFindFile);

				this._hFindFile = UnsafeNativeMethods.INVALID_HANDLE_VALUE;
			}
			GC.SuppressFinalize(this);
		}

		object IEnumerator.Current
		{
			get { return this; }
		}
	}

}
