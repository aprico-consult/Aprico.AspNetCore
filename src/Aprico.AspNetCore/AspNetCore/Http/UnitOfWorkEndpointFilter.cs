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
using Aprico.AspNetCore.Http.Extensions;
using Aprico.Persistence;
using Microsoft.AspNetCore.Http;

namespace Aprico.AspNetCore.Http;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
public class UnitOfWorkEndpointFilter(IUnitOfWorkFactory unitOfWorkFactory) : IEndpointFilter
{
	#region IEndpointFilter Members

	[SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Provided by ASP.NET Core.")]
	public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
	{
		if (!context.HttpContext.Request.IsCommandMethod()) return await next(context);

		using var unitOfWork = UnitOfWorkFactory.Create();
		var result = await next(context);
		await unitOfWork.CommitAsync(CancellationToken.None);
		return result;
	}

	#endregion

	private IUnitOfWorkFactory UnitOfWorkFactory { get; } = unitOfWorkFactory;
}
