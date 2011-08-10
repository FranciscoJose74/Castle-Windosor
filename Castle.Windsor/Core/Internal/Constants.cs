﻿// Copyright 2004-2011 Castle Project - http://www.castleproject.org/
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

namespace Castle.Core.Internal
{
	public abstract class Constants
	{
		private const string propertyFilters = "castle.property-filters";
		private const string defaultComponentForServiceFilter = "castle.default-component-for-service-filter";
		private const string scopeAccessorType = "castle.scope-accessor-type";
		private const string scopeRootSelector = "castle.scope-root";

		public static object DefaultComponentForServiceFilter
		{
			get { return defaultComponentForServiceFilter; }
		}

		public static object ScopeAccessorType
		{
			get { return scopeAccessorType; }
		}

		public static object ScopeRootSelector
		{
			get { return scopeRootSelector; }
		}

		public static object PropertyFilters
		{
			get { return propertyFilters; }
		}
	}
}