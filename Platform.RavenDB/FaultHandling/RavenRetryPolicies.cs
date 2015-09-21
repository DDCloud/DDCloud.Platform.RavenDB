using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;

namespace DDCloud.Platform.RavenDB.FaultHandling
{
	/// <summary>
	///		Factory methods for creating RavenDB-related retry policies.
	/// </summary>
	public static class RavenRetryPolicies
	{
		/// <summary>
		///		The default retry strategy for transient faults relating to RavenDB concurrency (i.e. out-of-date ETag).
		/// </summary>
		static readonly RetryStrategy DefaultConcurrencyRetryStrategy = new Incremental(
			retryCount: 3,
			initialInterval: TimeSpan.FromMilliseconds(50),
			increment: TimeSpan.FromMilliseconds(50),
			name: "Default Concurrency Retry Strategy" 
		);

		/// <summary>
		///		Create a retry policy for RavenDB concurrency errors.
		/// </summary>
		/// <param name="retryStrategy">
		///		An optional retry strategy to use (defaults to 3 retries, incremental back-off).
		/// </param>
		/// <returns>
		///		The retry policy.
		/// </returns>
		public static RetryPolicy<RavenConcurrencyErrorDetectionStrategy> Concurrency(RetryStrategy retryStrategy = null)
		{
            return new RetryPolicy<RavenConcurrencyErrorDetectionStrategy>(retryStrategy ?? DefaultConcurrencyRetryStrategy);
		}
	}
}
