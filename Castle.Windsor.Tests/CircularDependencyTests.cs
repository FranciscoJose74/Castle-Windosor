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

#if !SILVERLIGHT // we do not support xml config on SL

namespace Castle.Windsor.Tests
{
	using System;

	using Castle.Core;
	using Castle.MicroKernel.Handlers;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor.Tests.Components;
	using NUnit.Framework;

	[TestFixture]
	public class CircularDependencyTests
	{
		[Test]
		public void ShouldNotSetTheViewControllerProperty()
		{
			IWindsorContainer container = new WindsorContainer();
			container.Register(Component.For(typeof(IController)).ImplementedBy(typeof(Controller)).Named("controller"));
			container.Register(Component.For(typeof(IView)).ImplementedBy(typeof(View)).Named("view"));
			var controller = (Controller)container.Resolve("controller");
			Assert.IsNotNull(controller.View);
			Assert.IsNull(controller.View.Controller);
		}

		[Test]
		public void ThrowsACircularDependencyException2()
		{
			IWindsorContainer container = new WindsorContainer();
			container.Register(Component.For(typeof(CompA)).Named("compA"));
			container.Register(Component.For(typeof(CompB)).Named("compB"));
			container.Register(Component.For(typeof(CompC)).Named("compC"));
			container.Register(Component.For(typeof(CompD)).Named("compD"));

			var exception =
				Assert.Throws(typeof(HandlerException), () => container.Resolve("compA"));
			var expectedMessage =
				string.Format(
					"Can't create component 'compA' as it has dependencies to be satisfied. {0}compA is waiting for the following dependencies: {0}{0}Services: {0}- Castle.Windsor.Tests.Components.CompB which was registered but is also waiting for dependencies. {0}{0}compB is waiting for the following dependencies: {0}{0}Services: {0}- Castle.Windsor.Tests.Components.CompC which was registered but is also waiting for dependencies. {0}{0}compC is waiting for the following dependencies: {0}{0}Services: {0}- Castle.Windsor.Tests.Components.CompD which was registered but is also waiting for dependencies. {0}{0}compD is waiting for the following dependencies: {0}{0}Services: {0}- Castle.Windsor.Tests.Components.CompA which was registered but is also waiting for dependencies. {0}",
					Environment.NewLine);
			Assert.AreEqual(expectedMessage, exception.Message);
		}

		[Test]
		public void ShouldNotGetCircularDepencyExceptionWhenResolvingTypeOnItselfWithDifferentModels()
		{
			var container = new WindsorContainer(ConfigHelper.ResolveConfigPath("IOC-51.xml"));
			object o = container["path.fileFinder"];
			Assert.IsNotNull(o);
		}

		[Test]
		public void Should_not_try_to_instantiate_singletons_twice_when_circular_dependency()
		{
			SingletonComponent.CtorCallsCount = 0;
			var container = new WindsorContainer();
			container.Register(Component.For<SingletonComponent>(),
			                   Component.For<SingletonDependency>());

			container.Resolve<SingletonComponent>();

			Assert.AreEqual(1, SingletonComponent.CtorCallsCount);
		}

	}

	[Singleton]
	public class SingletonComponent
	{
		public static int CtorCallsCount;

		public SingletonComponent()
		{
			CtorCallsCount++;
		}

		public SingletonDependency E { get; set; }
	}

	[Singleton]
	public class SingletonDependency
	{
		public SingletonDependency(SingletonComponent c)
		{
		}
	} 

	namespace IOC51
	{
		using System.Reflection;

		public interface IPathProvider
		{
			string Path { get; }
		}

		public class AssemblyPath : IPathProvider
		{
			public string Path
			{
				get
				{
					Uri uriPath = new Uri(Assembly.GetExecutingAssembly().GetName(false).CodeBase);
					return uriPath.LocalPath;
				}
			}
		}

		public class RelativeFilePath : IPathProvider
		{
			public RelativeFilePath(IPathProvider basePathProvider, string extensionsPath)
			{
				_path = System.IO.Path.Combine(basePathProvider.Path + "\\", extensionsPath);
			}

			public string Path
			{
				get { return _path; }
			}

			private string _path;
		}
	}
}

#endif