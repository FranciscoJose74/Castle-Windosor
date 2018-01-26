// Copyright 2004-2017 Castle Project - http://www.castleproject.org/
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

namespace Castle.Facilities.AspNet.WebApi
{
	using System;

	public class AspNetWebApiControllerDeactivator : IDisposable
	{
		private readonly Action release;

		public AspNetWebApiControllerDeactivator(Action release)
		{
			this.release = release;
		}

		public void Dispose()
		{
			this.release();
		}
	}
}