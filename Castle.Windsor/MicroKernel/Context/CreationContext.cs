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

namespace Castle.MicroKernel.Context
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;

	using Castle.Core;
	using Castle.MicroKernel.Releasers;
	using Castle.MicroKernel.SubSystems.Conversion;

	/// <summary>
	///   Used during a component request, passed along to the whole process.
	///   This allow some data to be passed along the process, which is used 
	///   to detected cycled dependency graphs and now it's also being used
	///   to provide arguments to components.
	/// </summary>
#if (!SILVERLIGHT)
	[Serializable]
	public class CreationContext : MarshalByRefObject, ISubDependencyResolver
#else
	public class CreationContext : ISubDependencyResolver
#endif
	{
		private readonly ITypeConverter converter;

		/// <summary>
		///   Holds the scoped dependencies being resolved. 
		///   If a dependency appears twice on the same scope, we'd have a cycle.
		/// </summary>
		private readonly DependencyModelCollection dependencies;

		private readonly Type[] genericArguments;
		private readonly IHandler handler;

		/// <summary>
		///   The list of handlers that are used to resolve
		///   the component.
		///   We track that in order to try to avoid attempts to resolve a service
		///   with itself.
		/// </summary>
		private readonly Stack<IHandler> handlerStack = new Stack<IHandler>();

		private readonly IReleasePolicy releasePolicy;

		private readonly Stack<ResolutionContext> resolutionStack = new Stack<ResolutionContext>();
		private IDictionary additionalParameters;
		private IDictionary extendedProperties;

		/// <summary>
		///   Initializes a new instance of the <see cref = "CreationContext" /> class.
		/// </summary>
		/// <param name = "typeToExtractGenericArguments">The type to extract generic arguments.</param>
		/// <param name = "parentContext">The parent context.</param>
		/// <param name = "propagateInlineDependencies">When set to <c>true</c> will clone <paramref name = "parentContext" /> <see cref = "AdditionalParameters" />.</param>
		public CreationContext(Type typeToExtractGenericArguments, CreationContext parentContext,
		                       bool propagateInlineDependencies)
			: this(parentContext.Handler, parentContext.ReleasePolicy, typeToExtractGenericArguments, null, null, parentContext)
		{
			if (parentContext == null)
			{
				throw new ArgumentNullException("parentContext");
			}

			if (parentContext.extendedProperties != null)
			{
				extendedProperties = new Dictionary<object, object>(parentContext.extendedProperties.Count);
				foreach (DictionaryEntry parentProperty in parentContext.extendedProperties)
				{
					extendedProperties.Add(parentProperty.Key, parentProperty.Value);
				}
			}

			if (propagateInlineDependencies && parentContext.HasAdditionalParameters)
			{
				additionalParameters = new Arguments(parentContext.additionalParameters);
			}
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "CreationContext" /> class.
		/// </summary>
		/// <param name = "handler">The handler.</param>
		/// <param name = "releasePolicy">The release policy.</param>
		/// <param name = "typeToExtractGenericArguments">The type to extract generic arguments.</param>
		/// <param name = "additionalArguments">The additional arguments.</param>
		/// <param name = "conversionManager">The conversion manager.</param>
		/// <param name="parent">Parent context</param>
		public CreationContext(IHandler handler, IReleasePolicy releasePolicy,
		                       Type typeToExtractGenericArguments, IDictionary additionalArguments,
							   ITypeConverter conversionManager, CreationContext parent)
		{
			this.handler = handler;
			this.releasePolicy = releasePolicy;
			additionalParameters = EnsureAdditionalArgumentsWriteable(additionalArguments);
			converter = conversionManager;
			dependencies = new DependencyModelCollection();
			genericArguments = ExtractGenericArguments(typeToExtractGenericArguments);

			if (parent != null)
			{
				resolutionStack = parent.resolutionStack;
				dependencies.AddRange(parent.Dependencies);
				foreach (var handlerItem in parent.handlerStack)
				{
					handlerStack.Push(handlerItem);
				}
			}
		}

		/// <summary>
		///   Initializes a new instance of the <see cref = "CreationContext" /> class.
		/// </summary>
		private CreationContext()
		{
			dependencies = new DependencyModelCollection();
			releasePolicy = new NoTrackingReleasePolicy();
		}

		public IDictionary AdditionalParameters
		{
			get
			{
				if (additionalParameters == null)
				{
					additionalParameters = new Arguments();
				}
				return additionalParameters;
			}
		}

		public DependencyModelCollection Dependencies
		{
			get { return dependencies; }
		}

		public Type[] GenericArguments
		{
			get { return genericArguments; }
		}

		public IHandler Handler
		{
			get { return handler; }
		}

		public bool HasAdditionalParameters
		{
			get { return additionalParameters != null && additionalParameters.Count != 0; }
		}

		public IReleasePolicy ReleasePolicy
		{
			get { return releasePolicy; }
		}

		public void AddContextualProperty(object key, object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (extendedProperties == null)
			{
				extendedProperties = new Dictionary<object, object>();
			}
			extendedProperties.Add(key, value);
		}

		public ResolutionContext EnterResolutionContext(IHandler handlerBeingResolved)
		{
			return EnterResolutionContext(handlerBeingResolved, true);
		}

		public ResolutionContext EnterResolutionContext(IHandler handlerBeingResolved, bool createBurden)
		{
			var resolutionContext = new ResolutionContext(this, createBurden ? new Burden() : null);
			handlerStack.Push(handlerBeingResolved);
			if (createBurden)
			{
				resolutionStack.Push(resolutionContext);
			}
			return resolutionContext;
		}

		public object GetContextualProperty(object key)
		{
			if (extendedProperties == null)
			{
				return null;
			}

			return extendedProperties[key];
		}

		/// <summary>
		///   Method used by handlers to test whether they are being resolved in the context.
		/// </summary>
		/// <param name = "handler"></param>
		/// <returns></returns>
		/// <remarks>
		///   This method is provided as part of double dispatch mechanism for use by handlers.
		///   Outside of handlers, call <see cref = "IHandler.IsBeingResolvedInContext" /> instead.
		/// </remarks>
		public bool IsInResolutionContext(IHandler handler)
		{
			return handlerStack.Contains(handler);
		}

		public virtual bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
		                               ComponentModel model, DependencyModel dependency)
		{
			if (additionalParameters == null)
			{
				return false;
			}
			var canResolveByKey = CanResolveByKey(dependency);
			var canResolveByType = CanResolveByType(dependency);
			return canResolveByKey || canResolveByType;
		}

		public virtual object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
		                              ComponentModel model, DependencyModel dependency)
		{
			Debug.Assert(CanResolve(context, contextHandlerResolver, model, dependency),
			             "CanResolve(context, contextHandlerResolver, model, dependency)");

			var inlineArgument = additionalParameters[dependency.DependencyKey];
			var targetType = dependency.TargetItemType;
			if (inlineArgument != null)
			{
				if (converter != null &&
				    !targetType.IsInstanceOfType(inlineArgument) &&
				    dependency.DependencyType == DependencyType.Parameter)
				{
					return converter.PerformConversion(inlineArgument.ToString(), targetType);
				}

				return inlineArgument;
			}

			inlineArgument = additionalParameters[targetType];
			if (inlineArgument != null &&
			    converter != null &&
			    !targetType.IsInstanceOfType(inlineArgument) &&
			    dependency.DependencyType == DependencyType.Parameter)
			{
				return converter.PerformConversion(inlineArgument.ToString(), targetType);
			}

			return inlineArgument;
		}

		private bool CanResolve(DependencyModel dependency, object inlineArgument)
		{
			var type = dependency.TargetItemType;
			if (inlineArgument == null || type == null)
			{
				return false;
			}
			return type.IsInstanceOfType(inlineArgument) ||
			       (converter != null &&
			        dependency.DependencyType == DependencyType.Parameter &&
			        converter.CanHandleType(type));
		}

		private bool CanResolveByKey(DependencyModel dependency)
		{
			if (dependency.DependencyKey == null)
			{
				return false;
			}
			Debug.Assert(additionalParameters != null, "additionalArguments != null");
			return CanResolve(dependency, additionalParameters[dependency.DependencyKey]);
		}

		private bool CanResolveByType(DependencyModel dependency)
		{
			var type = dependency.TargetItemType;
			if (type == null)
			{
				return false;
			}
			Debug.Assert(additionalParameters != null, "additionalArguments != null");
			return CanResolve(dependency, additionalParameters[type]);
		}

		private IDictionary EnsureAdditionalArgumentsWriteable(IDictionary dictionary)
		{
			// NOTE: this is actually here mostly to workaround the fact that ReflectionBasedDictionaryAdapter is read only
			// we could make it writeable instead, but I'm not sure that would make sense.
			// NOTE: As noted in IOC-ISSUE-190 that may lead to issues with custom IDictionary implementations
			// We better just ignore not known implementations and if someone uses one, it's their problem to take that into
			// account when dealing with DynamicParameters
			if (dictionary == null)
			{
				return null;
			}

			if (!(dictionary is ReflectionBasedDictionaryAdapter))
			{
				return dictionary;
			}
			return new Arguments(dictionary);
		}

		private void ExitResolutionContext(Burden burden)
		{
			handlerStack.Pop();

			if (burden == null)
			{
				return;
			}

			resolutionStack.Pop();

			if (resolutionStack.Count != 0)
			{
				resolutionStack.Peek().Burden.AddChild(burden);
			}
		}

		/// <summary>
		///   Creates a new, empty <see cref = "CreationContext" /> instance.
		/// </summary>
		/// <remarks>
		///   A new CreationContext should be created every time, as the contexts keeps some state related to dependency resolution.
		/// </remarks>
		public static CreationContext Empty
		{
			get { return new CreationContext(); }
		}

		private static Type[] ExtractGenericArguments(Type typeToExtractGenericArguments)
		{
			return typeToExtractGenericArguments.GetGenericArguments();
		}

		public class ResolutionContext : IDisposable
		{
			private readonly Burden burden;
			private readonly CreationContext context;

			public ResolutionContext(CreationContext context, Burden burden)
			{
				this.context = context;
				this.burden = burden;
			}

			public Burden Burden
			{
				get { return burden; }
			}

			public void Dispose()
			{
				context.ExitResolutionContext(burden);
			}
		}
	}
}