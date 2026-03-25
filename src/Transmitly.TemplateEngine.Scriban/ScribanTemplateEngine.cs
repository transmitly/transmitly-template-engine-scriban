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
using System.Threading.Tasks;
using Scriban.Runtime;
using Transmitly.Template.Configuration;
using Transmitly.Util;
using SB = Scriban;

namespace Transmitly.TemplateEngine.Scriban
{
	internal sealed class ScribanTemplateEngine(ScribanOptions options) : ITemplateEngine
	{
		private readonly ScribanOptions _options = Guard.AgainstNull(options);

		public async Task<string?> RenderAsync(IContentTemplateRegistration? registration, IDispatchCommunicationContext context)
		{
			if (registration == null)
				return null;

			var model = context.ContentModel?.Model;

			var source = await registration.GetContentAsync(context);
			var template = Parse(source);
			if (template.HasErrors)
			{
				var messages = string.Join(Environment.NewLine, template.Messages);
				if (_options.ThrowIfTemplateError)
				{
					throw new ScribanTemplateEngineException($"Provided template has errors. Pipeline: '{context.PipelineIntent}'.{Environment.NewLine}{messages}");
				}
				System.Diagnostics.Debug.WriteLine($"{nameof(ScribanTemplateEngine)} {string.Join(";", messages)}");
				return null;
			}

			var renderContext = CreateTemplateContext(model);
			try
			{
				return await template.RenderAsync(renderContext);
			}
			finally
			{
				renderContext.PopGlobal();
			}
		}

		internal SB.Parsing.ParserOptions CreateParserOptions()
		{
			return new SB.Parsing.ParserOptions
			{
				ExpressionDepthLimit = _options.ExpressionDepthLimit ?? SB.Parsing.ParserOptions.Default.ExpressionDepthLimit,
			};
		}

		internal SB.TemplateContext CreateTemplateContext(object? model)
		{
			var scriptObject = new ScriptObject();
			if (model != null)
			{
				scriptObject.Import(model, renamer: _options.MemberRenamer, filter: _options.MemberFilterDelegate);
			}

			var renderContext = _options.UseLiquidTemplates ? new SB.LiquidTemplateContext() : new SB.TemplateContext();
			renderContext.MemberRenamer = _options.MemberRenamer;
			renderContext.MemberFilter = _options.MemberFilterDelegate;
			renderContext.ObjectRecursionLimit = _options.ObjectRecursionLimit;
			renderContext.LimitToString = _options.LimitToString;
			renderContext.PushGlobal(scriptObject);
			return renderContext;
		}

		private SB.Template Parse(string? content)
		{
			var parserOptions = CreateParserOptions();
			if (_options.UseLiquidTemplates)
				return SB.Template.ParseLiquid(content, parserOptions: parserOptions, lexerOptions: _options.LexerOptions);
			return SB.Template.Parse(content, parserOptions: parserOptions, lexerOptions: _options.LexerOptions);
		}

	}
}
