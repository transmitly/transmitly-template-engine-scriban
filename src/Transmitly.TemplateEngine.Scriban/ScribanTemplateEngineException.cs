// Copyright (c) Code Impressions, LLC. All Rights Reserved.
//  
//  Licensed under the Apache License, Version 2.0 (the "License")
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using System;
#if NETFRAMEWORK
using System.Runtime.Serialization;
#endif

namespace Transmitly.TemplateEngine.Scriban
{
#if NETFRAMEWORK
	[Serializable]
	/// <summary>
	/// Represents an error raised by the Scriban template engine while validating or rendering template content.
	/// </summary>
	public sealed class ScribanTemplateEngineException : Exception
	{
		public ScribanTemplateEngineException(string message) : base(message)
		{

		}

		private ScribanTemplateEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
		{

		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
	}
#else
	/// <summary>
	/// Represents an error raised by the Scriban template engine while validating or rendering template content.
	/// </summary>
	public sealed class ScribanTemplateEngineException(string message) : Exception(message)
	{
	}
#endif
}
