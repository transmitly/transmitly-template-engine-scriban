# Transmitly.TemplateEngine.Scriban

`Transmitly.TemplateEngine.Scriban` adds [Scriban](https://github.com/scriban/scriban) templating support to the core [Transmitly](https://github.com/transmitly/transmitly) library.

This package is typically used alongside:

- `Transmitly`
- a Transmitly channel provider such as `Transmitly.ChannelProvider.Smtp`, `Transmitly.ChannelProvider.SendGrid`, or `Transmitly.ChannelProvider.Infobip`

## Install

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
				<p>Your order has shipped. <a href="{{trackingUrl}}">Track package {{trackingNumber}}</a>.</p>
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

## Registration

Use either registration style:

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

Important settings:

- `UseLiquidTemplates`: parse Liquid-style templates with Scriban.
- `ThrowIfTemplateError`: controls whether invalid templates throw `ScribanTemplateEngineException` or return `null`.
- `LexerOptions` and `ParserOptions`: pass through to Scriban.
- `MemberRenamer` and `MemberFilterDelegate`: control how the content model is exposed to templates.

## Template Sources

The engine works with the normal Transmitly template registration APIs:

- `AddStringTemplate(...)`
- `AddEmbeddedResourceTemplate(...)`
- `AddTemplateResolver(...)`

## Behavior Notes

- Templates render against `context.ContentModel.Model`.
- By default, invalid templates throw `ScribanTemplateEngineException`.
- A `null` content model is allowed; missing values render as empty output.
- The current Transmitly core supports only one template engine registration per `CommunicationsClientBuilder`.

## Related Packages

- [Transmitly](https://github.com/transmitly/transmitly)
- [Transmitly.TemplateEngine.Fluid](https://github.com/transmitly/transmitly-template-engine-fluid)

---
_Copyright (c) Code Impressions, LLC. This open-source project is sponsored and maintained by Code Impressions and is licensed under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html)._
