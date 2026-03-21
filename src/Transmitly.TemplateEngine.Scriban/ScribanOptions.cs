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

using SB = Scriban;
namespace Transmitly.TemplateEngine.Scriban
{
	/// <summary>
	/// Configures how Scriban templates are parsed and rendered by the Transmitly template engine.
	/// </summary>
	public sealed class ScribanOptions
	{
		public const int DefaultExpressionDepthLimit = 250;
		public const int DefaultObjectRecursionLimit = 20;
		public const int DefaultLimitToString = 1024 * 1024;

		/// <summary>
		/// Gets or sets a value indicating whether templates should be parsed using Scriban's Liquid-compatible parser.
		/// </summary>
		public bool UseLiquidTemplates { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether parse errors should throw a <see cref="ScribanTemplateEngineException"/> instead of returning <see langword="null"/>.
		/// </summary>
		public bool ThrowIfTemplateError { get; set; } = true;

		/// <summary>
		/// Gets or sets the maximum nested expression depth allowed during parsing.
		/// Set to <see langword="null"/> to disable the limit.
		/// </summary>
		public int? ExpressionDepthLimit { get; set; } = DefaultExpressionDepthLimit;

		/// <summary>
		/// Gets or sets the maximum recursion depth allowed when Scriban converts rendered objects to strings.
		/// Set to <c>0</c> to disable the limit.
		/// </summary>
		public int ObjectRecursionLimit { get; set; } = DefaultObjectRecursionLimit;

		/// <summary>
		/// Gets or sets the maximum number of characters produced while Scriban converts rendered objects to strings.
		/// Set to <c>0</c> to disable the limit.
		/// </summary>
		public int LimitToString { get; set; } = DefaultLimitToString;

		/// <summary>
		/// Gets or sets the lexer options used when tokenizing template content.
		/// When <see langword="null"/>, Scriban uses its default lexer behavior.
		/// </summary>
		public SB.Parsing.LexerOptions? LexerOptions { get; set; }

		/// <summary>
		/// Gets or sets the base parser options used when parsing templates.
		/// The engine still applies <see cref="ExpressionDepthLimit"/> when creating the effective parser options.
		/// </summary>
		public SB.Parsing.ParserOptions ParserOptions { get; set; } = new();

		/// <summary>
		/// Gets or sets the delegate used to rename imported .NET members before they are exposed to templates.
		/// </summary>
		public SB.Runtime.MemberRenamerDelegate? MemberRenamer { get; set; }

		/// <summary>
		/// Gets or sets the delegate used to filter which imported .NET members are exposed to templates.
		/// </summary>
		public SB.Runtime.MemberFilterDelegate? MemberFilterDelegate { get; set; }
	}
}
