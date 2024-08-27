using AgentsMennager.DAL;
using AgentsMennager.DTO;
using AgentsMennager.Models;
using AgentsMennager.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgentsMennager.Services
{
    public class TargetServis
    {
        private readonly DataLayer db;

        public TargetServis(DataLayer _db)
        {
            db = _db;
        }


        // Creating a new target
        public async Task<int> createNewTarget(NewTargetDTO target)
        {
            TargetModel Ntarget = new TargetModel();
            Ntarget.name = target.name;
            Ntarget.photoUrl = target.photoUrl;
            Ntarget.position = target.position;
            Ntarget.location = new PositionModel { x = 1, y = 1 };
            Ntarget.Status = Utils.TargetStatus.TargetAlive;
            db.targets.Add(Ntarget);
            db.SaveChanges();
            return Ntarget.Id;
        }



        // Retrieving all targets
        public async Task<List<TargetModel>> getAllTargets()
        {
            return db.targets.Include(t => t.location).ToList();
        }



        // Retrieving a target by ID
        public async Task<TargetModel> getTargetBtId(int id)
        {
            return db.targets.Include(t => t.location).FirstOrDefault(t => t.Id == id);
        }



        // Pinning a target's location
        public async Task<TargetModel> pin(PinDTO targetpin, int id)
        {
            TargetModel? target = db.targets.Include(t => t.location).FirstOrDefault(t => t.Id == id);
            if (targetpin.x > 1 && targetpin.x < 1000 && targetpin.x > 1 && targetpin.x < 1000)
            {
                target.location.y = targetpin.y;
                target.location.x = targetpin.x;
                db.SaveChanges();
            }
            return target;
        }



        // Moving a target
        public async Task<PositionModel> move(int id, string direction)
        {
            TargetModel target = db.targets.Include(t => t.location).FirstOrDefault(a => a.Id == id);
            PositionModel position = target.location;
            if (target.location.x > 1 && target.location.x < 1000 && target.location.y > 1 && target.location.y < 1000)
            {
                target.location.move(position, direction);
                db.SaveChanges();
                return position;
            }
            return position;
        }



        // Suggesting a mission for a target
        public async Task<int> suggAMission(int targetId)
        {
            TargetModel target = db.targets.Find(targetId);
            if (target.Status != TargetStatus.TargetAlive)
            {
                throw new Exception("Target is not alive.");
            }

            var agents = db.agents
                .Where(t => t.status == AgentStatus.SleeperAgent)
                .Where(t => !db.missions.Any(m =>
                    m.target.Id == t.Id &&
                    (m.status == MissionStatus.Teamed || (m.target.Id == targetId && m.agent.Id == t.Id))))
                .ToList();

            foreach (AgentModel agent in agents)
            {
                MissionModel mission = new MissionModel();
                mission.target = db.targets.Find(targetId);
                mission.agent = agent;
                double time = (mission.timeToAction - DateTime.Now).TotalHours;
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
