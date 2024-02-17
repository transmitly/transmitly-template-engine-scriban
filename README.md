# Transmitly.TemplateEngine.Scriban

A [Transmitly](https://github.com/transmitly/transmitly) template engine that enables rendering templates with the [Scriban](https://github.com/scriban/scriban) template engine.

### Getting started

To use the Scriban template engine, first install the [NuGet package](https://nuget.org/packages/transmitly.templateengine.scriban):

```shell
dotnet add package Transmitly.TemplateEngine.Scriban
```

Then add the channel provider using `AddScribanTemplateEngine()`:

```csharp
using Transmitly;
...
var communicationClient = new CommunicationsClientBuilder()
	.AddScribanTemplateEngine();
```

* * Check out the [Transmitly](https://github.com/transmitly/transmitly) project for more details on what a template engine is and how it can be used to improve how you manage your customer communications.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://github.com/transmitly/transmitly/assets/3877248/524f26c8-f670-4dfa-be78-badda0f48bfb">
  <img alt="an open-source project sponsored by CiLabs of Code Impressions, LLC" src="https://github.com/transmitly/transmitly/assets/3877248/34239edd-234d-4bee-9352-49d781716364" width="350" align="right">
</picture> 

---------------------------------------------------

_Copyright &copy; Code Impressions, LLC - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
