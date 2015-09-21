namespace DDCloud.Platform.RavenDB.Models
{
	/// <summary>
	///		Represents an entity stored in RavenDB, together with its metadata.
	/// </summary>
	public interface IEntityWithMetadata
		: IEntityWithMetadata<RavenEntityMetadata>
	{
	}

	/// <summary>
	///		Represents an entity stored in RavenDB, together with its metadata.
	/// </summary>
	/// <typeparam name="TMetadata">
	///		The type of metadata associated with the entity.
	/// </typeparam>
	public interface IEntityWithMetadata<out TMetadata>
		: IEntity
		where TMetadata : IEntityMetadata
	{
		/// <summary>
		///		The entity metadata.
		/// </summary>
		TMetadata Metadata
		{
			get;
		}
	}
}
