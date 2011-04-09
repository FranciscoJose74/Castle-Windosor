#region license

// // Copyright 2009-2011 Henrik Feldt - http://logibit.se /
// // 
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// // 
// //     http://www.apache.org/licenses/LICENSE-2.0
// // 
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.

#endregion

using System;
using System.Diagnostics.Contracts;

namespace Castle.Services.vNextTransaction
{
	/// <summary>
	/// 	Static helper class for creating Maybe monads.
	/// </summary>
	public static class Maybe
	{
		/// <summary>
		/// 	Creates a new maybe monad with a value.
		/// </summary>
		/// <typeparam name = "TSome">The type of the value to set the monad to contain.</typeparam>
		/// <param name = "item">The item</param>
		/// <returns>A non-null maybe instance of the maybe monad.</returns>
		[Pure]
		public static Maybe<TSome> Some<TSome>(TSome item)
		{
			Contract.Ensures(Contract.Result<Maybe<TSome>>() != null);
			Contract.EndContractBlock();
			if (!(typeof(TSome).IsValueType || item != null))
				throw new ArgumentException("item must be either a value type or non-null");
			return new Maybe<TSome>(item);
		}

		[Pure]
		public static Maybe<TNone> None<TNone>()
		{
			Contract.Ensures(Contract.Result<Maybe<TNone>>() != null);
			return new Maybe<TNone>();
		}

		/// <summary>
		/// 	Perform an operation f on the maybe, where f might or mightn't return a Maybe.Some{{T}}.
		/// </summary>
		/// <typeparam name = "TSome"></typeparam>
		/// <param name = "maybe"></param>
		/// <param name = "f">A function to continue applying to the monad.</param>
		/// <returns>The same monad</returns>
		[Pure]
		public static Maybe<TSome> Do<TSome>(this Maybe<TSome> maybe, [Pure] Func<TSome, Maybe<TSome>> f)
		{
			Contract.Requires(maybe != null);
			Contract.Requires(f != null);
			Contract.Requires(!maybe.HasValue);
			return maybe.HasValue ? f(maybe.Value) : maybe;
		}

		/// <summary>
		/// 	Perform
		/// </summary>
		/// <typeparam name = "TSome"></typeparam>
		/// <param name = "maybe"></param>
		/// <param name = "f"></param>
		/// <returns></returns>
		[Pure]
		public static Maybe<TSome> Do<TSome>(this Maybe<TSome> maybe, Func<TSome, TSome> f)
		{
			Contract.Requires(maybe != null);
			Contract.Requires(f != null);

			return maybe.HasValue ? Some(f(maybe.Value)) : maybe;
		}

		/// <summary>
		/// </summary>
		/// <typeparam name = "TSome"></typeparam>
		/// <typeparam name = "TOther"></typeparam>
		/// <param name = "maybe"></param>
		/// <param name = "f"></param>
		/// <returns></returns>
		[Pure]
		public static Maybe<TOther> Do<TSome, TOther>(this Maybe<TSome> maybe, Func<TSome, Maybe<TOther>> f)
		{
			return maybe.HasValue ? f(maybe.Value) : None<TOther>();
		}

		/// <summary>
		/// </summary>
		/// <typeparam name = "TSome"></typeparam>
		/// <typeparam name = "TOther"></typeparam>
		/// <param name = "maybe"></param>
		/// <param name = "f"></param>
		/// <returns></returns>
		[Pure]
		public static Maybe<TOther> Do<TSome, TOther>(this Maybe<TSome> maybe, Func<TSome, TOther> f)
		{
			return maybe.HasValue ? Some(f(maybe.Value)) : None<TOther>();
		}

		/// <summary>
		/// 	Returns the maybe or the default value passed as a parameter.
		/// </summary>
		/// <typeparam name = "TSome"></typeparam>
		/// <param name = "maybe"></param>
		/// <param name = "default"></param>
		/// <returns>The unwrapped value of the maybe or otherwise the default.</returns>
		[Pure]
		public static TSome OrDefault<TSome>(this Maybe<TSome> maybe, TSome @default)
		{
			return maybe.HasValue ? maybe.Value : @default;
		}

		/// <summary>
		/// 	The <c>Ambient</c> operator, that selects the second option if the first is unavailable.
		/// 	Because C# doesn't support lazy parameters, it's a func, i.e. a factory method for getting the
		/// 	second option.
		/// </summary>
		/// <typeparam name = "TSome">The type of the maybe</typeparam>
		/// <param name = "firstOption">The first maybe, which is returned if it has a value</param>
		/// <param name = "secondOption">Evaluation if the first option fails.</param>
		/// <returns>Maybe TSome.</returns>
		[Pure]
		public static Maybe<TSome> Amb<TSome>(this Maybe<TSome> firstOption, Func<Maybe<TSome>> secondOption)
		{
			return firstOption.HasValue ? firstOption : secondOption();
		}

		/// <summary>
		/// 	If the maybe doesn't have a value, throws the exception yielded by the func.
		/// </summary>
		/// <typeparam name = "TSome"></typeparam>
		/// <param name = "maybe"></param>
		/// <param name = "ex"></param>
		/// <returns>The item in the maybe.</returns>
		[Pure]
		public static TSome OrThrow<TSome>(this Maybe<TSome> maybe, Func<Exception> ex)
		{
			if (!maybe.HasValue) throw ex();
			return maybe.Value;
		}
	}


	/// <summary>
	/// 	An implementation of the maybe monad.
	/// </summary>
	/// <typeparam name = "T"></typeparam>
	public sealed class Maybe<T>
	{
		private readonly T _Value;
		private readonly bool _HasValue;

		internal Maybe()
		{
			_Value = default(T);
			_HasValue = false;
		}

		internal Maybe(T value)
		{
			_Value = value;
			_HasValue = true;
		}

		/// <summary>
		/// 	Gets whether the maybe has a value.
		/// </summary>
		[Pure]
		public bool HasValue
		{
			get { return _HasValue; }
		}

		/// <summary>
		/// 	Gets the value.
		/// </summary>
		/// <exception cref = "InvalidOperationException">If <see cref = "HasValue" /> is false.</exception>
		public T Value
		{
			get
			{
				if (!HasValue)
					throw new InvalidOperationException("The maybe has no value.");

				return _Value;
			}
		}

		[Pure]
		public static implicit operator Maybe<T>(T item)
		{
			Contract.Ensures(Contract.Result<Maybe<T>>() != null,
			                 "The implicit conversion means that whatever is being implicitly convered, is wrapped in a maybe, no matter what its nullability or defaultness.");

			if (typeof (T).IsValueType)
			{
				return Maybe.Some(item);
			}

// ReSharper disable CompareNonConstrainedGenericWithNull
			return item == null ? Maybe.None<T>() : Maybe.Some(item);
// ReSharper restore CompareNonConstrainedGenericWithNull
		}
	}
}