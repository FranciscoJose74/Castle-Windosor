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

namespace Castle.DynamicProxy.Generators
{
	using System;
	using System.Reflection;
	using System.Threading;

	using Castle.DynamicProxy.Generators.Emitters;
	using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

	public class ClassProxyGenerator : BaseProxyGenerator
	{
		public ClassProxyGenerator(ModuleScope scope) : base(scope)
		{
		}

		public Type GenerateCode(Type theClass, Type[] interfaces, ProxyGenerationOptions options)
		{
			ReaderWriterLock rwlock = Scope.RWLock;

			rwlock.AcquireReaderLock(-1);
			
#if DOTNET2
			if (theClass.IsGenericType)
			{
				theClass = theClass.GetGenericTypeDefinition();
			}
#endif

			CacheKey cacheKey = new CacheKey(theClass, interfaces, options);

			Type cacheType = GetFromCache(cacheKey);
			
			if (cacheType != null)
			{
				rwlock.ReleaseReaderLock();

				return cacheType;
			}

			LockCookie lc = rwlock.UpgradeToWriterLock(-1);

			try
			{
				baseType = theClass;

				emitter = BuildClassEmitter(Guid.NewGuid().ToString("N"), theClass, interfaces);

				GenerateFields();
				
				IProxyGenerationHook hook = options.Hook;
				
				GenerateMethods(theClass, SelfReference.Self, hook, options.UseSelector);
				
				// TODO: Add interfaces and mixins
				
				hook.MethodsInspected();

				GenerateConstructor();
				GenerateIProxyTargetAccessor();

//				if (theClass.IsSerializable)
//				{
//					ImplementGetObjectData( interfaces );
//				}
//
//				GenerateInterfaceImplementation(interfaces);
//				GenerateConstructors(theClass);
//
//				if (_delegateToBaseGetObjectData)
//				{
//					GenerateSerializationConstructor();
//				}

				Type type = CreateType();
				
				AddToCache(cacheKey, type);

				return type;
			}
			finally
			{
				rwlock.DowngradeFromWriterLock(ref lc);

				Scope.SaveAssembly();
			}
		}

		protected override Reference GetProxyTargetReference()
		{
			return SelfReference.Self;
		}

		private void GenerateConstructor()
		{
			ArgumentReference cArg0 = new ArgumentReference(typeof(IInterceptor[]));

			ConstructorEmitter constructor = emitter.CreateConstructor(cArg0);

			constructor.CodeBuilder.AddStatement(new AssignStatement(interceptorsField, cArg0.ToExpression()));
			constructor.CodeBuilder.InvokeBaseConstructor();
			constructor.CodeBuilder.AddStatement(new ReturnStatement());
		}
	}
}