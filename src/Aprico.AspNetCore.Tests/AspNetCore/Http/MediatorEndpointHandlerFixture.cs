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

using System.Threading;
using System.Threading.Tasks;
using Aprico.AutoFixture.Xunit2;
using Aprico.Moq.Extensions;
using AutoFixture.AutoMoq;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace Aprico.AspNetCore.Http;

public class MediatorEndpointHandlerFixture
{
	[Theory]
	[AutoData<AutoMoqCustomization>]
	public async Task DispatchesMessageViaMediatorAndReturnsStatusCreated(IRequest<int> command, IMediator mediator, int result)
	{
		mediator.AsMock()
			.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
			.ReturnsAsync(result);

		var status = await MediatorEndpointHandler.ThatReturnsStatusCreated<IRequest<int>, int>(command, mediator, CancellationToken.None);

		mediator.AsMock()
			.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
		status.Should()
			.BeOfType<Created<int>>();
		status.Should()
			.BeEquivalentTo(TypedResults.Created((string?) null, result));
	}

	[Theory]
	[AutoData<AutoMoqCustomization>]
	public async Task DispatchesMessageViaMediatorAndReturnsStatusNoContent(IRequest command, IMediator mediator)
	{
		var status = await MediatorEndpointHandler.ThatReturnsStatusNoContent(command, mediator, CancellationToken.None);

		mediator.AsMock()
			.Verify(m => m.Send(command, It.IsAny<CancellationToken>()));
		status.Should()
			.BeOfType<NoContent>();
		status.Should()
			.Be(TypedResults.NoContent());
	}
}
