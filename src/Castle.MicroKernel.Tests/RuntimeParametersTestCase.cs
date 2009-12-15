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

namespace Castle.MicroKernel.Tests
{
	using System.Collections.Generic;

	using Castle.MicroKernel.Handlers;
	using Castle.MicroKernel.Tests.RuntimeParameters;

	using NUnit.Framework;

	[TestFixture]
	public class RuntimeParametersTestCase
	{
		private IKernel kernel;
		private Dictionary<string,object> deps;

		[SetUp]
		public void Init()
		{
			kernel = new DefaultKernel();
			kernel.AddComponent("compa", typeof(CompA));
			kernel.AddComponent("compb", typeof(CompB));

			deps = new Dictionary<string, object>();
			deps.Add("cc", new CompC(12));
			deps.Add("myArgument", "ernst");
		}

		[TearDown]
		public void Dispose()
		{
			kernel.Dispose();
		}

		[Test]
		public void WithoutParameters()
		{
			var expectedMessage = @"Can't create component 'compb' as it has dependencies to be satisfied. 
compb is waiting for the following dependencies: 

Services: 
- Castle.MicroKernel.Tests.RuntimeParameters.CompC which was not registered. 

Keys (components with specific keys)
- myArgument which was not registered. 
";
			var exception = Assert.Throws(typeof(HandlerException), () =>
			{
				var compb = kernel[typeof(CompB)];
			});
			Assert.AreEqual(expectedMessage, exception.Message);
		}

		[Test]
		public void WillAlwaysResolveCustomParameterFromServiceComponent()
		{
			kernel.AddComponent("compc", typeof(CompC));
			var c_dependencies = new Dictionary<object, object>();
			c_dependencies["test"] = 15;
			kernel.RegisterCustomDependencies(typeof(CompC), c_dependencies);
			var b_dependencies = new Dictionary<object,object>();
			b_dependencies["myArgument"] = "foo";
			kernel.RegisterCustomDependencies(typeof(CompB), b_dependencies);
			CompB b = kernel["compb"] as CompB;
			Assert.IsNotNull(b);
			Assert.AreEqual(15, b.Compc.test);
		}

		[Test]
		public void ResolveUsingParameters()
		{
			CompB compb = kernel.Resolve(typeof(CompB), deps) as CompB;

			AssertDependencies(compb);
		}

		[Test]
		public void ResolveUsingParametersWithinTheHandler()
		{
			kernel.RegisterCustomDependencies("compb", deps);
			CompB compb = kernel[typeof(CompB)] as CompB;

			AssertDependencies(compb);
		}

		[Test]
		public void ParametersPrecedence()
		{
			kernel.RegisterCustomDependencies("compb", deps);

			CompB instance_with_model = (CompB) kernel[typeof(CompB)];
			Assert.AreSame(deps["cc"], instance_with_model.Compc, "Model dependency should override kernel dependency");

			Dictionary<string, object> deps2 = new Dictionary<string, object>();
			deps2.Add("cc", new CompC(12));
			deps2.Add("myArgument", "ayende");

			CompB instance_with_args = (CompB) kernel.Resolve(typeof(CompB), deps2);

			Assert.AreSame(deps2["cc"], instance_with_args.Compc, "Should get it from resolve params");
			Assert.AreEqual("ayende", instance_with_args.MyArgument);
		}

		[Test]
		public void AddingDependencyToServiceWithCustomDependency()
		{
			DefaultKernel k = new DefaultKernel();
			k.AddComponent("NeedClassWithCustomerDependency",typeof(NeedClassWithCustomerDependency));
			k.AddComponent("HasCustomDependency", typeof(HasCustomDependency));

			Assert.AreEqual(HandlerState.WaitingDependency, k.GetHandler("HasCustomDependency").CurrentState);

			var hash = new Dictionary<object, object>();
			hash["name"] = new CompA();
			k.RegisterCustomDependencies("HasCustomDependency", hash);
			Assert.AreEqual(HandlerState.Valid, k.GetHandler("HasCustomDependency").CurrentState);

			Assert.IsNotNull(k.Resolve(typeof(NeedClassWithCustomerDependency)));
		}

		private void AssertDependencies(CompB compb)
		{
			Assert.IsNotNull(compb, "Component B should have been resolved");

			Assert.IsNotNull(compb.Compc, "CompC property should not be null");
			Assert.IsTrue(compb.MyArgument != string.Empty, "MyArgument property should not be empty");

			Assert.AreSame(deps["cc"], compb.Compc, "CompC property should be the same instnace as in the hashtable argument");
			Assert.IsTrue("ernst".Equals(compb.MyArgument),
			              string.Format("The MyArgument property of compb should be equal to ernst, found {0}", compb.MyArgument));
		}
	}
}