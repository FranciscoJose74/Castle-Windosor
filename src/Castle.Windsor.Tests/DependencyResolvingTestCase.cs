// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
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
	using Castle.MicroKernel.Registration;
	using Castle.Windsor.Tests;
	using Castle.Windsor.Tests.Components;

	using NUnit.Framework;

	[TestFixture]
	public class DependencyResolvingTestCase : AbstractContainerTestCase
	{
		[Test]
		public void ContainerShouldUseFirstRegisteredDependencyOfTypeByDefault_EmailRegisteredFirst()
		{
			Kernel.Register(Component.For<IAlarmSender>().ImplementedBy<EmailSender>(),
			                Component.For<IAlarmSender>().ImplementedBy<SmsSender>(),
			                Component.For<AlarmGenerator>());

			var gen = Kernel.Resolve<AlarmGenerator>();

			Assert.IsInstanceOf<EmailSender>(gen.Sender);
		}

		[Test]
		public void ContainerShouldUseFirstRegisteredDependencyOfTypeByDefault_SmsRegisteredFirst()
		{
			Kernel.Register(Component.For<IAlarmSender>().ImplementedBy<SmsSender>(),
			                Component.For<IAlarmSender>().ImplementedBy<EmailSender>(),
			                Component.For<AlarmGenerator>());

			var gen = Kernel.Resolve<AlarmGenerator>();

			Assert.IsInstanceOf<SmsSender>(gen.Sender);
		}
	}
}