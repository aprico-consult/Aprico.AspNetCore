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
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Aprico.Ddd;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Aprico.AspNetCore.Diagnostics;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Public API.")]
public class EndpointExceptionHandler : IExceptionHandler
{
	#region IExceptionHandler Members

	[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Provided by ASP.NET Core.")]
	public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		switch (exception)
		{
			case ValidationException:
				httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
				break;
			case EntityNotFoundException:
				httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
				break;
			case InvalidOperationException:
				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
				break;
			default:
				return ValueTask.FromResult(result: false); // return false to continue with the default behavior
		}

		return ValueTask.FromResult(result: true); // return true to indicate that this exception has been handled
	}

	#endregion
}
