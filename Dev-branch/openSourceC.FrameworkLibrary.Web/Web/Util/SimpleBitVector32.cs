﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace openSourceC.FrameworkLibrary.Web.Util
{
	[Serializable, StructLayout(LayoutKind.Sequential)]
	internal struct SimpleBitVector32
	{
		private int data;
		internal SimpleBitVector32(int data)
		{
			this.data = data;
		}

		internal int IntegerValue
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}
		internal bool this[int bit]
		{
			get
			{
				return ((this.data & bit) == bit);
			}
			set
			{
				int data = this.data;
				if (value)
				{
					this.data = data | bit;
				}
				else
				{
					this.data = data & ~bit;
				}
			}
		}
		internal void Set(int bit)
		{
			this.data |= bit;
		}

		internal void Clear(int bit)
		{
			this.data &= ~bit;
		}
	}
}
