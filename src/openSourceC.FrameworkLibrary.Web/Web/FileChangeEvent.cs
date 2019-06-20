using System;
using System.Web;

namespace openSourceC.FrameworkLibrary.Web
{
	internal sealed class FileChangeEvent : EventArgs
	{
		internal FileAction Action;
		internal string FileName;

		internal FileChangeEvent(FileAction action, string fileName)
		{
			this.Action = action;
			this.FileName = fileName;
		}
	}
}
