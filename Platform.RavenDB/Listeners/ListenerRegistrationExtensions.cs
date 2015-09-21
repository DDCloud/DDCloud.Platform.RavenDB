using Raven.Client;
using Raven.Client.Listeners;
using System;

namespace DDCloud.Platform.RavenDB.Listeners
{
	using Models;

	/// <summary>
	///		Extension methods for registering platform RavenDB document store listeners.
	/// </summary>
	public static class ListenerRegistrationExtensions
	{
		/// <summary>
		///		Enable persistence contracts that implement <see cref="IHaveAdditionalMetadata"/> to contribute additional metadata to be persisted with the document.
		/// </summary>
		/// <typeparam name="TDocumentStore">
		///		The RavenDB document store type.
		/// </typeparam>
		/// <param name="documentStore">
		///		The RavenDB document store.
		/// </param>
		/// <returns>
		///		The RavenDB document store (enables method-chaining).
		/// </returns>
		public static TDocumentStore EnableAdditionalMetadata<TDocumentStore>(this TDocumentStore documentStore)
			where TDocumentStore : IDocumentStore
		{
			if (documentStore == null)
				throw new ArgumentNullException(nameof(documentStore));

			documentStore.Listeners.RegisterListener(
				new AdditionalMetadataListener()
			);

			return documentStore;
		}

		/// <summary>
		///		Enable automatic population of <see cref="IEntityWithMetadata{TMetadata}">entity</see> <see cref="IEntityMetadata">metadata</see> by registering the the platform RavenDB entity metadata document store listener.
		/// </summary>
		/// <typeparam name="TDocumentStore">
		///		The RavenDB document store type.
		/// </typeparam>
		/// <param name="documentStore">
		///		The RavenDB document store.
		/// </param>
		/// <returns>
		///		The RavenDB document store (enables method-chaining).
		/// </returns>
		public static TDocumentStore EnableEntityMetadata<TDocumentStore>(this TDocumentStore documentStore)
			where TDocumentStore : IDocumentStore
		{
			if (documentStore == null)
				throw new ArgumentNullException(nameof(documentStore));

			EntityMetadataListener metadataListener = new EntityMetadataListener();
			documentStore.Listeners.RegisterListener(
				(IDocumentStoreListener)metadataListener
			);
			documentStore.Listeners.RegisterListener(
				(IDocumentConversionListener)metadataListener
			);

			return documentStore;
		}

		/// <summary>
		///		Enable automatic population of <see cref="IEntityWithMetadata{TMetadata}">entity</see> <see cref="IEntityMetadata">metadata</see> by registering a specific implementation of the the platform RavenDB entity metadata document store listener.
		/// </summary>
		/// <typeparam name="TMetadataListener">
		///		The metadata listener implementation type.
		/// </typeparam>
		/// <param name="documentStore">
		///		The RavenDB document store.
		/// </param>
		/// <returns>
		///		The RavenDB document store (enables method-chaining).
		/// </returns>
		public static IDocumentStore EnableEntityMetadata<TMetadataListener>(this IDocumentStore documentStore)
			where TMetadataListener : EntityMetadataListener, new()
		{
			if (documentStore == null)
				throw new ArgumentNullException(nameof(documentStore));

			TMetadataListener metadataListener = new TMetadataListener();
			documentStore.Listeners.RegisterListener(
				(IDocumentStoreListener)metadataListener
			);
			documentStore.Listeners.RegisterListener(
				(IDocumentConversionListener)metadataListener
			);

			return documentStore;
		}
	}
}
