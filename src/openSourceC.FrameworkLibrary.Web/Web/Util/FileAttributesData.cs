using System;
using System.IO;
using System.Web;
using System.Runtime.InteropServices;

namespace openSourceC.FrameworkLibrary.Web.Util
{
	internal sealed class FileAttributesData
	{
		// Fields
		internal readonly FileAttributes FileAttributes;
		internal readonly long FileSize;
		internal readonly DateTime UtcCreationTime;
		internal readonly DateTime UtcLastAccessTime;
		internal readonly DateTime UtcLastWriteTime;

		// Methods
		private FileAttributesData()
		{
			this.FileSize = -1L;
		}

		private FileAttributesData(ref UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA data)
		{
			this.FileAttributes = (FileAttributes)data.fileAttributes;
			this.UtcCreationTime = DateTimeUtil.FromFileTimeToUtc((long)((data.ftCreationTimeHigh << 0x20) | data.ftCreationTimeLow));
			this.UtcLastAccessTime = DateTimeUtil.FromFileTimeToUtc((long)((data.ftLastAccessTimeHigh << 0x20) | data.ftLastAccessTimeLow));
			this.UtcLastWriteTime = DateTimeUtil.FromFileTimeToUtc((long)((data.ftLastWriteTimeHigh << 0x20) | data.ftLastWriteTimeLow));
			this.FileSize = (data.fileSizeHigh << 0x20) | data.fileSizeLow;
		}

		internal FileAttributesData(ref UnsafeNativeMethods.WIN32_FIND_DATA wfd)
		{
			this.FileAttributes = (FileAttributes)wfd.dwFileAttributes;
			this.UtcCreationTime = DateTimeUtil.FromFileTimeToUtc((long)((wfd.ftCreationTime_dwHighDateTime << 0x20) | wfd.ftCreationTime_dwLowDateTime));
			this.UtcLastAccessTime = DateTimeUtil.FromFileTimeToUtc((long)((wfd.ftLastAccessTime_dwHighDateTime << 0x20) | wfd.ftLastAccessTime_dwLowDateTime));
			this.UtcLastWriteTime = DateTimeUtil.FromFileTimeToUtc((long)((wfd.ftLastWriteTime_dwHighDateTime << 0x20) | wfd.ftLastWriteTime_dwLowDateTime));
			this.FileSize = (wfd.nFileSizeHigh << 0x20) | wfd.nFileSizeLow;
		}

		internal static int GetFileAttributes(string path, out FileAttributesData fad)
		{
			UnsafeNativeMethods.WIN32_FILE_ATTRIBUTE_DATA win_file_attribute_data;
			fad = null;
			if (!UnsafeNativeMethods.GetFileAttributesEx(path, 0, out win_file_attribute_data))
			{
				//return HttpException.HResultFromLastError(Marshal.GetLastWin32Error());
				return -1;
			}
			fad = new FileAttributesData(ref win_file_attribute_data);
			return 0;
		}

		// Properties
		internal static FileAttributesData NonExistantAttributesData
		{
			get
			{
				return new FileAttributesData();
			}
		}
	}
}
