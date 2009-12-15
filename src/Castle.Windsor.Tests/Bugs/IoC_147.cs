// Copyright 2004-2009 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Windsor.Tests.Bugs
{
	using System.Collections;
	using System.Collections.Generic;

	using Castle.MicroKernel.Registration;

	using NUnit.Framework;

	[TestFixture]
	public class IoC_147
	{
		[Test]
		public void ShouldBeAbleToSupplyDictionaryAsARuntimeParameter()
		{
			var container = new WindsorContainer();
			container.Register(Component.For<ClassTakingDictionary>());

			var someDictionary = new Dictionary<string, object>();

			var s = container.Resolve<ClassTakingDictionary>(new { SomeDictionary = someDictionary });
			Assert.IsNotNull(s);
		}


		public class ClassTakingDictionary
		{
			public IDictionary SomeDictionary { get; set; }
		}
	}
}