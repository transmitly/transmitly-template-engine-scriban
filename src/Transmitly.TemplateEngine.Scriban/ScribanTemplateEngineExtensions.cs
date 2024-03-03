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
using Transmitly.Template.Configuration;
using Transmitly.TemplateEngine.Scriban;

namespace Transmitly
{
	public static class ScribanTemplateEngineExtensions
	{
		private const string ScribanId = "Scriban";


		public static string Scriban(this TemplateEngines templateEngines, string? providerId = null)
		{
			Guard.AgainstNull(templateEngines);

			return templateEngines.GetId(ScribanId, providerId);
		}

		public static CommunicationsClientBuilder AddScribanTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, Action<ScribanOptions> options, string? templateEngineId = null)
		{
			Guard.AgainstNull(templateConfiguration);
			Guard.AgainstNull(options);

			var opts = new ScribanOptions();
			options(opts);
			return templateConfiguration.Add(new ScribanTemplateEngine(opts), Id.TemplateEngines.Scriban(templateEngineId));
		}

		public static CommunicationsClientBuilder AddScribanTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, string? templateEngineId = null)
		{
			return AddScribanTemplateEngine(templateConfiguration, (opts) => { }, templateEngineId);
		}

		public static CommunicationsClientBuilder AddScribanTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder)
		{
			return AddScribanTemplateEngine(communicationsClientBuilder.TemplateEngine, (opts) => { });
		}

		public static CommunicationsClientBuilder AddScribanTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder, Action<ScribanOptions> options)
		{
			return AddScribanTemplateEngine(communicationsClientBuilder.TemplateEngine, options, Id.TemplateEngines.Scriban());
		}
	}
}
