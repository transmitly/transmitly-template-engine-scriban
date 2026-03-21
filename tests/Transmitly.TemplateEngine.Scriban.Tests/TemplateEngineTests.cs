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

using Moq;
using Transmitly.Template.Configuration;
using Transmitly.Util;

namespace Transmitly.TemplateEngine.Scriban.Tests
{
	[TestClass]
	public partial class TemplateEngineTests
	{
		[TestMethod]
		public async Task CanRender()
		{
			var expected = "Hello World!";
			var templateContent = "Hello {{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			//var model = new Mock<IContentModel>();
			var x = typeof(IContentModel).Assembly.GetTypes().Where(t => t.Name == "ContentModel");
			var tm = TransactionModel.Create(new { name = "World" });
			var instance = Guard.AgainstNull((IContentModel?)Activator.CreateInstance(x.First(), tm, Array.Empty<IPlatformIdentityProfile>()));

			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(instance);
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions { });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public async Task CanRenderWithNullModel()
		{
			var expected = "Hello !";
			var templateContent = "Hello {{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			var model = new Mock<IContentModel>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			model.Setup(s => s.Model).Returns(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(model.Object);
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions { });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNotNull(result);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public async Task ShouldThrowWithInvalidTemplateByDefault()
		{
			var templateContent = "Hello {{{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			var model = new Mock<IContentModel>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			model.Setup(s => s.Model).Returns(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(model.Object);
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions { });

			await Assert.ThrowsExactlyAsync<ScribanTemplateEngineException>(() => engine.RenderAsync(template.Object, context.Object));
		}

		[TestMethod]
		public async Task ShouldReturnNullWithInvalidtemplate()
		{
			var templateContent = "Hello {{{name}}!";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			var model = new Mock<IContentModel>();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			model.Setup(s => s.Model).Returns(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(model.Object);
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions { ThrowIfTemplateError = false });

			var result = await engine.RenderAsync(template.Object, context.Object);

			Assert.IsNull(result);
		}

		[TestMethod]
		public void ShouldApplySecureDefaultParserLimit()
		{
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions());

			var parserOptions = engine.CreateParserOptions();

			Assert.AreEqual(ScribanOptions.DefaultExpressionDepthLimit, parserOptions.ExpressionDepthLimit);
		}

		[TestMethod]
		public void ShouldApplySecureDefaultRenderLimits()
		{
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions());
			var renderContext = engine.CreateTemplateContext(model: null);

			try
			{
				Assert.AreEqual(ScribanOptions.DefaultObjectRecursionLimit, renderContext.ObjectRecursionLimit);
				Assert.AreEqual(ScribanOptions.DefaultLimitToString, renderContext.LimitToString);
			}
			finally
			{
				renderContext.PopGlobal();
			}
		}

		[TestMethod]
		public async Task ShouldThrowWhenExpressionDepthLimitExceeded()
		{
			var nestedExpression = new string('(', ScribanOptions.DefaultExpressionDepthLimit + 1) + "1" + new string(')', ScribanOptions.DefaultExpressionDepthLimit + 1);
			var templateContent = $"{{{{ {nestedExpression} }}}}";
			var template = new Mock<IContentTemplateRegistration>();
			template.Setup(s => s.GetContentAsync(It.IsAny<IDispatchCommunicationContext>())).Returns(Task.FromResult<string?>(templateContent));
			var model = new Mock<IContentModel>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
			model.Setup(s => s.Model).Returns(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
			var context = new Mock<IDispatchCommunicationContext>();
			context.Setup(s => s.ContentModel).Returns(model.Object);
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions());

			var exception = await Assert.ThrowsExactlyAsync<ScribanTemplateEngineException>(() => engine.RenderAsync(template.Object, context.Object));

			StringAssert.Contains(exception.Message, "errors");
		}

		[TestMethod]
		public async Task ShouldThrowWhenObjectRecursionLimitExceeded()
		{
			var recursiveObject = new global::Scriban.Runtime.ScriptObject();
			recursiveObject["self"] = recursiveObject;
			var model = new global::Scriban.Runtime.ScriptObject
			{
				["a"] = recursiveObject
			};
			var engine = new Scriban.ScribanTemplateEngine(new ScribanOptions());
			var template = global::Scriban.Template.Parse("{{ a }}");
			var renderContext = engine.CreateTemplateContext(model);

			try
			{
				var exception = await Assert.ThrowsExactlyAsync<global::Scriban.Syntax.ScriptRuntimeException>(async () => await template.RenderAsync(renderContext));

				StringAssert.Contains(exception.Message, "deeply nested");
			}
			finally
			{
				renderContext.PopGlobal();
			}
		}
	}
}
