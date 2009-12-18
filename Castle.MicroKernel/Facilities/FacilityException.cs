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

namespace Castle.MicroKernel.Facilities
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// Base exception to be used by facilities.
	/// </summary>
#if (!SILVERLIGHT)
	[Serializable]
#endif
	public class FacilityException : Exception
	{
		public FacilityException(string message) : base(message)
		{
		}

		public FacilityException(string message, Exception innerException) : base(message, innerException)
		{
		}
		
#if (!SILVERLIGHT)
		public FacilityException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
#endif
	}
}
