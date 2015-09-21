using System;

namespace DDCloud.Platform.RavenDB.Models
{
	/// <summary>
	///		Represents the metadata for an entity stored in RavenDB.
	/// </summary>
	public interface IEntityMetadata
	{
		/// <summary>
		///		The entity Id.
		/// </summary>
		string Id
		{
			get;
		}

		/// <summary>
		///		The entity's RavenDB ETag.
		/// </summary>
		/// <remarks>
		///		ETags are a RavenDB construct and can be modified by RavenDB, even if the entity itself has not been modified (e.g. by index
		/// </remarks>
		Guid ETag
		{
			get;
		}

		/// <summary>
		///		The entity update token.
		/// </summary>
		/// <remarks>
		///		Update tokens are an application-specific construct and will never be touched by RavenDB.
		/// </remarks>
		Guid UpdateToken
		{
			get;
		}

		/// <summary>
		///		The UTC date / time that the entity was created.
		/// </summary>
		DateTimeOffset CreatedUtc
		{
			get;
		}

		/// <summary>
		///		The UTC date / time (if ever) that the entity was last modified.
		/// </summary>
		DateTimeOffset? LastModifiedUtc
		{
			get;
		}

		/// <summary>
		///		The Aperture activity Id (if known) associated with the activity that last modified the entity.
		/// </summary>
		Guid? LastModifiedActivityId
		{
			get;
			set;
		}

		/// <summary>
		///		Touch the entity's metadata, giving it a new <see cref="RavenEntityMetadata.UpdateToken">update token</see>.
		/// </summary>
		void Touch();
	}
}