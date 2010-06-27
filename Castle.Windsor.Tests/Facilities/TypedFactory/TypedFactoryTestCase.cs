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

namespace Castle.Windsor.Tests.Facilities.TypedFactory
{
	using Castle.Facilities.TypedFactory;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor.Tests.Facilities.TypedFactory.Components;
	using Castle.Windsor.Tests.Facilities.TypedFactory.Factories;

	using NUnit.Framework;

	/// <summary>
	/// Summary description for TypedFactoryTestCase.
	/// </summary>
	[TestFixture]
	public class TypedFactoryTestCase
	{
		[SetUp]
		public void Init()
		{
			_container = new WindsorContainer(new DefaultConfigurationStore());
			_facility = new TypedFactoryFacility();
			_container.AddFacility("typedfactory", _facility);
		}

		[TearDown]
		public void Finish()
		{
			_container.Dispose();
		}

		private IWindsorContainer _container;
		private TypedFactoryFacility _facility;

		[Test]
		public void Factory1()
		{
#pragma warning disable 0618 //call to obsolete method
			_facility.AddTypedFactoryEntry(
				new FactoryEntry(
					"protocolHandlerFactory", typeof(IProtocolHandlerFactory1), "Create", "Release"));
#pragma warning restore
			_container.Register(
				Component.For(typeof(IProtocolHandler)).ImplementedBy(typeof(MirandaProtocolHandler)).Named("miranda"));
			_container.Register(
				Component.For(typeof(IProtocolHandler)).ImplementedBy(typeof(MessengerProtocolHandler)).Named("messenger"));

			var factory =
				(IProtocolHandlerFactory1)_container["protocolHandlerFactory"];

			Assert.IsNotNull(factory);

			var handler = factory.Create();

			Assert.IsNotNull(handler);

			factory.Release(handler);
		}

		[Test]
		public void Factory2()
		{
#pragma warning disable 0618 //call to obsolete method
			_facility.AddTypedFactoryEntry(
				new FactoryEntry(
					"protocolHandlerFactory", typeof(IProtocolHandlerFactory2), "Create", "Release"));
#pragma warning restore
			_container.Register(
				Component.For(typeof(IProtocolHandler)).ImplementedBy(typeof(MirandaProtocolHandler)).Named("miranda"));
			_container.Register(
				Component.For(typeof(IProtocolHandler)).ImplementedBy(typeof(MessengerProtocolHandler)).Named("messenger"));

			var factory =
				(IProtocolHandlerFactory2)_container["protocolHandlerFactory"];

			Assert.IsNotNull(factory);

			var handler = factory.Create("miranda");
			Assert.IsNotNull(handler);
			Assert.IsTrue(handler is MirandaProtocolHandler);
			factory.Release(handler);

			handler = factory.Create("messenger");
			Assert.IsNotNull(handler);
			Assert.IsTrue(handler is MessengerProtocolHandler);
			factory.Release(handler);
		}

		[Test]
		public void Factory3()
		{
#pragma warning disable 0618 //call to obsolete method
			_facility.AddTypedFactoryEntry(
				new FactoryEntry(
					"compFactory", typeof(IComponentFactory1), "Construct", ""));
#pragma warning restore
			_container.Register(Component.For(typeof(IDummyComponent)).ImplementedBy(typeof(Component1)).Named("comp1"));
			_container.Register(Component.For(typeof(IDummyComponent)).ImplementedBy(typeof(Component2)).Named("comp2"));

			var factory =
				(IComponentFactory1)_container["compFactory"];

			Assert.IsNotNull(factory);

			var comp1 = factory.Construct();
			Assert.IsNotNull(comp1);

			var comp2 = factory.Construct();
			Assert.IsNotNull(comp2);
		}

		[Test]
		public void Factory4()
		{
#pragma warning disable 0618 //call to obsolete method
			_facility.AddTypedFactoryEntry(
				new FactoryEntry(
					"compFactory", typeof(IComponentFactory2), "Construct", ""));
#pragma warning restore

			_container.Register(Component.For(typeof(IDummyComponent)).ImplementedBy(typeof(Component1)).Named("comp1"));
			_container.Register(Component.For(typeof(IDummyComponent)).ImplementedBy(typeof(Component2)).Named("comp2"));

			var factory =
				(IComponentFactory2)_container["compFactory"];

			Assert.IsNotNull(factory);

			var comp1 = (IDummyComponent)factory.Construct("comp1");
			Assert.IsTrue(comp1 is Component1);
			Assert.IsNotNull(comp1);

			var comp2 = (IDummyComponent)factory.Construct("comp2");
			Assert.IsTrue(comp2 is Component2);
			Assert.IsNotNull(comp2);
		}
	}
}