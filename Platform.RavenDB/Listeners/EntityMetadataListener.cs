using Raven.Client.Listeners;
using Raven.Json.Linq;
using System;

namespace DDCloud.Platform.RavenDB.Listeners
{
	using Models;

	/// <summary>
	///		RavenDB document conversion listener that reads / writes entity metadata for <see cref="IEntityWithMetadata">DMS entities</see>.
	/// </summary>
	public class EntityMetadataListener
		: IDocumentConversionListener, IDocumentStoreListener
	{
		/// <summary>
		///		Create a new entity metadata listener.
		/// </summary>
		public EntityMetadataListener()
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
		void IDocumentConversionListener.BeforeConversionToDocument(string key, object entity, RavenJObject metadata)
		{
			SaveEntityMetadata(entity, metadata);
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
		void IDocumentConversionListener.AfterConversionToDocument(string key, object entity, RavenJObject document, RavenJObject metadata)
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
		void IDocumentConversionListener.BeforeConversionToEntity(string key, RavenJObject document, RavenJObject metadata)
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
		void IDocumentConversionListener.AfterConversionToEntity(string key, RavenJObject document, RavenJObject metadata, object entity)
		{
			LoadEntityMetadata(entity, metadata);
		}

		#endregion // IDocumentConversionListener

		#region IDocumentStoreListener

		/// <summary>
		///		Called before a store request is sent to the server.
		/// </summary>
		/// <param name="key">
		///		The document key.
		/// </param>
		/// <param name="entityInstance">
		///		The entity instance.
		/// </param>
		/// <param name="metadata">
		///		The entity metadata.
		/// </param>
		/// <param name="original">
		///		The original document that was loaded from the server.
		/// </param>
		/// <returns>
		///		<c>true</c>, if the entity instance has been modified and needs to be re-serialised; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		///		If only metadata has been modified, you can still return <c>false</c>.
		/// </remarks>
		bool IDocumentStoreListener.BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
		{
			// Nothing to do.

			return false;
		}

		/// <summary>
		///		Called after a store request is sent to the server.
		/// </summary>
		/// <param name="key">
		///		The document key.
		/// </param>
		/// <param name="entityInstance">
		///		The entity instance.
		/// </param>
		/// <param name="metadata">
		///		The document metadata.
		/// </param>
		void IDocumentStoreListener.AfterStore(string key, object entityInstance, RavenJObject metadata)
		{
			LoadEntityMetadata(entityInstance, metadata);
		}

		#endregion // IDocumentStoreListener

		#region Helpers

		/// <summary>
		///		If the entity is an <see cref="IEntityWithMetadata"/>, load its metadata from the metadata JSON document.
		/// </summary>
		/// <param name="entity">
		///		The entity.
		/// </param>
		/// <param name="metadataJson">
		///		The metadata JSON document.
		/// </param>
		protected virtual void LoadEntityMetadata(object entity, RavenJObject metadataJson)
		{
			IEntityWithMetadata<RavenEntityMetadata> entityWithMetadata = entity as IEntityWithMetadata<RavenEntityMetadata>;
			if (entityWithMetadata == null)
				return;

			if (metadataJson == null)
				throw new ArgumentNullException(nameof(metadataJson));

			// Populate Id.
			entityWithMetadata.Metadata.Id = entityWithMetadata.Id;
			entityWithMetadata.Metadata.LoadFrom(metadataJson);
		}

		/// <summary>
		///		If the entity is an <see cref="IEntityWithMetadata"/>, save its metadata to the metadata JSON document.
		/// </summary>
		/// <param name="entity">
		///		The entity.
		/// </param>
		/// <param name="metadataJson">
		///		The metadata JSON document.
		/// </param>
		protected virtual void SaveEntityMetadata(object entity, RavenJObject metadataJson)
		{
			IEntityWithMetadata<RavenEntityMetadata> entityWithMetadata = entity as IEntityWithMetadata<RavenEntityMetadata>;
			if (entityWithMetadata == null)
				return;

			if (metadataJson == null)
				throw new ArgumentNullException(nameof(metadataJson));

			entityWithMetadata.Metadata.SaveTo(metadataJson);
		}

		#endregion // Helpers
	}
}
