﻿// Copyright 2004-2008 Castle Project - http://www.castleproject.org/
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

namespace Castle.Facilities.WcfIntegration
{
	using System.Collections.Generic;
	using Castle.Core;
	using Castle.MicroKernel;

	internal class ExtensionDependencies
	{
		private readonly ComponentModel model;
		private readonly IKernel kernel;

		public ExtensionDependencies(ComponentModel model, IKernel kernel)
		{
			this.model = model;
			this.kernel = kernel;
		}

		public ExtensionDependencies Apply(ICollection<IWcfExtension> extensions)
		{
			foreach (var extension in extensions)
			{
				extension.AddDependencies(kernel, model);
			}
			return this;
		}

		public ExtensionDependencies Apply(params IWcfExtension[] extensions)
		{
			return Apply((ICollection<IWcfExtension>)extensions);
		}
	}
}
