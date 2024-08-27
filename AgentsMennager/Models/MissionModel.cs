
using AgentsMennager.Utils;

namespace AgentsMennager.Models
{
    public class MissionModel
    {
        public int Id { get; set; }
        public AgentModel? agent { get; set; }
        public TargetModel? target { get; set; }
        public DateTime timeToEndMission { get; set; }
        public void initTime()
        {
            AgentModel agentToId = new AgentModel();
            TargetModel targetToId = new TargetModel();

            double calc = Math.Sqrt(Math.Pow(agent.location.x - targetToId.location.x, 2)
                                   + (Math.Pow(agentToId.location.y - targetToId.location.y, 2)));
            timeToEndMission = DateTime.Now.AddHours(calc / 5);
        }
        public DateTime timeToAction { get; set; }
        public MissionStatus status { get; set; }
    }
}
