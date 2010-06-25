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

namespace Castle.MicroKernel.SubSystems.Conversion
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using Castle.Core.Configuration;
	using Castle.Core.Internal;

#if (!SILVERLIGHT)
	[Serializable]
#endif
	public class GenericListConverter : AbstractTypeConverter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericListConverter"/> class.
		/// </summary>
		public GenericListConverter()
		{
		}

		public override bool CanHandleType(Type type)
		{
			if (!type.IsGenericType) return false;

			Type genericDef = type.GetGenericTypeDefinition();

			return (genericDef == typeof(IList<>)
			        || genericDef == typeof(ICollection<>)
			        || genericDef == typeof(List<>)
			        || genericDef == typeof(IEnumerable<>));
		}

		public override object PerformConversion(String value, Type targetType)
		{
			throw new NotImplementedException();
		}

		public override object PerformConversion(IConfiguration configuration, Type targetType)
		{
#if DEBUG
			Debug.Assert(CanHandleType(targetType));
#endif
			Type[] argTypes = targetType.GetGenericArguments();

			if (argTypes.Length != 1)
			{
				throw new ConverterException("Expected type with one generic argument.");
			}

			String itemType = configuration.Attributes["type"];
			Type convertTo = argTypes[0];

			if (itemType != null)
			{
				convertTo = Context.Composition.PerformConversion<Type>(itemType);
			}

			var helperType = typeof(ListHelper<>).MakeGenericType(convertTo);
			var converterHelper = ReflectionUtil.CreateInstance<IGenericCollectionConverterHelper>(helperType, this);
			return converterHelper.ConvertConfigurationToCollection(configuration);
		}

		private class ListHelper<T> : IGenericCollectionConverterHelper
		{
			private readonly GenericListConverter parent;

			public ListHelper(GenericListConverter parent)
			{
				this.parent = parent;
			}

			public object ConvertConfigurationToCollection(IConfiguration configuration)
			{
				var list = new List<T>();
				foreach(var itemConfig in configuration.Children)
				{
					var item = parent.Context.Composition.PerformConversion<T>(itemConfig);
					list.Add(item);
				}

				return list;
			}
		}
	}
}
