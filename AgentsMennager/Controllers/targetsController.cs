using AgentsMennager.DTO;
using AgentsMennager.Models;
using AgentsMennager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AgentsMennager.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class targetsController : ControllerBase
    {


        // Injecting the Target service
        private readonly TargetServis targetServis;

        
        public targetsController(TargetServis _ts)
        {
            targetServis = _ts;
        }

        
        // Creating a new target
        // The function receives a NewTargetDTO from the body and returns a dictionary with the target's ID.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> createNewTarget(NewTargetDTO target)
        {
            int targetCreated = await targetServis.createNewTarget(target);
            if (targetCreated != null)
            {
                return Ok(new Dictionary<string, int> { ["id"] = targetCreated });
            }
            return BadRequest();
        }

        
        // Retrieving all targets
        // The function returns a list of all targets.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<TargetModel>>> GetAllTargetges()
        {
            List<TargetModel> allTargets = await targetServis.getAllTargets();
            return Ok(allTargets);
        }

        
        // Retrieving a target by ID
        // The function receives an ID parameter and returns the target.
        [HttpGet("/targets/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TargetModel>> getTargetByID(int id)
        {
            return Ok(await targetServis.getTargetBtId(id));
        }

        
        // Pinning a target
        // The function receives a PinDTO and an ID, updates the target's position, and returns the target.
        [HttpPut("/targets/{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PositionModel>> pin([FromBody] PinDTO tp, int id)
        {
            TargetModel target = await targetServis.pin(tp, id);
            return Ok(target);
        }

        
        // Moving a target
        // The function receives a moveDTO and an ID, updates the target's position based on direction, and returns the new position.
        [HttpPut("/targets/{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PositionModel>> move([FromBody] moveDTO md, int id)
        {
            PositionModel pos = await targetServis.move(id, md.direction);
            return Ok(new Dictionary<string, int> { ["direction"] = pos.x });
        }
    }
}
