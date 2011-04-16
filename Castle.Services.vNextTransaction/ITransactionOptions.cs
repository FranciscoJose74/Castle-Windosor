﻿#region license

// Copyright 2009-2011 Henrik Feldt - http://logibit.se/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.Contracts;
using System.Transactions;

namespace Castle.Services.vNextTransaction
{
	[ContractClass(typeof (ITransactionOptionsContract))]
	public interface ITransactionOptions : IEquatable<ITransactionOptions>
	{
		/// <summary>
		/// 	Gets the transaction isolation level.
		/// </summary>
		IsolationLevel IsolationLevel { [Pure] get; }

		/// <summary>
		/// 	Gets the transaction mode.
		/// </summary>
		TransactionScopeOption Mode { [Pure] get; }

		/// <summary>
		/// 	Gets whether the transaction is read only.
		/// </summary>
		bool ReadOnly { [Pure] get; }

		/// <summary>
		/// 	Gets whether the current transaction's method should forked off.
		/// </summary>
		bool Fork { [Pure] get; }

		/// <summary>
		/// 	Gets the Timeout for this managed transaction. Beware that the timeout 
		/// 	for the transaction option is not the same as your database has specified.
		/// 	Often it's a good idea to let your database handle the transactions
		/// 	timing out and leaving this option to its max value. Your mileage may vary though.
		/// </summary>
		TimeSpan Timeout { [Pure] get; }

		/// <summary>
		/// 	Gets or sets whether the <see cref = "Fork" /> operation should wait for all to complete, or
		/// 	simply return and let the topmost transaction wait instead.
		/// </summary>
		bool WaitAll { [Pure] get; }

		/// <summary>
		/// 	Gets or sets whether the commit should be done asynchronously. Default is false. If you have done a lot of work
		/// 	in the transaction, an asynchronous commit might be preferrable.
		/// </summary>
		bool AsyncCommit { [Pure] get; }
	}
}