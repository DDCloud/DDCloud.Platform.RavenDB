namespace DDCloud.Platform.RavenDB.Models
{
	/// <summary>
	///		Represents a RavenDB entity.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		///		The entity Id.
		/// </summary>
		string Id
		{
			get;
		}

		/// <summary>
		///		Is the entity marked as deleted?
		/// </summary>
		bool IsDeleted
		{
			get;
			set;
		}
	}
}
