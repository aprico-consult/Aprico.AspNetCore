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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace Aprico.AspNetCore.Http.Extensions;

[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Public API.")]
public static class HttpRequestExtensions
{
	[SuppressMessage("Design", "CA1062:Validate arguments of public methods")]
	public static bool IsCommandMethod(this HttpRequest httpRequest)
	{
		return _commandMethods.Contains(httpRequest.Method);
	}

	private static readonly HashSet<string> _commandMethods = new(
		[HttpMethod.Post.Method, HttpMethod.Put.Method, HttpMethod.Delete.Method, HttpMethod.Patch.Method],
		StringComparer.OrdinalIgnoreCase);
}
