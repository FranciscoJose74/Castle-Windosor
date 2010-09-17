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

namespace Castle.MicroKernel.Tests.SpecializedResolvers
{
	using System;
	using System.Linq;

	using Castle.MicroKernel.Handlers;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor.Tests;
	using Castle.Windsor.Tests.Components;

	using NUnit.Framework;

	[TestFixture]
	public class CollectionResolverTestCase : AbstractContainerTestFixture
	{
		[Test]
		public void Composite_service_can_be_resolved_without_triggering_circular_dependency_detection_fuse()
		{
			Container.Register(AllTypes.FromThisAssembly()
			                   	.BasedOn<IEmptyService>()
			                   	.WithService.Base()
			                   	.ConfigureFor<EmptyServiceComposite>(r => r.Forward<EmptyServiceComposite>()));

			var composite = Container.Resolve<EmptyServiceComposite>();
			Assert.AreEqual(2, composite.Inner.Length);
		}

		[Test]
		public void DependencyOnArrayOfServices_OnConstructor()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<ArrayDepAsConstructor>());

			var component = Container.Resolve<ArrayDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Length);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnArrayOfServices_OnConstructor_empty_allowed_empty_provided()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<ArrayDepAsConstructor>());

			var component = Container.Resolve<ArrayDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.IsEmpty(component.Services);
		}

		[Test]
		public void DependencyOnArrayOfServices_OnConstructor_empty_not_allowed_throws()
		{
			Container.Register(Component.For<ArrayDepAsConstructor>());

			Assert.Throws<HandlerException>(() => Container.Resolve<ArrayDepAsConstructor>());
		}

		[Test]
		public void DependencyOnArrayOfServices_OnProperty()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<ArrayDepAsProperty>());

			var component = Container.Resolve<ArrayDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Length);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnArrayOfServices_OnProperty_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<ArrayDepAsProperty>());

			var component = Container.Resolve<ArrayDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.IsEmpty(component.Services);
		}

		[Test]
		public void DependencyOnArrayOfServices_OnProperty_empty_not_allowed_null()
		{
			Container.Register(Component.For<ArrayDepAsProperty>());

			var component = Container.Resolve<ArrayDepAsProperty>();
			Assert.IsNull(component.Services);
		}

		[Test]
		public void DependencyOnCollectionOfServices_OnConstructor()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<CollectionDepAsConstructor>());

			var component = Container.Resolve<CollectionDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Count);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnCollectionOfServices_OnConstructor_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<CollectionDepAsConstructor>());

			var component = Container.Resolve<CollectionDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(0, component.Services.Count);
		}

		[Test]
		public void DependencyOnCollectionOfServices_OnConstructor_empty_not_allowed_throws()
		{
			Container.Register(Component.For<CollectionDepAsConstructor>());

			Assert.Throws<HandlerException>(() => Container.Resolve<CollectionDepAsConstructor>());
		}

		[Test]
		public void DependencyOnCollectionOfServices_OnProperty()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<CollectionDepAsProperty>());

			var component = Container.Resolve<CollectionDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Count);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnCollectionOfServices_OnProperty_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<CollectionDepAsProperty>());

			var component = Container.Resolve<CollectionDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(0, component.Services.Count);
		}

		[Test]
		public void DependencyOnCollectionOfServices_OnProperty_empty_not_allowed_throws()
		{
			Container.Register(Component.For<CollectionDepAsConstructor>());

			Assert.Throws<HandlerException>(() => Container.Resolve<CollectionDepAsConstructor>());
		}

		[Test]
		public void DependencyOnEnumerableOfServices_OnConstructor()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<EnumerableDepAsProperty>());

			var component = Container.Resolve<EnumerableDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Count());
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnEnumerableOfServices_OnConstructor_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<EnumerableDepAsProperty>());

			var component = Container.Resolve<EnumerableDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.IsFalse(component.Services.Any());
		}

		[Test]
		public void DependencyOnEnumerableOfServices_OnConstructor_empty_not_allowed_throws()
		{
			Container.Register(Component.For<EnumerableDepAsConstructor>());

			Assert.Throws<HandlerException>(() => Container.Resolve<EnumerableDepAsConstructor>());
		}

		[Test]
		public void DependencyOnEnumerableOfServices_OnProperty()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<EnumerableDepAsProperty>());

			var component = Container.Resolve<EnumerableDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Count());
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnEnumerableOfServices_OnProperty_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<EnumerableDepAsProperty>());

			var component = Container.Resolve<EnumerableDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.IsFalse(component.Services.Any());
		}

		[Test]
		public void DependencyOnEnumerableOfServices_OnProperty_empty_not_allowed_null()
		{
			Container.Register(Component.For<EnumerableDepAsProperty>());

			var component = Container.Resolve<EnumerableDepAsProperty>();
			Assert.IsNull(component.Services);
		}

		[Test]
		public void DependencyOnListOfServices_OnConstructor()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<ListDepAsConstructor>());

			var component = Container.Resolve<ListDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Count);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnListOfServices_OnConstructor_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<ListDepAsConstructor>());

			var component = Container.Resolve<ListDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(0, component.Services.Count);
		}

		[Test]
		public void DependencyOnListOfServices_OnConstructor_empty_not_allowed_throws()
		{
			Container.Register(Component.For<ListDepAsConstructor>());

			Assert.Throws<HandlerException>(() => Container.Resolve<ListDepAsConstructor>());
		}

		[Test]
		public void DependencyOnListOfServices_OnProperty()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<ListDepAsProperty>());

			var component = Container.Resolve<ListDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Count);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void DependencyOnListOfServices_OnProperty_empty()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel, allowEmptyCollections: true));
			Container.Register(Component.For<ListDepAsProperty>());

			var component = Container.Resolve<ListDepAsProperty>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(0, component.Services.Count);
		}

		[Test]
		public void DependencyOnListOfServices_OnProperty_empty_not_allowed_null()
		{
			Container.Register(Component.For<ListDepAsProperty>());

			var component = Container.Resolve<ListDepAsProperty>();
			Assert.IsNull(component.Services);
		}

		[Test]
		public void DependencyOn_ref_ArrayOfServices_OnConstructor()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<ArrayRefDepAsConstructor>());

			var component = Container.Resolve<ArrayRefDepAsConstructor>();

			Assert.IsNotNull(component.Services);
			Assert.AreEqual(2, component.Services.Length);
			foreach (var service in component.Services)
			{
				Assert.IsNotNull(service);
			}
		}

		[Test]
		public void List_is_readonly()
		{
			Container.Register(Component.For<IEmptyService>().ImplementedBy<EmptyServiceA>(),
			                   Component.For<IEmptyService>().ImplementedBy<EmptyServiceB>(),
			                   Component.For<ListDepAsConstructor>());

			var component = Container.Resolve<ListDepAsConstructor>();

			Assert.IsTrue(component.Services.IsReadOnly);
			Assert.Throws<NotSupportedException>(() => component.Services.Add(new EmptyServiceA()));
		}

		[SetUp]
		public void SetUp()
		{
			Kernel.Resolver.AddSubResolver(new CollectionResolver(Kernel));
		}
	}
}