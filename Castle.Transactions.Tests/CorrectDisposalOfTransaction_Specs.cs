﻿#region license

// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Transactions;
using Castle.Transactions.Tests.Framework;
using NUnit.Framework;

namespace Castle.Transactions.Tests
{
	public class CorrectDisposalOfTransaction_Specs : TransactionManager_SpecsBase
	{
		[Test]
		public void Dispose_ITransaction_using_IDisposable_should_run_action()
		{
			var actionWasCalled = false;

			var opt = new DefaultTransactionOptions();
			var cmt = new CommittableTransaction();

			using (ITransaction tx = new Transaction(cmt, 1, opt, () => actionWasCalled = true))
				tx.Complete();

			Assert.That(actionWasCalled);
		}
	}
}