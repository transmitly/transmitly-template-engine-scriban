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

* Check out the [Transmitly](https://github.com/transmitly/transmitly) project for more details on what a template engine is and how it can be used to improve how you manage your customer communications.

---
_Copyright © Code Impressions, LLC.  This open-source project is sponsored and maintained by Code Impressions
and is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
