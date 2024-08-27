using AgentsMennager.DAL;
using AgentsMennager.DTO;
using AgentsMennager.Models;
using AgentsMennager.Utils;
using Microsoft.EntityFrameworkCore;

namespace AgentsMennager.Services
{
    public class AgentService
    {
        private readonly DataLayer db;

        public AgentService(DataLayer _db)
        {
            db = _db;
        }


        // Creating a new agent
        // The function receives a NewAgentDTO, initializes the agent's details, saves it to the database, and returns the created agent's ID.
        public async Task<int> CreateAgent(NewAgentDTO agent)
        {
            AgentModel agentModel = new AgentModel();
            agentModel.location = new PositionModel { x = 1, y = 1 };
            agentModel.photoUrl = agent.photoUrl;
            agentModel.nickname = agent.nickname;
            agentModel.status = Utils.AgentStatus.SleeperAgent;
            db.agents.Add(agentModel);
            db.SaveChanges();
            AgentModel? created = db.agents.FirstOrDefault(a => a.Id == agentModel.Id);
            return created.Id;
        }



        // Retrieving an agent by ID
        // The function receives an ID, finds the agent along with its location, and returns the agent.
        public async Task<AgentModel> GetAgentById(int id)
        {
            return db.agents.Include(a => a.location).FirstOrDefault(a => a.Id == id);
        }



        // Retrieving all agents
        // The function retrieves all agents from the database along with their locations.
        public async Task<List<AgentModel>> allAgents()
        {
            List<AgentModel> agents = db.agents.Include(a => a.location).ToList();
            return agents;
        }



        // Changing an agent's status
        // The function checks the agent's current status and any associated missions, changes the status if necessary, and returns the new status.
        public async Task<AgentStatus> changeAgentStatus(int id)
        {
            MissionModel agentInMission = db.missions.FirstOrDefault(m => m.agent.Id == id && m.status == MissionStatus.Teamed);
            AgentModel agent = db.agents.FirstOrDefault(a => a.Id == id);
            if (agent.status == AgentStatus.SleeperAgent)
            {
                agent.status = AgentStatus.ActiveAgent;
            }
            else
            {
                if (agentInMission == null)
                {
                    agent.status = AgentStatus.SleeperAgent;
                }
            }
            db.SaveChanges();
            return agent.status;
        }



        // Pinning an agent's location
        // The function updates the agent's location coordinates, suggests a mission, and returns the updated agent.
        public async Task<AgentModel> pin(PinDTO pin, int id)
        {
            AgentModel agent = db.agents.Include(a => a.location).FirstOrDefault(a => a.Id == id);

            if (pin.x > 1 && pin.x < 1000 && pin.y > 1 && pin.y < 1000)
            {
                agent.location.x = pin.x;
                agent.location.y = pin.y;
            }
            db.SaveChanges();
            suggAMission(id);
            return agent;
        }



        // Moving an agent
        // The function adjusts the agent's location based on the given direction, checks mission progress, and suggests a new mission if needed.
        public async Task<bool> move(string direction, int id)
        {
            AgentModel? agent = db.agents.Include(a => a.location).FirstOrDefault(a => a.Id == id);
            if (agent.status == AgentStatus.ActiveAgent)
            {
                MissionModel mission = db.missions.FirstOrDefault(m => m.agent.Id == id);
                if (mission.target.location.x > agent.location.x)
                {
                    agent.location.x += 1;
                }
                if (mission.target.location.y > agent.location.y)
                {
                    agent.location.y += 1;
                }
                if (mission.target.location.x < agent.location.x)
                {
                    agent.location.x -= 1;
                }
                if (mission.target.location.y < agent.location.y)
                {
                    agent.location.y -= 1;
                }
                if (mission.agent.location.x == mission.target.location.x && mission.agent.location.y == mission.target.location.y)
                {
                    mission.agent.status = AgentStatus.SleeperAgent;
                    mission.target.Status = TargetStatus.TargetDead;
                    mission.status = MissionStatus.MissionAccomplished;
                }
                db.SaveChanges();
                MissionModel missionOfAgent = db.missions.Include(m => m.agent).Include(m => m.target).FirstOrDefault(m => m.agent.Id == id);
            }

            PositionModel position = agent.location;
            if (agent.location.x > 1 && agent.location.x < 1000 && agent.location.y > 1 && agent.location.y < 1000)
            {
                agent.location.move(position, direction);
                db.SaveChanges();
                return true;
            }
            suggAMission(id);
            return false;
        }



        // Suggesting a mission for an agent
        // The function finds available targets, checks mission criteria, and assigns a mission to the agent if a match is found.
        public async Task<int> suggAMission(int agentId)
        {
            AgentModel agent = db.agents.Find(agentId);
            if (agent.status == AgentStatus.ActiveAgent)
            {
                throw new Exception("Agent is currently active");
            }

            var targets = db.targets
                .Where(t => t.Status == TargetStatus.TargetAlive)
                .Where(t => !db.missions.Any(m =>
                    m.target.Id == t.Id &&
                    (m.status == MissionStatus.Teamed || (m.agent.Id == agentId && m.target.Id == t.Id))))
                .ToList();
            foreach (TargetModel target in targets)
            {
                MissionModel mission = new MissionModel();
                mission.target = target;
                mission.agent = db.agents.Find(agentId);
                double time = (mission.timeToAction - DateTime.Now).TotalHours;
                Console.WriteLine(time);
                if ((time / 5) < 40)
                {
                    db.missions.Add(mission);
                    db.SaveChanges();
                    return mission.Id;
                }
            }
            throw new Exception("No suitable mission found.");
        }
    }
}
