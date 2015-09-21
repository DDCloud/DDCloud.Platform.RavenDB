using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Raven.Abstractions.Exceptions;
using System;
using System.Linq;

namespace DDCloud.Platform.RavenDB.FaultHandling
{
	/// <summary>
	///		Concurrency error detection strategy for RavenDB.
	/// </summary>
	public class RavenConcurrencyErrorDetectionStrategy
		: ITransientErrorDetectionStrategy
	{
		/// <summary>
		///		Determines whether the specified exception represents a transient failure that can be compensated by a retry.
		/// </summary>
		/// <param name="exception">
		///		The exception object to be verified.
		/// </param>
		/// <returns>
		///		True if the specified exception is considered as transient; otherwise, false.
		/// </returns>
		public bool IsTransient(Exception exception)
		{
			if (exception == null)
				return false;

			return exception.CausalChain().OfType<ConcurrencyException>().Any();
		}
	}
}
