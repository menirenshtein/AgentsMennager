using AgentsMennager.DTO;
using AgentsMennager.Models;
using AgentsMennager.Services;
using AgentsMennager.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsMennager.Controllers
{
   
    
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
  
    
    public class agentsController : ControllerBase
    {
     
        
        // Injecting the agent service
        private readonly AgentService agentService;

        public agentsController(AgentService _agentService)
        {
            agentService = _agentService;
        }


        // Creating a new agent
        // The function takes the nickname and photoUrl from the body in the form of DTO and returns a dictionary with the agent's ID.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> creatNewAgent([FromBody] NewAgentDTO req)
        {
            int AgentId = await agentService.CreateAgent(req);
            if (AgentId == null)
            {
                return BadRequest();
            }
            return Ok(new Dictionary<string, int> { ["id"] = AgentId });
        }



        //[HttpPut("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> changeAgentStatus(int id)
        //{
        //    AgentModel agent = await agentService.GetAgentById(id);
        //    AgentStatus status = await agentService.changeAgentStatus(id);
        //    if (agent.status == status)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(status);
        //}



        // Getting an agent by ID
        // The function receives an ID parameter and returns the agent.
        [HttpGet("/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgentModel>> getAgentById(int id)
        {
            return Ok(await agentService.GetAgentById(id));
        }

        
        // Getting all agents
        // The function returns all agents.
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AgentModel>>> getAllAgents()
        {
            return Ok(await agentService.allAgents());
        }

        
        // Changing an agent's pin
        // The function takes an ID parameter and numbers representing the X/Y points, changes the agent's pin, and returns the updated agent.
        [HttpPut("/agents/{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AgentModel>> pin([FromBody] PinDTO pin, int id)
        {
            AgentModel agent = await agentService.pin(pin, id);
            return Ok(agent);
        }


        // Moving an agent
        // The function takes an ID parameter and moves the agent based on their status, returning OK if successful.
        [HttpPut("/agents/{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> move([FromBody] moveDTO move, int id)
        {
            bool pos = await agentService.move(move.direction, id);
            return Ok();
        }
    }
}
