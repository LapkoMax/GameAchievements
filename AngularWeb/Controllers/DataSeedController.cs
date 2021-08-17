using AngularWeb.MediatRComands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AngularWeb.Controllers
{
    [Route("api/dataSeed")]
    [ApiController]
    public class DataSeedController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DataSeedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> SeedData([FromQuery]string tableName, [FromQuery]int count, [FromQuery]long gameId = 0)
        {
            var createdCount = await _mediator.Send(new SeedDataCommand { tableName = tableName, count = count, gameId = gameId }, CancellationToken.None);
            return Ok(createdCount);
        }
    }
}
