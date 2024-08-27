//using AgentsMennager.DTO;
//using AgentsMennager.Services;
//using LinkOut.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Formats.Tar;

//namespace AgentsMennager.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    public class loginController : ControllerBase
//    {
//        private readonly AgentService agentService;
//        private readonly TargetServis targetservis;
//        private readonly MissionServices missionservice;
//        private readonly loginService loginService;
//        private readonly JwtService jwtservice;

//        public loginController(AgentService _agentService, TargetServis _targetservis, MissionServices _missionservice, loginService _loginService, JwtService _jwtservice)
//        {
//            agentService = _agentService;
//            targetservis = _targetservis;   
//            missionservice = _missionservice;   
//            loginService = _loginService;
//            jwtservice = _jwtservice;
//        }


//        [HttpPost("login")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<ActionResult<string>> login([FromBody] TokenDTO token)
//        {
//            TokenDTO token = await loginService.
//        }
//    }
//}
