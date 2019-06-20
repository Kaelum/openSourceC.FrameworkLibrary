using System;
using System.Collections.Generic;

namespace openSourceC.FrameworkLibrary.Data
{
	/// <summary>
	///		The ComplexItemBase abstract class should be implemented by all data item
	///		classes of collections that use the DataAccess.ExecuteReader() method.
	/// </summary>
	/// <typeparam name="TItem">The deriving object class.</typeparam>
	[Serializable]
	public abstract class ComplexItemBase<TItem> : SimpleItemBase<TItem>
		where TItem : ComplexItemBase<TItem>
	{
		#region Constructors

		/// <summary>
		///		Constructor.
		/// </summary>
		public ComplexItemBase()
		{
			IsNew = true;
			IsMarkedForDeletion = false;
		}

		#endregion

		#region Public Abstract Properties

		/// <summary>
		///		Gets a value indicating that the uncommitted dirt fields differ from the
		///		committed dirt fields.  New objects that are not marked for deletion
		///		and committed objects that are marked for deletion are always dirty.
		/// </summary>
		public abstract bool IsDirty { get; }

		#endregion

		#region Public Properties

		/// <summary>
		///		Gets or sets a value indicating that the object is marked for deletion.
		/// </summary>
		public bool IsMarkedForDeletion { get; set; }

		/// <summary>
		///		Gets a value indicating that this record has not been committed.
		/// </summary>
		public bool IsNew { get; private set; }

		#endregion

		#region Public Methods

		/// <summary>
		///		Restore this object to its last committed state and reset the
		///		<see cref="P:IsMarkedForDeletion"/> flag.
		/// </summary>
		public void Restore()
		{
			CopyCommittedToUncommitted();

			IsMarkedForDeletion = false;
		}

		/// <summary>
		///		Copies the uncommitted field values to the committed field values
		///		and sets the flags to their committed state.
		/// </summary>
		public void SetCommitted()
		{
			CopyUncommittedToCommitted();

			IsNew = false;
			IsMarkedForDeletion = false;
		}

		/// <summary>
		///		Resets the <see cref="P:IsNew"/> and <see cref="P:IsMarkedForDeletion"/> flags.
		/// </summary>
		public void SetNew()
		{
			IsNew = true;
			IsMarkedForDeletion = false;
		}

		#endregion

		#region Protected Abstract Methods

		/// <summary>
		///		Copies the committed store to the uncommitted store.
		/// </summary>
		protected abstract void CopyCommittedToUncommitted();

		/// <summary>
		///		Copies the uncommitted store to the committed store.
		/// </summary>
		protected abstract void CopyUncommittedToCommitted();

		#endregion
	}

	/// <summary>
	///		The ComplexItemListBase abstract class should be implemented by all data item
	///		collection classes that use the DataAccess.ExecuteReader() method.
	/// </summary>
	/// <typeparam name="TList">The deriving collection class.</typeparam>
	/// <typeparam name="TItem">The data object class.</typeparam>
	[Serializable]
	public abstract class ComplexItemListBase<TList, TItem> : SimpleItemListBase<TList, TItem>
		where TList : ComplexItemListBase<TList, TItem>
		where TItem : ComplexItemBase<TItem>
	{
		#region Class Constructors / Deconstructors

		/// <summary>
		///		Initializes a new instance of the <c>ComplexItemBaseList</c> class
		///		that is empty and has the default initial capacity.
		/// </summary>
		public ComplexItemListBase()
		{
		}

		/// <summary>
		///		Initializes a new instance of the <c>ComplexItemBaseList</c> class
		///		that is empty and has the default initial capacity.
		/// </summary>
		protected ComplexItemListBase(IList<TItem> list)
			: base(list)
		{
		}

		/// <summary>
		///     Class deconstructor.
		/// </summary>
		~ComplexItemListBase()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			Dispose(false);
		}

		#endregion
	}
}
