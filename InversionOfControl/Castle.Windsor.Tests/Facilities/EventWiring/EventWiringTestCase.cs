// Copyright 2004-2006 Castle Project - http://www.castleproject.org/
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

namespace Castle.Facilities.EventWiring.Tests
{
	using System;
	using System.IO;
	using Castle.Windsor.Tests;
	using NUnit.Framework;
	
	using Castle.Windsor;
	using Castle.Facilities.EventWiring.Tests.Model;

	[TestFixture]
	public class EventWiringTestCase
	{
		private WindsorContainer _container;

		public EventWiringTestCase()
		{
		}

		[TearDown]
		public void Dispose()
		{
			_container.Dispose();
		}

		[Test]
		public void TriggerStaticEvent()
		{
			CreateContainer("simple.xml");

			SimplePublisher publisher = (SimplePublisher)_container["SimplePublisher"];
			SimpleListener listener = (SimpleListener)_container["SimpleListener2"];

			Assert.IsFalse(listener.Listened);
			Assert.IsNull(listener.Sender);

			publisher.StaticTrigger();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisher, listener.Sender);
		}

		[Test]
		public void TriggerEvent()
		{
			CreateContainer("simple.xml");

			SimplePublisher publisher = (SimplePublisher)_container["SimplePublisher"];
			SimpleListener listener = (SimpleListener)_container["SimpleListener"];

			Assert.IsFalse(listener.Listened);
			Assert.IsNull(listener.Sender);

			publisher.Trigger();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisher, listener.Sender);
		}

		[Test, ExpectedException(typeof(EventWiringException))]
		public void EventNotFound()
		{
			CreateContainer("badconfig.xml");

			SimplePublisher publisher = (SimplePublisher)_container["SimplePublisher"];
		}

		[Test]
		public void MultiEvents()
		{
			CreateContainer("multi.xml");

			MultiListener listener = (MultiListener)_container["MultiListener"];
			MultiPublisher publisher = (MultiPublisher)_container["MultiPublisher"];
			PublisherListener publisherThatListens = (PublisherListener)_container["PublisherListener"];
			SimplePublisher anotherPublisher = (SimplePublisher)_container["SimplePublisher"];

			Assert.IsFalse(listener.Listened);
			Assert.IsNull(listener.Sender);

			publisher.Trigger1();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisher, listener.Sender);

			listener.Reset();

			publisher.Trigger2();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisher, listener.Sender);

			listener.Reset();

			publisherThatListens.Trigger1();

			Assert.IsTrue(listener.Listened);
			Assert.AreSame(publisherThatListens, listener.Sender);


			Assert.IsFalse(publisherThatListens.Listened);
			Assert.IsNull(publisherThatListens.Sender);

			anotherPublisher.Trigger();

			Assert.IsTrue(publisherThatListens.Listened);
			Assert.AreSame(anotherPublisher, publisherThatListens.Sender);
		}

		private void RegisterComponents()
		{
			_container.AddComponent("SimpleListener", typeof(SimpleListener));
			_container.AddComponent("SimpleListener2", typeof(SimpleListener));
			_container.AddComponent("SimplePublisher", typeof(SimplePublisher));
			_container.AddComponent("MultiPublisher", typeof(MultiPublisher));
			_container.AddComponent("MultiListener", typeof(MultiListener));
			_container.AddComponent("PublisherListener", typeof(PublisherListener), typeof(PublisherListener));
			_container.AddComponent("BadConfig", typeof(SimpleListener));
		}

		private void CreateContainer(String config)
		{
			_container = new WindsorContainer(Path.Combine(ConfigHelper.ResolveConfigPath("Facilities/EventWiring/"), config));
	
			_container.AddFacility("eventwiring", new EventWiringFacility());
	
			RegisterComponents();
		}
	}
}
