using Raven.Abstractions.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace DDCloud.Platform.RavenDB
{
	/// <summary>
	///		Extension methods for RavenDB exceptions.
	/// </summary>
	public static class RavenExceptionExtensions
	{
		/// <summary>
		///		The regular expression used to extract an entity Id from RavenDB's <see cref="ConcurrencyException"/>
		/// </summary>
		/// <remarks>
		///		It's brain-dead that we have to resort to parsing the message, but RavenDB doesn't store the document Id in a property the exception!
		///		Oh well, at least they don't localise the error message.
		/// </remarks>
		static readonly Regex DocumentIdFromConcurrencyExceptionRegex = new Regex(
			pattern: @".*'(.*)' using a non current etag",
			options: RegexOptions.Compiled
		);

		/// <summary>
		///		Get the Id of the RavenDB document to which the specified <see cref="ConcurrencyException"/> refers.
		/// </summary>
		/// <param name="concurrencyException">
		///		The RavenDB concurrency exception.
		/// </param>
		/// <returns>
		///		The document Id, or <c>null</c> if no document Id could be extracted from the <see cref="ConcurrencyException"/>.
		/// </returns>
		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "This only works for RavenDB ConcurrencyExceptions.")]
		public static string GetDocumentId(this ConcurrencyException concurrencyException)
		{
			if (concurrencyException == null)
				throw new ArgumentNullException(nameof(concurrencyException));

			Match match = DocumentIdFromConcurrencyExceptionRegex.Match(concurrencyException.Message);
			if (!match.Success)
				return null;

			return match.Groups[1].Value;
		}
	}
}
