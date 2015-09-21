using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDCloud.Platform.RavenDB.FaultHandling
{
	/// <summary>
	///		Extension methods for <see cref="Exception"/>s.
	/// </summary>
	static class ExceptionExtensions
	{
		/// <summary>
		///		Find the first nested exception of the specified type (if one exists).
		/// </summary>
		/// <typeparam name="TException">
		///		The type of exception to find.
		/// </typeparam>
		/// <param name="exception">
		///		The exception whose nested exceptions are to be examined.
		/// </param>
		/// <returns>
		///		The nested exception, or <c>null</c> if no nested exception of the specified type was found.
		/// </returns>
		public static TException FindInnerException<TException>(this Exception exception)
			where TException : Exception
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

			Exception innerException = exception.InnerException;
			while (innerException != null)
			{
				TException targetException = innerException as TException;
				if (targetException != null)
					return targetException;

				// Special case - recurse into aggregate exceptions.
				AggregateException aggregateException = innerException as AggregateException;
				if (aggregateException != null)
				{
					foreach (Exception nestedInnerException in aggregateException.Flatten().InnerExceptions)
					{
						targetException = nestedInnerException as TException ?? nestedInnerException.FindInnerException<TException>();
						if (targetException != null)
							return targetException;
					}
				}

				innerException = innerException.InnerException;
			}

			return null;
		}

		/// <summary>
		///		Safely return a string representation of the error.
		/// </summary>
		/// <param name="exception">
		///		The exception.
		/// </param>
		/// <returns>
		///		<paramref name="exception"/>.<see cref="Exception.ToString"/>, or <paramref name="exception"/>.<see cref="Exception.Message"/> if <paramref name="exception"/>.<see cref="Exception.ToString"/> throws an exception.
		/// </returns>
		public static string SafeToString(this Exception exception)
		{
			if (exception == null)
				return "No exception information was available.";

			string exceptionDetail;
			try
			{
				exceptionDetail = exception.ToString();
			}
			catch (Exception eToString)
			{
				exceptionDetail = exception.Message + " - Error while calling Exception::ToString() " + eToString;
			}

			if (exceptionDetail == exception.Message) // AF: If you do this in your exception class, you are on my shit-list (I'm looking at you, Microsoft Service Bus).
			{
				exceptionDetail = String.Format(
					"{0}: {1}\n{2}",
					exception.GetType().FullName,
					exception.Message,
					exception.StackTrace
				);
			}

			return exceptionDetail;
		}

		/// <summary>
		///		Enumerate the exception's causal chain (i.e. any exceptions that lead to it being raised).
		/// </summary>
		/// <param name="exception">
		///		The exception.
		/// </param>
		/// <param name="flattenAggregateExceptions">
		///		Flatten any <see cref="AggregateException">aggregate exception</see>s?
		///		If <c>true</c>, <see cref="AggregateException"/>s with only a single inner exception will be omitted from the sequence, and only their inner exception will be recursed into.
		/// 
		///		Defaults to <c>true</c>.
		/// </param>
		/// <param name="flattenTypeLoadExceptions">
		///		Flatten <see cref="ReflectionTypeLoadException"/>s?
		///		If <c>true</c>, any <see cref="ReflectionTypeLoadException"/>s encountered will be omitted from the sequence, and only their inner <see cref="ReflectionTypeLoadException.LoaderExceptions"/>s will be yielded.
		/// 
		///		Defaults to <c>true</c>.
		/// </param>
		/// <param name="includeOuterException">
		///		Include the outer-most exception in the results?
		/// 
		///		Defaults to <c>true</c>.
		/// </param>
		/// <returns>
		///		A sequence of 0 or more exceptions.
		/// </returns>
		/// <remarks>
		///		Effectively, recurses into <see cref="Exception"/>.<see cref="Exception.InnerException"/>.
		/// </remarks>
		public static IEnumerable<Exception> CausalChain(this Exception exception, bool flattenAggregateExceptions = true, bool flattenTypeLoadExceptions = true, bool includeOuterException = true)
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

			Exception innerException = exception;
			do
			{
				if (!includeOuterException && ReferenceEquals(innerException, exception))
					continue;

				AggregateException aggregateException = innerException as AggregateException;
				if (aggregateException != null)
				{
					if (flattenAggregateExceptions)
					{
						aggregateException = aggregateException.Flatten();
						if (aggregateException.InnerExceptions.Count == 1)
						{
							yield return aggregateException.InnerExceptions[0];

							continue;
						}
					}

					IEnumerable<Exception> recursiveAggregateInnerExceptions =
						aggregateException.InnerExceptions.SelectMany(
							aggregateInner => aggregateInner.CausalChain(flattenAggregateExceptions, flattenTypeLoadExceptions, includeOuterException)
						);
					foreach (Exception recursiveAggregateInnerException in recursiveAggregateInnerExceptions)
						yield return recursiveAggregateInnerException;

					continue;
				}

				ReflectionTypeLoadException reflectionTypeLoadException = innerException as ReflectionTypeLoadException;
				if (reflectionTypeLoadException != null)
				{
					if (flattenTypeLoadExceptions)
					{
						foreach (Exception typeLoadException in reflectionTypeLoadException.LoaderExceptions)
							yield return typeLoadException;

						continue;
					}
				}

				yield return innerException;
			}
			while ((innerException = innerException.InnerException) != null);
		}
	}
}
