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

namespace Castle.Windsor
{
	using System;
	using System.Collections;

	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;

	/// <summary>
	///   The <c>IWindsorContainer</c> interface exposes all the 
	///   functionality the Windsor implements.
	/// </summary>
	public partial interface IWindsorContainer : IDisposable
	{
		/// <summary>
		///   Returns the inner instance of the MicroKernel
		/// </summary>
		IKernel Kernel { get; }

		/// <summary>
		///   Gets the container's name
		/// </summary>
		/// <remarks>
		///   Only useful when child containers are being used
		/// </remarks>
		/// <value>The container's name.</value>
		string Name { get; }

		/// <summary>
		///   Gets or sets the parent container if this instance
		///   is a sub container.
		/// </summary>
		IWindsorContainer Parent { get; set; }

		/// <summary>
		///   Registers a subcontainer. The components exposed
		///   by this container will be accessible from subcontainers.
		/// </summary>
		/// <param name = "childContainer"></param>
		void AddChildContainer(IWindsorContainer childContainer);

		/// <summary>
		///   Registers a facility within the container.
		/// </summary>
		/// <param name = "facility">The <see cref = "IFacility" /> to add to the container.</param>
		IWindsorContainer AddFacility(IFacility facility);

		/// <summary>
		///   Creates and adds an <see cref = "IFacility" /> facility to the container.
		/// </summary>
		/// <typeparam name = "TFacility">The facility type.</typeparam>
		/// <returns></returns>
		IWindsorContainer AddFacility<TFacility>() where TFacility : IFacility, new();

		/// <summary>
		///   Creates and adds an <see cref = "IFacility" /> facility to the container.
		/// </summary>
		/// <typeparam name = "TFacility">The facility type.</typeparam>
		/// <param name = "onCreate">The callback for creation.</param>
		/// <returns></returns>
		IWindsorContainer AddFacility<TFacility>(Action<TFacility> onCreate)
			where TFacility : IFacility, new();

		/// <summary>
		///   Gets a child container instance by name.
		/// </summary>
		/// <param name = "name">The container's name.</param>
		/// <returns>The child container instance or null</returns>
		IWindsorContainer GetChildContainer(string name);

		/// <summary>
		///   Installs the components provided by the <see cref = "IWindsorInstaller" />s
		///   with the <see cref = "IWindsorContainer" />.
		///   <param name = "installers">The component installers.</param>
		///   <returns>The container.</returns>
		/// </summary>
		IWindsorContainer Install(params IWindsorInstaller[] installers);

		/// <summary>
		///   Registers the components provided by the <see cref = "IRegistration" />s
		///   with the <see cref = "IWindsorContainer" />.
		///   <para />
		///   Create a new registration using <see cref = "MicroKernel.Registration.Component" />.For() or <see cref = "AllTypes" />.
		/// </summary>
		/// <example>
		///   <code>
		///     container.Register(Component.For&lt;IService&gt;().ImplementedBy&lt;DefaultService&gt;());
		///   </code>
		/// </example>
		/// <param name = "registrations">The component registrations.</param>
		/// <returns>The container.</returns>
		IWindsorContainer Register(params IRegistration[] registrations);

		/// <summary>
		///   Releases a component instance
		/// </summary>
		/// <param name = "instance"></param>
		void Release(object instance);

		/// <summary>
		///   Remove a child container
		/// </summary>
		/// <param name = "childContainer"></param>
		void RemoveChildContainer(IWindsorContainer childContainer);

		/// <summary>
		///   Returns a component instance by the key
		/// </summary>
		/// <param name = "key"></param>
		/// <param name = "service"></param>
		/// <returns></returns>
		object Resolve(String key, Type service);

		/// <summary>
		///   Returns a component instance by the service
		/// </summary>
		/// <param name = "service"></param>
		/// <returns></returns>
		object Resolve(Type service);

		/// <summary>
		///   Returns a component instance by the service
		/// </summary>
		/// <param name = "service"></param>
		/// <param name = "arguments"></param>
		/// <returns></returns>
		object Resolve(Type service, IDictionary arguments);

		/// <summary>
		///   Returns a component instance by the service
		/// </summary>
		/// <param name = "service"></param>
		/// <param name = "argumentsAsAnonymousType"></param>
		/// <returns></returns>
		object Resolve(Type service, object argumentsAsAnonymousType);

		/// <summary>
		///   Returns a component instance by the service
		/// </summary>
		/// <typeparam name = "T">Service type</typeparam>
		/// <returns>The component instance</returns>
		T Resolve<T>();

		/// <summary>
		///   Returns a component instance by the service
		/// </summary>
		/// <typeparam name = "T">Service type</typeparam>
		/// <param name = "arguments"></param>
		/// <returns>The component instance</returns>
		T Resolve<T>(IDictionary arguments);

		/// <summary>
		///   Returns a component instance by the service
		/// </summary>
		/// <typeparam name = "T">Service type</typeparam>
		/// <param name = "argumentsAsAnonymousType"></param>
		/// <returns>The component instance</returns>
		T Resolve<T>(object argumentsAsAnonymousType);

		/// <summary>
		///   Returns a component instance by the key
		/// </summary>
		/// <param name = "key">Component's key</param>
		/// <typeparam name = "T">Service type</typeparam>
		/// <returns>The Component instance</returns>
		T Resolve<T>(String key);

		/// <summary>
		///   Returns a component instance by the key
		/// </summary>
		/// <typeparam name = "T">Service type</typeparam>
		/// <param name = "key">Component's key</param>
		/// <param name = "arguments"></param>
		/// <returns>The Component instance</returns>
		T Resolve<T>(String key, IDictionary arguments);

		/// <summary>
		///   Returns a component instance by the key
		/// </summary>
		/// <typeparam name = "T">Service type</typeparam>
		/// <param name = "key">Component's key</param>
		/// <param name = "argumentsAsAnonymousType"></param>
		/// <returns>The Component instance</returns>
		T Resolve<T>(String key, object argumentsAsAnonymousType);

		/// <summary>
		///   Returns a component instance by the key
		/// </summary>
		/// <param name = "key"></param>
		/// <param name = "service"></param>
		/// <param name = "arguments"></param>
		/// <returns></returns>
		object Resolve(String key, Type service, IDictionary arguments);

		/// <summary>
		///   Returns a component instance by the key
		/// </summary>
		/// <param name = "key"></param>
		/// <param name = "service"></param>
		/// <param name = "argumentsAsAnonymousType"></param>
		/// <returns></returns>
		object Resolve(String key, Type service, object argumentsAsAnonymousType);

		/// <summary>
		///   Resolve all valid components that match this type.
		/// </summary>
		/// <typeparam name = "T">The service type</typeparam>
		T[] ResolveAll<T>();

		/// <summary>
		///   Resolve all valid components that match this service
		///   <param name = "service">the service to match</param>
		/// </summary>
		Array ResolveAll(Type service);

		/// <summary>
		///   Resolve all valid components that match this service
		///   <param name = "service">the service to match</param>
		///   <param name = "arguments">Arguments to resolve the service</param>
		/// </summary>
		Array ResolveAll(Type service, IDictionary arguments);

		/// <summary>
		///   Resolve all valid components that match this service
		///   <param name = "service">the service to match</param>
		///   <param name = "argumentsAsAnonymousType">Arguments to resolve the service</param>
		/// </summary>
		Array ResolveAll(Type service, object argumentsAsAnonymousType);

		/// <summary>
		///   Resolve all valid components that match this type.
		///   <typeparam name = "T">The service type</typeparam>
		///   <param name = "arguments">Arguments to resolve the service</param>
		/// </summary>
		T[] ResolveAll<T>(IDictionary arguments);

		/// <summary>
		///   Resolve all valid components that match this type.
		///   <typeparam name = "T">The service type</typeparam>
		///   <param name = "argumentsAsAnonymousType">Arguments to resolve the service</param>
		/// </summary>
		T[] ResolveAll<T>(object argumentsAsAnonymousType);
	}
}