using Raven.Json.Linq;
using System;

namespace DDCloud.Platform.RavenDB.Models
{
	using Listeners;

	/// <summary>
	///		A strongly-typed wrapper for RavenDB entity document metadata.
	/// </summary>
	public class RavenEntityMetadata
		: IEntityMetadata
	{
		/// <summary>
		///		Create new entity metadata.
		/// </summary>
		public RavenEntityMetadata()
		{
		}

		/// <summary>
		///		The entity Id.
		/// </summary>
		public string Id
		{
			get;
			set;
		}

		/// <summary>
		///		The entity's RavenDB ETag.
		/// </summary>
		/// <remarks>
		///		ETags are a RavenDB construct and can be modified by RavenDB, even if the entity itself has not been modified (e.g. by indexes that use LoadDocument).
		/// 
		///		This value is normally populated by RavenDB and / or the <see cref="EntityMetadataListener"/> and should not be manually modified except in test scenarios.
		/// </remarks>
		public Guid ETag
		{
			get;
			set;
		}

		/// <summary>
		///		The entity update token.
		/// </summary>
		/// <remarks>
		///		Update tokens are an application-specific construct and will never be touched by RavenDB.
		/// </remarks>
		public Guid UpdateToken
		{
			get;
			set;
		}

		/// <summary>
		///		The UTC date / time that the entity was created.
		/// </summary>
		public DateTimeOffset CreatedUtc
		{
			get;
			set;
		}

		/// <summary>
		///		The UTC date / time (if ever) that the entity was last modified.
		/// </summary>
		public DateTimeOffset? LastModifiedUtc
		{
			get;
			set;
		}

		/// <summary>
		///		The Aperture activity Id (if known) associated with the activity that last modified the entity.
		/// </summary>
		public Guid? LastModifiedActivityId
		{
			get;
			set;
		}

		/// <summary>
		///		Touch the entity's metadata, giving it a new <see cref="UpdateToken">update token</see>.
		/// </summary>
		public virtual void Touch()
		{
			UpdateToken = Guid.NewGuid();
		}

		/// <summary>
		///		Load metadata properties from the specified document metadata.
		/// </summary>
		/// <param name="metadataJson">
		///		The JSON representing the document metadata.
		/// </param>
		public virtual void LoadFrom(RavenJObject metadataJson)
		{
			if (metadataJson == null)
				throw new ArgumentNullException(nameof(metadataJson));

			ETag = metadataJson.Value<Guid?>(WellKnownMetadata.ETag) ?? Guid.Empty;
			UpdateToken = metadataJson.Value<Guid?>(WellKnownMetadata.UpdateToken) ?? Guid.Empty;

			CreatedUtc = metadataJson.Value<DateTimeOffset>(WellKnownMetadata.Created);
			LastModifiedUtc = metadataJson.Value<DateTimeOffset?>(WellKnownMetadata.LastModified);
			
			string lastModifiedActivityId = metadataJson.Value<string>(WellKnownMetadata.LastModifiedActivityId);
			LastModifiedActivityId =
				!String.IsNullOrWhiteSpace(lastModifiedActivityId) ?
					Guid.Parse(lastModifiedActivityId)
					:
					(Guid?)null;
		}

		/// <summary>
		///		Save  metadata properties from the specified document metadata.
		/// </summary>
		/// <param name="metadataJson">
		///		The JSON representing the document metadata.
		/// </param>
		public virtual void SaveTo(RavenJObject metadataJson)
		{
			if (metadataJson == null)
				throw new ArgumentNullException(nameof(metadataJson));

			if (UpdateToken != Guid.Empty)
				metadataJson[WellKnownMetadata.UpdateToken] = new RavenJValue(UpdateToken);
			else
				metadataJson.Remove(WellKnownMetadata.UpdateToken);

			if (LastModifiedActivityId.HasValue)
				metadataJson[WellKnownMetadata.LastModifiedActivityId] = new RavenJValue(LastModifiedActivityId);
			else
				metadataJson[WellKnownMetadata.LastModifiedActivityId] = RavenJValue.Null;
		}

		/// <summary>
		///		Load entity metadata from JSON.
		/// </summary>
		/// <param name="metadataJson">
		///		JSON representing the entity metadata.
		/// </param>
		/// <returns>
		///		The metadata.
		/// </returns>
		public static RavenEntityMetadata FromJson(RavenJObject metadataJson)
		{
			if (metadataJson == null)
				throw new ArgumentNullException(nameof(metadataJson));

			return FromJson<RavenEntityMetadata>(metadataJson);
		}

		/// <summary>
		///		Load entity metadata of the specified type from JSON.
		/// </summary>
		/// <typeparam name="TMetadata">
		///		The entity metadata type.
		/// </typeparam>
		/// <param name="metadataJson">
		///		JSON representing the entity metadata.
		/// </param>
		/// <returns>
		///		The metadata.
		/// </returns>
		public static TMetadata FromJson<TMetadata>(RavenJObject metadataJson)
			where TMetadata : RavenEntityMetadata, new()
		{
			if (metadataJson == null)
				throw new ArgumentNullException(nameof(metadataJson));

			TMetadata metadata = new TMetadata();
			metadata.LoadFrom(metadataJson);

			return metadata;
		}
	}
}
