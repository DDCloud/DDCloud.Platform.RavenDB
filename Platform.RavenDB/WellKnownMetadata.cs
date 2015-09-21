namespace DDCloud.Platform.RavenDB
{
	/// <summary>
	///		Well-known RavenDB document metadata names.
	/// </summary>
	public static class WellKnownMetadata
	{
		/// <summary>
		///		The document entity Id.
		/// </summary>
		public static readonly string Id = "@id";

		/// <summary>
		///		The document's RavenDB ETag.
		/// </summary>
		public static readonly string ETag = "@etag";

		/// <summary>
		///		The entity update token.
		/// </summary>
		/// <remarks>
		///		Given that RavenDB tends to update document ETags when a document hasn't actually changed, it's often useful to hand out your own "ETag" which can be manually touched as required (and never by the RavenDB indexing process).
		/// </remarks>
		public static readonly string UpdateToken = "UpdateToken";

		/// <summary>
		///		The UTC date / time that the document was created.
		/// </summary>
		public static readonly string Created = "Created";

		/// <summary>
		///		The UTC date / time that the document was last modified (if ever).
		/// </summary>
		public static readonly string LastModified = "Last-Modified";

		/// <summary>
		///		The activity-correlation Id of the last logical activity that modified the document.
		/// </summary>
		public static readonly string LastModifiedActivityId = "LastModifiedActivityId";

		/// <summary>
		///		The legacy (v1) Id of the document.
		/// </summary>
		public static readonly string LegacyId = "LegacyId";

		/// <summary>
		///		The document entity name.
		/// </summary>
		public static readonly string RavenEntityName = "Raven-Entity-Name";

		/// <summary>
		///		The provisioning status of a provisionable entity.
		/// </summary>
		public static readonly string ProvisioningStatus = "ProvisioningStatus";

		/// <summary>
		///		Is entity deleted?
		/// </summary>
		public static readonly string IsDeleted = "IsDeleted";
	}
}
