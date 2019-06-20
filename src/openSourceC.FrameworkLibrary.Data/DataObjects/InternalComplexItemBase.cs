using System;
using System.Collections.Generic;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		The InternalComplexItemBase abstract class is for internal FrameworkLibrary use.  It
	///		may be used as a reference for your own internal item base class.
	/// </summary>
	/// <typeparam name="TItem">The deriving object class.</typeparam>
	[Serializable]
	internal abstract class InternalComplexItemBase<TItem> : SimpleItemBase<TItem>
		where TItem : InternalComplexItemBase<TItem>
	{
		/// <summary>Indicates that the object has not been committed.</summary>
		private bool _isNew = true;
		/// <summary>Indicates that the object is marked for deletion.</summary>
		private bool _markedForDeletion = false;


		#region Class Constructors

		/// <summary>
		///		Class constructor.  Prevents external creation of objects.
		/// </summary>
		internal InternalComplexItemBase() { }

		#endregion

		#region Internal Properties

		/// <summary>
		///		Gets a value indicating that the uncommitted dirt fields differ from the
		///		committed dirt fields.  New objects that are not marked for deletion
		///		and committed objects that are marked for deletion are always dirty.
		/// </summary>
		internal abstract bool IsDirty { get; }

		/// <summary>
		///		Gets or sets a value indicating that the object is marked for deletion.
		/// </summary>
		internal bool IsMarkedForDeletion
		{
			get { return _markedForDeletion; }
			set { _markedForDeletion = value; }
		}

		/// <summary>
		///		Gets a value indicating that this record has not been committed.
		/// </summary>
		internal bool IsNew
		{
			get { return _isNew; }
		}

		#endregion

		#region Internal Methods

		/// <summary>
		///		Restore this object to its last committed state.
		/// </summary>
		internal void Restore()
		{
			CopyCommittedToUncommitted();

			_markedForDeletion = false;
		}

		/// <summary>
		///		Copies the uncommitted field values to the committed field values
		///		and sets the flags to their committed state.
		/// </summary>
		internal void SetCommitted()
		{
			CopyUncommittedToCommitted();

			_isNew = false;
			_markedForDeletion = false;
		}

		/// <summary>
		///		Resets the <see cref="P:IsNew"/> and <see cref="P:IsMarkedForDeletion"/> flags.
		/// </summary>
		internal void SetNew()
		{
			_isNew = true;
			_markedForDeletion = false;
		}

		#endregion

		#region Protected Internal Methods

		/// <summary>
		///		Copies the committed store to the uncommitted store.
		/// </summary>
		protected internal abstract void CopyCommittedToUncommitted();

		/// <summary>
		///		Copies the uncommitted store to the committed store.
		/// </summary>
		protected internal abstract void CopyUncommittedToCommitted();

		#endregion
	}

	/// <summary>
	///		The InternalComplexItemListBase abstract class is for internal FrameworkLibrary use.  It
	///		may be used as a reference for your own internal item list base class.
	/// </summary>
	/// <typeparam name="TList">The deriving collection class.</typeparam>
	/// <typeparam name="TItem">The data object class.</typeparam>
	[Serializable]
	internal abstract class InternalComplexItemListBase<TList, TItem> : SimpleItemListBase<TList, TItem>
		where TList : InternalComplexItemListBase<TList, TItem>
		where TItem : InternalComplexItemBase<TItem>
	{
		#region Constructors

		/// <summary>
		///		Initializes a new instance of the <c>InternalComplexItemListBase</c> class
		///		that is empty and has the default initial capacity.
		/// </summary>
		internal InternalComplexItemListBase() { }

		/// <summary>
		///		Initializes a new instance of the <c>InternalComplexItemListBase</c> class
		///		that is empty and has the default initial capacity.
		/// </summary>
		protected InternalComplexItemListBase(IList<TItem> list) : base(list) { }

		#endregion

		#region Deconstructors

		/// <summary>
		///     Class deconstructor.
		/// </summary>
		~InternalComplexItemListBase()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}

		#endregion
	}
}
