using System;

namespace openSourceC.FrameworkLibrary.Web
{
	/// <summary>
	///		
	/// </summary>
	public enum FileAction
	{
		/// <summary></summary>
		Added = 1,
		/// <summary></summary>
		Dispose = -2,
		/// <summary></summary>
		Error = -1,
		/// <summary></summary>
		Modified = 3,
		/// <summary></summary>
		Overwhelming = 0,
		/// <summary></summary>
		Removed = 2,
		/// <summary></summary>
		RenamedNewName = 5,
		/// <summary></summary>
		RenamedOldName = 4
	}

	/// <summary>
	///		
	/// </summary>
	public enum IVType
	{
		/// <summary></summary>
		None,
		/// <summary></summary>
		Random,
		/// <summary></summary>
		Hash
	}

	/// <summary>
	///		
	/// </summary>
	[Flags]
	public enum VirtualPathOptions
	{
		/// <summary></summary>
		AllowAbsolutePath = 0x04,
		/// <summary></summary>
		AllowAllPath = 0x1c,
		/// <summary></summary>
		AllowAppRelativePath = 0x08,
		/// <summary></summary>
		AllowNull = 0x01,
		/// <summary></summary>
		AllowRelativePath = 0x10,
		/// <summary></summary>
		EnsureTrailingSlash = 0x02,
		/// <summary></summary>
		FailIfMalformed = 0x20
	}
}
