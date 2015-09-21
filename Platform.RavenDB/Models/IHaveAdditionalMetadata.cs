using Raven.Json.Linq;
using System.Collections.Generic;

namespace DDCloud.Platform.RavenDB.Models
{
	/// <summary>
	///		Represents a RavenDB document contract that needs to persist custom metadata.
	/// </summary>
	public interface IHaveAdditionalMetadata
	{
		/// <summary>
		///		Get the custom metadata (if any) to persist with the document.
		/// </summary>
		/// <returns>
		///		A sequence of 0 or more <see cref="KeyValuePair{String,RavenJToken}">key / value pairs</see> representing the metadata to persist.
		/// </returns>
		IEnumerable<KeyValuePair<string, RavenJToken>> GetMetadataToPersist();
	}
}
