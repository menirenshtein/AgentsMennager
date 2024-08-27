using AgentsMennager.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgentsMennager.Models
{
    public class TargetModel
    {
        public TargetModel()
        {
            Status = TargetStatus.TargetAlive;
           
        }

        public int Id { get; set; }
        public string name { get; set; }
        public string photoUrl { get; set; }
        public string position { get; set; }
        public PositionModel location { get; set; } 
        public TargetStatus Status { get; set; }
    }
}
