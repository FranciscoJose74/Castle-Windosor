#region license

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

namespace Castle.Transactions.IO.Tests
{
	using System;

	using NUnit.Framework;

	public abstract class TxFTestFixtureBase
	{
		[TestFixtureSetUp]
		public void EnsureSupported()
		{
			if (!VerifySupported())
				return;
		}

		private static bool VerifySupported()
		{
			if (Environment.OSVersion.Version.Major < 6)
				Assert.Ignore("OSVersion.Version.Major < 6 don't support transactional NTFS");

			return true;
		}
	}
}