using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Serialization;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		The SimpleItemBase abstract class is the base class for data item classes that do not
	///		need to track changes.
	/// </summary>
	/// <typeparam name="TItem">The deriving object class.</typeparam>
	[Serializable]
	public abstract class SimpleItemBase<TItem> : IComparable<TItem>, IEqualityComparer<TItem>, IEquatable<TItem>
		where TItem : SimpleItemBase<TItem>
	{
		#region IComparable<T> Implmentations

		/// <summary>
		///		Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">The object to compare with this object.</param>
		/// <returns>
		///		A 32-bit signed integer that indicates the relative order of the
		///		comparands. The return value has these meanings:
		///		<list type="bullet">
		///			<item>Less than zero, this instance is less than <paramref name="other"/>.</item>
		///			<item>Zero, this instance is equal to <paramref name="other"/>.</item>
		///			<item>Greater than zero, this instance is greater than <paramref name="other"/>.</item>
		///		</list>
		///	</returns>
		public abstract int CompareTo(TItem other);

		#endregion

		#region IEqualityComparer<T> Implmentations

		/// <summary>
		///		Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns>
		///		<b>true</b> if the specified objects are equal; otherwise <b>false</b>.
		/// </returns>
		public bool Equals(TItem x, TItem y)
		{
			if (x == null || y == null)
			{
				if (x == null && y == null)
				{
					return true;
				}

				return false;
			}

			return x.Equals(y);
		}

		/// <summary>
		///		Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The object for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(TItem obj)
		{
			return (obj == null ? 0 : obj.GetHashCode());
		}

		#endregion

		#region IEquatable<T> Implmentations

		/// <summary>
		///		Determines whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		///		<b>true</b> if the specified object is equal to the committed object; otherwise <b>false</b>.
		/// </returns>
		public abstract bool Equals(TItem other);

		#endregion

		#region Public Methods

		/// <summary>
		///		Returns an <see cref="object"/> array of the key values.
		/// </summary>
		/// <returns>
		///		An <see cref="object"/> array of the key values.
		///	</returns>
		public object[] GetKeyValues()
		{
			SortedList<int, PropertyInfo> keyProperties = new SortedList<int, PropertyInfo>();
			Type type = typeof(TItem);

			foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				if (Attribute.GetCustomAttribute(pi, typeof(KeyAttribute)) != null)
				{
					DisplayAttribute columnAttribute = (DisplayAttribute)Attribute.GetCustomAttribute(pi, typeof(DisplayAttribute));

					keyProperties.Add(columnAttribute == null ? -1 : columnAttribute.Order, pi);
				}

				//keyProperties.Add(new KeyColumnAttributes() { Key = keyAttribute, Column = (DisplayAttribute)Attribute.GetCustomAttribute(pi, typeof(DisplayAttribute)) });
			}

			if (keyProperties.Count == 0)
			{
				throw new OscErrorException("No key column(s).");
			}

			object[] returnValue = new object[keyProperties.Count];

			for (int i = 0; i < keyProperties.Count; i++)
			{
				returnValue[i] = type.InvokeMember(keyProperties[i].Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, null, this, null);
			}

			return returnValue;
		}

		#endregion

		#region Public Override/Overload Methods

		/// <summary>
		///		Determines whether the specified <see cref="object"/> is equal to the current
		///		<see cref="object"/>.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return base.Equals(obj);
			}

			if (!(obj is TItem))
			{
				throw new InvalidCastException(string.Format("The 'obj' argument is not a {0} object.", typeof(TItem).Name));
			}
			else
			{
				return Equals(obj as TItem);
			}
		}

		/// <summary>
		///		Returns the hash code for the committed object.
		/// </summary>
		/// <returns>A hash code for the committed object.</returns>
		public abstract override int GetHashCode();

		/// <summary>
		///		Returns a string that represents this instance.
		/// </summary>
		public abstract override string ToString();

		/// <summary>
		///		Returns a string that represents the contents of this instance.
		/// </summary>
		/// <param name="format">A format string.</param>
		public abstract string ToString(string format);

		#endregion
	}

	/// <summary>
	///		The SimpleItemListBase abstract class is the base class for data item collection classes
	///		that do not need to track changes.
	/// </summary>
	/// <typeparam name="TList">The deriving collection class.</typeparam>
	/// <typeparam name="TItem">The data object class.</typeparam>
	[Serializable]
	public abstract class SimpleItemListBase<TList, TItem> : List<TItem>, IDisposable
		where TList : SimpleItemListBase<TList, TItem>
		where TItem : SimpleItemBase<TItem>
	{
		/// <summary>The committed <see cref="IComparer&lt;TItem&gt;"/> being used for sorting.</summary>
		private IComparer<TItem> _currentComparer;
		private bool _isCurrentComparerValid = false;

		// Track whether Dispose has been called.
		private bool _disposed = false;


		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <c>SimpleItemListBase</c> class
		///		that is empty and has the default initial capacity.
		/// </summary>
		public SimpleItemListBase() { }

		/// <summary>
		///		Initializes a new instance of the <c>SimpleItemListBase</c> class
		///		that is empty and has the default initial capacity.
		/// </summary>
		protected SimpleItemListBase(IList<TItem> list) : base(list) { }

		#endregion

		#region Dispose

		/// <summary>
		///     Releases all resources used by this instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			//GC.SuppressFinalize(this);
		}

		/// <summary>
		///     Releases the unmanaged resources used by this instance and optionally
		///     releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		///     If true, the method has been called directly or indirectly by a user's code.
		///     Managed and unmanaged resources can be disposed. If false, the method has been
		///     called by the runtime from inside the finalizer and you should not reference
		///     other objects. Only unmanaged resources can be disposed.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose() has already been called.
			if (!_disposed)
			{
				// Check to see if managed resources need to be disposed of.
				if (disposing)
				{
					// Dispose managed resources.

					// Nullify references to managed resources that are not disposable.

					// Nullify references to externally created managed resources.
				}

				_disposed = true;
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		///		Gets the <see cref="IComparer&lt;T&gt;"/> that is used to determine equality of keys for the dictionary.
		/// </summary>
		[XmlIgnore]
		public IComparer<TItem> CurrentComparer
		{
			get
			{
				if (!_isCurrentComparerValid)
				{
					throw new InvalidOperationException("The current comparer is not valid.");
				}

				return _currentComparer;
			}
		}

		/// <summary>
		///		Gets a value indicating whether or not the current comparer is valid.
		/// </summary>
		[XmlIgnore]
		public bool IsCurrentComparerValid
		{
			get { return _isCurrentComparerValid; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		///		Sorts the elements in the entire <see cref="List&lt;T&gt;"/> using the default comparer.
		/// </summary>
		public new void Sort()
		{
			_currentComparer = null;
			_isCurrentComparerValid = true;
			base.Sort();
		}

		/// <summary>
		///		Sorts the elements in the entire <see cref="List&lt;T&gt;"/> using the specified <see cref="Comparison&lt;T&gt;"/>.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparison&lt;TItem&gt;"/> to use when comparing elements.</param>
		public new void Sort(Comparison<TItem> comparison)
		{
			_isCurrentComparerValid = false;
			base.Sort(comparison);
		}

		/// <summary>
		///		Sorts the elements in the entire <see cref="List&lt;T&gt;"/> using the specified comparer.
		/// </summary>
		/// <param name="comparer">The <see cref="IComparer&lt;TItem&gt;"/> to use when comparing
		///		elements, or a null reference to use the default comparer
		///		<see cref="System.Collections.Generic.Comparer&lt;T&gt;.Default"/>.</param>
		public new void Sort(IComparer<TItem> comparer)
		{
			_currentComparer = comparer;
			_isCurrentComparerValid = true;
			base.Sort(comparer);
		}

		/// <summary>
		///		Sorts the elements in a range of elements in <see cref="List&lt;T&gt;"/> using the
		///		specified comparer.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range to sort.</param>
		/// <param name="count">The length of the range to sort.</param>
		/// <param name="comparer">The <see cref="IComparer&lt;TItem&gt;"/> to use when comparing
		///		elements, or a null reference to use the default comparer
		///		<see cref="System.Collections.Generic.Comparer&lt;T&gt;.Default"/>.</param>
		public new void Sort(int index, int count, IComparer<TItem> comparer)
		{
			_currentComparer = comparer;
			_isCurrentComparerValid = true;
			base.Sort(index, count, comparer);
		}

		#endregion
	}
}
