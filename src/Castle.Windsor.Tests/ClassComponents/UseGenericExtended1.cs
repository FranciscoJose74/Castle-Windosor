// Copyright 2004-2012 Castle Project - http://www.castleproject.org/
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

namespace CastleTests.ClassComponents
{
	public class UseGenericExtended1
	{
		private readonly IGeneric<string> generic;
		private readonly IGenericExtended<string> genericExtended;

		public UseGenericExtended1(IGeneric<string> generic, IGenericExtended<string> genericExtended)
		{
			this.generic = generic;
			this.genericExtended = genericExtended;
		}

		public IGeneric<string> Generic
		{
			get { return generic; }
		}

		public IGenericExtended<string> GenericExtended
		{
			get { return genericExtended; }
		}
	}
}