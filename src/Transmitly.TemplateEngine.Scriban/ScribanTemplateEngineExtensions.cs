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
using Transmitly.Template.Configuration;
using Transmitly.TemplateEngine.Scriban;
using Transmitly.Util;

namespace Transmitly
{
	/// <summary>
	/// Extensions for registering and referencing the Scriban template engine within a Transmitly configuration.
	/// </summary>
	public static class ScribanTemplateEngineExtensions
	{
		private const string ScribanId = "Scriban";

		/// <summary>
		/// Gets the identifier used to reference the Scriban template engine configurations.
		/// </summary>
		/// <param name="templateEngines">The template engine identifier source.</param>
		/// <param name="providerId">An optional provider-specific suffix used to create a distinct engine identifier.</param>
		/// <returns>The resolved template engine identifier.</returns>
		public static string Scriban(this TemplateEngines templateEngines, string? providerId = null)
		{
			Guard.AgainstNull(templateEngines);

			return templateEngines.GetId(ScribanId, providerId);
		}

		/// <summary>
		/// Registers the Scriban template engine using the supplied configuration callback.
		/// </summary>
		/// <param name="templateConfiguration">The template configuration builder that will receive the engine registration.</param>
		/// <param name="options">A callback that configures <see cref="ScribanOptions"/> before registration.</param>
		/// <param name="templateEngineId">An optional explicit template engine identifier.</param>
		/// <returns>The parent communications client builder.</returns>
		public static CommunicationsClientBuilder AddScribanTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, Action<ScribanOptions> options, string? templateEngineId = null)
		{
			Guard.AgainstNull(templateConfiguration);
			Guard.AgainstNull(options);

			var opts = new ScribanOptions();
			options(opts);
			return templateConfiguration.Add(new ScribanTemplateEngine(opts), Id.TemplateEngines.Scriban(templateEngineId));
		}

		/// <summary>
		/// Registers the Scriban template engine using the default <see cref="ScribanOptions"/>.
		/// </summary>
		/// <param name="templateConfiguration">The template configuration builder that will receive the engine registration.</param>
		/// <param name="templateEngineId">An optional explicit template engine identifier.</param>
		/// <returns>The parent communications client builder.</returns>
		public static CommunicationsClientBuilder AddScribanTemplateEngine(this TemplateConfigurationBuilder templateConfiguration, string? templateEngineId = null)
		{
			return AddScribanTemplateEngine(templateConfiguration, (opts) => { }, templateEngineId);
		}

		/// <summary>
		/// Registers the Scriban template engine on a communications client using the default <see cref="ScribanOptions"/>.
		/// </summary>
		/// <param name="communicationsClientBuilder">The communications client builder to configure.</param>
		/// <returns>The configured communications client builder.</returns>
		public static CommunicationsClientBuilder AddScribanTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder)
		{
			return AddScribanTemplateEngine(communicationsClientBuilder.TemplateEngine, (opts) => { });
		}

		/// <summary>
		/// Registers the Scriban template engine on a communications client using the supplied configuration callback.
		/// </summary>
		/// <param name="communicationsClientBuilder">The communications client builder to configure.</param>
		/// <param name="options">A callback that configures <see cref="ScribanOptions"/> before registration.</param>
		/// <returns>The configured communications client builder.</returns>
		public static CommunicationsClientBuilder AddScribanTemplateEngine(this CommunicationsClientBuilder communicationsClientBuilder, Action<ScribanOptions> options)
		{
			return AddScribanTemplateEngine(communicationsClientBuilder.TemplateEngine, options, Id.TemplateEngines.Scriban());
		}
	}
}
