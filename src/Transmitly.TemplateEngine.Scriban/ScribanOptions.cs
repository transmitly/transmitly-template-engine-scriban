// ﻿﻿Copyright (c) Code Impressions, LLC. All Rights Reserved.
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

using SB = Scriban;
namespace Transmitly.TemplateEngine.Scriban
{
	public sealed class ScribanOptions
	{
		/// <summary>
		/// Whether to force the use of the Liquid template parser. (Default=false)
		/// </summary>
		public bool UseLiquidTemplates { get; set; }
		public SB.Parsing.LexerOptions? LexerOptions { get; set; }
		public SB.Parsing.ParserOptions ParserOptions { get; set; }
		public SB.Runtime.MemberRenamerDelegate? MemberRenamer { get; set; }
		public SB.Runtime.MemberFilterDelegate? MemberFilterDelegate { get; set; }
	}
}
