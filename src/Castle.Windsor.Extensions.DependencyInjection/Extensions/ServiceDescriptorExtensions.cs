// Copyright 2004-2020 Castle Project - http://www.castleproject.org/
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//	 http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.Windsor.Extensions.DependencyInjection.Extensions
{
	using System.Reflection;

	using Castle.MicroKernel.Registration;

	public static class ServiceDescriptorExtensions
	{
		public static IRegistration CreateWindsorRegistration(this Microsoft.Extensions.DependencyInjection.ServiceDescriptor service)
		{
			if (service.ServiceType.ContainsGenericParameters)
			{
				return RegistrationAdapter.FromOpenGenericServiceDescriptor(service);
			}
			else
			{
				var method = typeof(RegistrationAdapter).GetMethod("FromServiceDescriptor", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(service.ServiceType);
				return method.Invoke(null, new object[] { service }) as IRegistration;
			}
		}
	}
}