#region Copyright & License

// Copyright © 2024 - 2025 Aprico Consultants
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Aprico.AspNetCore.Http.Extensions;

public class HttpRequestExtensionsFixture
{
	[Theory]
	[MemberData(nameof(_commandMethods))]
	public void IsCommandMethod(string method)
	{
		var httpRequestMock = new Mock<HttpRequest>();
		httpRequestMock.Setup(static r => r.Method)
			.Returns(method);
		httpRequestMock.Object.IsCommandMethod()
			.Should()
			.BeTrue();
	}

	[Theory]
	[MemberData(nameof(_queryMethods))]
	public void IsNotCommandMethod(string method)
	{
		var httpRequestMock = new Mock<HttpRequest>();
		httpRequestMock.Setup(static r => r.Method)
			.Returns(method);
		httpRequestMock.Object.IsCommandMethod()
			.Should()
			.BeFalse();
	}

	[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Required by Xunit.emberDataAttribute.")]
	public static TheoryData<string> _commandMethods = [HttpMethod.Post.Method, HttpMethod.Put.Method, HttpMethod.Delete.Method, HttpMethod.Patch.Method];

	[SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "Required by Xunit.MemberDataAttribute.")]
	[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
	public static TheoryData<string> _queryMethods = [
		.. typeof(HttpMethod).GetProperties(BindingFlags.Public | BindingFlags.Static)
			.Select(static p => (HttpMethod) p.GetValue(obj: null)!)
			.Select(static m => m.Method)
			.Except(_commandMethods)
	];
}
