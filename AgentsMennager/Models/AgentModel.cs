using AgentsMennager.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgentsMennager.Models
{
    public class AgentModel
    {
        public AgentModel()
        {
            status = AgentStatus.SleeperAgent;
        }

        public int Id {  get; set; }
        public string nickname { get; set; }
        public string photoUrl { get; set; }
        public PositionModel location { get;  set; } 
        public AgentStatus status { get; set; } 
    }
}
