# Transmitly.TemplateEngine.Scriban

`Transmitly.TemplateEngine.Scriban` adds [Scriban](https://github.com/scriban/scriban) templating support to the core [Transmitly](https://github.com/transmitly/transmitly) library.

This package is typically used alongside:

- `Transmitly`
- A Transmitly channel provider such as `Transmitly.ChannelProvider.Smtp`, `Transmitly.ChannelProvider.SendGrid`, or `Transmitly.ChannelProvider.Infobip`

## Installation

Install the core library, this template engine, and a channel provider:

```shell
dotnet add package Transmitly
dotnet add package Transmitly.TemplateEngine.Scriban
dotnet add package Transmitly.ChannelProvider.Smtp
```

## Quick Start

```csharp
using Transmitly;
using Transmitly.ChannelProvider.Smtp.Configuration;

ICommunicationsClient client = new CommunicationsClientBuilder()
	.AddSmtpSupport(options =>
	{
		options.Host = "smtp.example.com";
		options.Port = 587;
		options.UserName = "smtp-user";
		options.Password = "smtp-password";
	})
	.AddScribanTemplateEngine()
	.AddPipeline("order-shipped", pipeline =>
	{
		pipeline.AddEmail("orders@my.app".AsIdentityAddress("Orders"), email =>
		{
			email.Subject.AddStringTemplate("Order {{orderNumber}} has shipped");
			email.HtmlBody.AddStringTemplate(
				"""
				<p>Hello {{firstName}},</p>
				{{ if trackingUrl }}
				<p>
					Your order has shipped.
					Track it here:
					<a href="{{trackingUrl}}">{{trackingNumber}}</a>
				</p>
				{{ else }}
				<p>Your order has shipped.</p>
				{{ end }}
				"""
			);
			email.TextBody.AddStringTemplate("Hello {{firstName}}, your order {{orderNumber}} has shipped.");
		});
	})
	.BuildClient();

var result = await client.DispatchAsync(
	"order-shipped",
	"customer@example.com".AsIdentityAddress("Ava Example"),
	new
	{
		firstName = "Ava",
		orderNumber = "A10023",
		trackingNumber = "1Z999",
		trackingUrl = "https://carrier.example/track/1Z999"
	});
```

## Why Scriban

Use this package when you want Scriban syntax and features directly in your Transmitly pipelines. It is a good fit when:

- your team already uses Scriban elsewhere
- you want stricter template error handling by default
- you want the option to parse Liquid templates with Scriban

## Registering The Engine

Use either of these registration styles:

```csharp
new CommunicationsClientBuilder()
	.AddScribanTemplateEngine();
```

```csharp
new CommunicationsClientBuilder()
	.TemplateEngine.AddScribanTemplateEngine();
```

## Options

`AddScribanTemplateEngine(options => ...)` accepts `ScribanOptions`.

| Option | Default | Description |
| --- | --- | --- |
| `UseLiquidTemplates` | `false` | Parse templates using Scriban's Liquid parser instead of native Scriban syntax. |
| `ThrowIfTemplateError` | `true` | Throw `ScribanTemplateEngineException` when the template has parser errors. |
| `LexerOptions` | `null` | Optional Scriban lexer configuration. |
| `ParserOptions` | Scriban default | Optional Scriban parser configuration. |
| `MemberRenamer` | `null` | Optional member renaming strategy when exposing the content model to Scriban. |
| `MemberFilterDelegate` | `null` | Optional member filter to restrict what is accessible to templates. |

Example:

```csharp
using Transmitly.TemplateEngine.Scriban;

new CommunicationsClientBuilder()
	.AddScribanTemplateEngine(options =>
	{
		options.ThrowIfTemplateError = false;
		options.UseLiquidTemplates = true;
	});
```

## Error Handling

By default, invalid Scriban templates throw `ScribanTemplateEngineException`.

If you prefer fail-soft behavior, set:

```csharp
options.ThrowIfTemplateError = false;
```

With that setting, invalid templates return `null`.

## Template Sources

The engine works with the normal Transmitly template registration APIs, including:

- `AddStringTemplate(...)`
- `AddEmbeddedResourceTemplate(...)`
- `AddTemplateResolver(...)`

## Behavior Notes

- Templates render against `context.ContentModel.Model`.
- A `null` content model is allowed; missing values render as empty output.
- The current Transmitly core supports only one template engine registration per `CommunicationsClientBuilder`.

## Related Packages

- [Transmitly](https://github.com/transmitly/transmitly)
- [Transmitly.TemplateEngine.Fluid](https://github.com/transmitly/transmitly-template-engine-fluid)
- [Transmitly.ChannelProvider.Smtp](https://github.com/transmitly/transmitly-channel-provider-smtp)

---
_Copyright (c) Code Impressions, LLC. This open-source project is sponsored and maintained by Code Impressions and is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
