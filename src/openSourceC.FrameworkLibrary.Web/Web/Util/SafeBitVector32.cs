using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace openSourceC.FrameworkLibrary.Web.Util
{
	[Serializable, StructLayout(LayoutKind.Sequential)]
	internal struct SafeBitVector32
	{
		private volatile int _data;


		internal SafeBitVector32(int data)
		{
			this._data = data;
		}

		internal bool this[int bit]
		{
			get { return ((this._data & bit) == bit); }

			set
			{
				int num;
				int num2;


				do
				{
					num = this._data;
					if (value)
					{
						num2 = num | bit;
					}
					else
					{
						num2 = num & ~bit;
					}
				}

#pragma warning disable 420
				while (Interlocked.CompareExchange(ref this._data, num2, num) != num);
#pragma warning restore 420
			}
		}

		internal bool ChangeValue(int bit, bool value)
		{
			int num;
			int num2;


			do
			{
				num = this._data;

				if (value)
				{
					num2 = num | bit;
				}
				else
				{
					num2 = num & ~bit;
				}

				if (num == num2)
				{
					return false;
				}
			}

#pragma warning disable 420
			while (Interlocked.CompareExchange(ref this._data, num2, num) != num);
#pragma warning restore 420

			return true;
		}
	}
}
