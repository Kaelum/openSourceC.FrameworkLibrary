using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using openSourceC.FrameworkLibrary.Data;

namespace SampleApp
{
	public class MyDataItem : SimpleItemBase<MyDataItem>
	{
		[Key]
		[Column(Order = 0)]
		public int PrimaryKey0 { get; set; }

		[Key]
		[Column(Order = 1)]
		public int PrimaryKey1 { get; set; }

		public string DataString { get; set; }

		public override int CompareTo(MyDataItem other)
		{
			throw new NotImplementedException();
		}

		public override bool Equals(MyDataItem other)
		{
			throw new NotImplementedException();
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public override string ToString(string format)
		{
			throw new NotImplementedException();
		}
	}
}
