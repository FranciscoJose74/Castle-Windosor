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

namespace Castle.MicroKernel.Registration.Proxy
{
	using System.Linq;

	using Castle.Core;
	using Castle.MicroKernel.Proxy;

    public class ProxyMixIns<S> : ComponentDescriptor<S>
        where S : class 
	{
		private readonly MixinRegistration mixIns;

		public ProxyMixIns(MixinRegistration mixIns)
		{
			this.mixIns = mixIns;
		}

		protected internal override void ApplyToModel(IKernel kernel, ComponentModel model)
		{
			if (!mixIns.Any())
			{
				return;
			}
			var options = ProxyUtil.ObtainProxyOptions(model, true);
			foreach (var mixIn in mixIns)
			{
				options.AddMixinReference(mixIn);
			}
		}
	}
}
