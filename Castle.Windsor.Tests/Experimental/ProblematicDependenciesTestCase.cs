// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
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

namespace Castle.Windsor.Tests.Experimental
{
	using System.Linq;

	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor.Experimental.Debugging;
	using Castle.Windsor.Experimental.Debugging.Primitives;

	using NUnit.Framework;

	public class ProblematicDependenciesTestCase : AbstractContainerTestFixture
	{
		private DefaultDebuggingSubSystem subSystem;

		[Test]
		public void Can_detect_singleton_depending_on_transient()
		{
			Container.Register(Component.For<B>().LifeStyle.Singleton,
			                   Component.For<A>().LifeStyle.Transient);

			var faultyComponents =
				subSystem.SelectMany(e => e.Attach()).SingleOrDefault(i => i.Name == "Potential Lifestyle Mismatches");
			Assert.IsNotNull(faultyComponents);
			var components = faultyComponents.Value as DebuggerViewItem[];
			Assert.IsNotNull(components);
			Assert.AreEqual(1, components.Length);
		}

		[Test]
		public void Can_detect_singleton_depending_on_transient_indirectory()
		{
			Container.Register(Component.For<C>().LifeStyle.Singleton,
			                   Component.For<B>().LifeStyle.Singleton,
			                   Component.For<A>().LifeStyle.Transient);
			

			var faultyComponents =
				subSystem.SelectMany(e => e.Attach()).SingleOrDefault(i => i.Name == "Potential Lifestyle Mismatches");
			Assert.IsNotNull(faultyComponents);
			var components = faultyComponents.Value as DebuggerViewItem[];
			Assert.IsNotNull(components);
			Assert.AreEqual(2, components.Length);
			var debuggerViewItems = components.Select(i => i.Value).ToArray();
			Assert.AreEqual(2, debuggerViewItems.Length);
		}

		[SetUp]
		public void SetSubSystem()
		{
			subSystem = new DefaultDebuggingSubSystem();
			Kernel.AddSubSystem(SubSystemConstants.DebuggingKey, subSystem);
		}
	}
}