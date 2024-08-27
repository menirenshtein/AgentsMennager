using AgentsMennager.DAL;
using AgentsMennager.DTO;
using AgentsMennager.Models;
using AgentsMennager.Services;
using AgentsMennager.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AgentsMennager.Controllers
{
    // Defines the route for the controller and sets it up as an API controller
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class missionsController : ControllerBase
    {
        private readonly MissionServices missionServices;

        // Constructor for the controller, injecting MissionServices
        public missionsController(MissionServices _missionServices)
        {
            missionServices = _missionServices;
        }

        // Endpoint to create a new mission
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MissionModel>> createMission([FromBody] NewMissionDTO misson)
        {
            MissionModel mission = await missionServices.createMission(misson);
            return mission;
        }

        // Endpoint to retrieve all missions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<List<MissionModel>> getAllMisions()
        {
            List<MissionModel> missions = await missionServices.getAllMission();
            return missions;
        }

        // Endpoint to change the status of a mission by ID
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<MissionStatus> changeMissionStstus([FromBody] missionStatusDTO req)
        {
            MissionStatus status1 = await missionServices.ChangeMissionStatus(req);
            return status1;
        }
    }
}
