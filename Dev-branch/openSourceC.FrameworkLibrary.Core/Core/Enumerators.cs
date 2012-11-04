using System;

namespace openSourceC.FrameworkLibrary
{
	/// <summary>
	///		Defines the states of a 3-way switch.
	/// </summary>
	public enum ForceStateSwitch
	{
		/// <summary>Force the switch to the off state.</summary>
		ForceOff = -1,

		/// <summary>Leave the switch in its current or automatic state.</summary>
		Automatic = 0,

		/// <summary>Force the switch to the on state.</summary>
		ForceOn = 1
	}
}
