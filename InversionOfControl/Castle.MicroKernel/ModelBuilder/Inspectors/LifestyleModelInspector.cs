// Copyright 2004 DigitalCraftsmen - http://www.digitalcraftsmen.com.br/
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

	using Castle.Model;

	/// <summary>
	/// Summary description for LifestyleModelInspector.
	/// </summary>
	public class LifestyleModelInspector : IContributeComponentModelConstruction
	{
		private static readonly LifestyleModelInspector instance = new LifestyleModelInspector();

		public static LifestyleModelInspector Instance
		{
			get { return instance; }
		}

		#region IContributeComponentModelConstruction Members

		public virtual void ProcessModel(IKernel kernel, ComponentModel model)
		{
			object[] attributes = model.Implementation.GetCustomAttributes( 
				typeof(LifestyleAttribute), true );

			if (attributes.Length != 0)
			{
				LifestyleAttribute attribute = (LifestyleAttribute)
					attributes[0];

				model.LifestyleType = attribute.LifestyleType;

				if (model.LifestyleType == LifestyleType.Custom)
				{
					CustomLifestyleAttribute custom = (CustomLifestyleAttribute)
						attribute;
					model.CustomLifestyle = custom.LifestyleHandlerType;
				}
			}
		}

		#endregion
	}
}
