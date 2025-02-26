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
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Aprico.AspNetCore.Http;

[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Provided by dependency injection.")]
[SuppressMessage("ReSharper", "MemberCanBeInternal", Justification = "Public API.")]
public static class MediatorEndpointHandler
{
	[SuppressMessage("Design", "CA1054:URI-like parameters should not be strings")]
	public static async Task<IResult> ThatReturnsStatusCreated<T, TR>(T command, IMediator mediator, CancellationToken cancellationToken = default)
		where T : IRequest<TR>
	{
		return TypedResults.Created((string?) null, await mediator.Send(command, cancellationToken));
	}

	public static async Task<IResult> ThatReturnsStatusNoContent<T>(T command, IMediator mediator, CancellationToken cancellationToken = default)
		where T : IRequest
	{
		await mediator.Send(command, cancellationToken);
		return TypedResults.NoContent();
	}
}
