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

namespace Castle.MicroKernel.ModelBuilder.Inspectors
{
	using System;

	using Castle.Core.Configuration;
	using Castle.Core.Interceptor;
	using Castle.DynamicProxy;
	using Castle.MicroKernel.Proxy;
	using Castle.MicroKernel.Util;
	using Castle.Core;

	/// <summary>
	/// Inspect the component for <c>InterceptorAttribute</c> and
	/// the configuration for the interceptors node
	/// </summary>
#if (!SILVERLIGHT)
	[Serializable]
#endif
	public class InterceptorInspector : IContributeComponentModelConstruction
	{
		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			CollectFromAttributes(model);
			CollectFromConfiguration(model);
		}

		protected virtual void CollectFromConfiguration(ComponentModel model)
		{
			if (model.Configuration == null) return;

			var interceptors = model.Configuration.Children["interceptors"];
			if (interceptors == null) return;

			foreach (var interceptor in interceptors.Children)
			{
				var value = interceptor.Value;

				if (!ReferenceExpressionUtil.IsReference(value))
				{
					throw new Exception(
						String.Format("The value for the interceptor must be a reference to a component (Currently {0})", value));
				}

				var reference = new InterceptorReference(ReferenceExpressionUtil.ExtractComponentKey(value));

				model.Interceptors.Add(reference);
				model.Dependencies.Add(CreateDependencyModel(reference));
			}

			var options = ProxyUtil.ObtainProxyOptions(model, true);
			CollectSelector(interceptors, options);
			CollectHook(interceptors, options);
		}

		protected virtual void CollectHook(IConfiguration interceptors, ProxyOptions options)
		{
			var hook = interceptors.Attributes["hook"];
			if (hook == null)
			{
				return;
			}

			if (!ReferenceExpressionUtil.IsReference(hook))
			{
				throw new Exception(
					String.Format("The value for the hook must be a reference to a component (Currently {0})", hook));
			}

			var componentKey = ReferenceExpressionUtil.ExtractComponentKey(hook);
			options.Hook = new ComponentReference<IProxyHook>(componentKey);
		}

		protected virtual void CollectSelector(IConfiguration interceptors, ProxyOptions options)
		{
			var selector = interceptors.Attributes["selector"];
			if (selector == null)
			{
				return;
			}

			if (!ReferenceExpressionUtil.IsReference(selector))
			{
				throw new Exception(
					String.Format("The value for the selector must be a reference to a component (Currently {0})", selector));
			}

			var componentKey = ReferenceExpressionUtil.ExtractComponentKey(selector);
			options.Selector = new ComponentReference<IInterceptorSelector>(componentKey);
		}

		protected virtual void CollectFromAttributes(ComponentModel model)
		{
			if (!model.Implementation.IsDefined(typeof(InterceptorAttribute), true))
			{
				return;
			}

			object[] attributes = model.Implementation.GetCustomAttributes(true);

			foreach (object attribute in attributes)
			{
				if (attribute is InterceptorAttribute)
				{
					InterceptorAttribute attr = (attribute as InterceptorAttribute);

					AddInterceptor(
						attr.Interceptor,
						model.Interceptors);

					model.Dependencies.Add(
						CreateDependencyModel(attr.Interceptor));
				}
			}
		}

		protected virtual DependencyModel CreateDependencyModel(InterceptorReference interceptor)
		{
			if (string.IsNullOrEmpty(interceptor.ComponentKey))
			{
				return new DependencyModel(DependencyType.Service, interceptor.ComponentKey, interceptor.ServiceType, false);
			}

			return new DependencyModel(DependencyType.ServiceOverride, interceptor.ComponentKey, interceptor.ServiceType, false);
		}

		protected virtual void AddInterceptor(InterceptorReference interceptorRef, InterceptorReferenceCollection interceptors)
		{
			interceptors.Add(interceptorRef);
		}
	}
}

