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

using System;
using System.Threading.Tasks;
using Transmitly.Template.Configuration;
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
                if (options.ThrowIfTemplateError)
                {
                    throw new ScribanTemplateEngineException($"Provided template has errors. Pipeline: '{context.PipelineName}'.{Environment.NewLine}{messages}");
                }
                System.Diagnostics.Debug.WriteLine($"{nameof(ScribanTemplateEngine)} {string.Join(";", messages)}");
                return null;
            }
            var result = await template.RenderAsync(model, _options.MemberRenamer, _options.MemberFilterDelegate);
            return result?.ToString();
        }

        private SB.Template Parse(string? content)
        {
            if (_options.UseLiquidTemplates)
                return SB.Template.ParseLiquid(content, parserOptions: _options.ParserOptions, lexerOptions: _options.LexerOptions);
            return SB.Template.Parse(content, parserOptions: _options.ParserOptions, lexerOptions: _options.LexerOptions);
        }

    }
}
