using AgentsMennager.DAL;
using AgentsMennager.DTO;
using AgentsMennager.Models;
using AgentsMennager.Utils;
using Microsoft.EntityFrameworkCore;

namespace AgentsMennager.Services
{
    public class MissionServices
    {
        private readonly DataLayer db;
        private readonly AgentService agentService;

        public MissionServices(DataLayer _db, AgentService _agentService)
        {
            db = _db;
            agentService = _agentService;
        }



        // Creating a new mission
        // The function receives a NewMissionDTO, initializes the mission details, saves it to the database, and returns the created mission.
        public async Task<MissionModel> createMission(NewMissionDTO mission)
        {
            MissionModel Newmission = new MissionModel();
            Newmission.target = db.targets.Include(t => t.location).FirstOrDefault(t => t.Id == mission.targetId);
            Newmission.agent = db.agents.Include(a => a.location).FirstOrDefault(a => a.Id == mission.agentId);
            Newmission.status = MissionStatus.TeamSuggestion;
            db.missions.Add(Newmission);
            db.SaveChanges();
            return Newmission;
        }



        // Retrieving all missions
        // The function retrieves all missions from the database, including the associated target and agent locations.
        public async Task<List<MissionModel>> getAllMission()
        {
            return db.missions
                .Include(m => m.target).ThenInclude(t => t.location)
                .Include(m => m.agent).ThenInclude(a => a.location)
                .ToList();
        }



        // Changing mission status
        // The function updates the mission status if it is a team suggestion and the update status is 'Teamed'.
        public async Task<MissionStatus> ChangeMissionStatus(missionStatusDTO updateMission)
        {
            MissionModel? mission = db.missions.FirstOrDefault(m => m.Id == updateMission.missionId);
            if (mission.status == MissionStatus.TeamSuggestion && updateMission.status == MissionStatus.Teamed)
            {
                mission.status = updateMission.status;
            }
            return mission.status;
        }



        // Updating missions
        // The function retrieves all teamed missions, moves the associated agents, and returns the updated list of missions.
        public async Task<List<MissionModel>> update()
        {
            List<MissionModel> missions = db.missions
                .Include(m => m.target).ThenInclude(t => t.location)
                .Include(m => m.agent).ThenInclude(a => a.location)
                .Where(m => m.status == MissionStatus.Teamed)
                .ToList();

            foreach (MissionModel mission in missions)
            {
                AgentModel agent = mission.agent;
                agentService.move("", agent.Id);
            }
            return missions;
        }
    }
}
