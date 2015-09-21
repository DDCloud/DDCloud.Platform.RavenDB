using Raven.Client.Listeners;
using Raven.Json.Linq;
using System.Collections.Generic;

namespace DDCloud.Platform.RavenDB.Listeners
{
	using Models;

	/// <summary>
	///		RavenDB document conversion listener that persists additional metadata for any document type that implements <see cref="IHaveAdditionalMetadata"/>.
	/// </summary>
	public sealed class AdditionalMetadataListener
		: IDocumentConversionListener
	{
		/// <summary>
		///		Create a new additional-metadata listener.
		/// </summary>
		public AdditionalMetadataListener()
		{
		}

		#region IDocumentConversionListener

		/// <summary>
		///     Called before converting an entity to a document and metadata
		/// </summary>
		/// <param name="key">
		///		The entity key.
		/// </param>
		/// <param name="entity">
		///		The entity.
		/// </param>
		/// <param name="metadata">
		///		The JSON document representing the entity metadata.
		/// </param>
		public void BeforeConversionToDocument(string key, object entity, RavenJObject metadata)
		{
			IHaveAdditionalMetadata withAdditionalMetadata = entity as IHaveAdditionalMetadata;
			if (withAdditionalMetadata == null)
				return;

			if (metadata == null)
				throw new System.ArgumentNullException(nameof(metadata));

			foreach (KeyValuePair<string, RavenJToken> metadataProperty in withAdditionalMetadata.GetMetadataToPersist())
				metadata[metadataProperty.Key] = metadataProperty.Value;
		}

		/// <summary>
		///     Called after having converted an entity to a document and metadata
		/// </summary>
		/// <param name="key">
		///		The entity key.
		/// </param>
		/// <param name="entity">
		///		The entity.
		/// </param>
		/// <param name="document">
		///		The JSON document representing the entity.
		/// </param>
		/// <param name="metadata">
		///		The JSON document representing the entity metadata.
		/// </param>
		public void AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
		{
			// Nothing to do.
		}

		/// <summary>
		///     Called before converting a document and metadata to an entity
		/// </summary>
		/// <param name="key">
		///		The entity key.
		/// </param>
		/// <param name="document">
		///		The JSON document representing the entity.
		/// </param>
		/// <param name="metadata">
		///		The JSON document representing the entity metadata.
		/// </param>
		public void BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata)
		{
			// Nothing to do.
		}

		/// <summary>
		///     Called after having converted a document and metadata to an entity
		/// </summary>
		/// <param name="key">
		///		The entity key.
		/// </param>
		/// <param name="document">
		///		The JSON document representing the entity.
		/// </param>
		/// <param name="metadata">
		///		The JSON document representing the entity metadata.
		/// </param>
		/// <param name="entity">
		///		The entity.
		/// </param>
		public void AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
		{
		}

		#endregion // IDocumentConversionListener
	}
}
