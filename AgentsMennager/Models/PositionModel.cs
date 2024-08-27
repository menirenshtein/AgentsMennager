using AgentsMennager.DAL;
using Azure.Core.GeoJson;
using System.ComponentModel.DataAnnotations;

namespace AgentsMennager.Models
{
    public class PositionModel
    {
        [Key]
        public int Id { get; set; }
        public int x { get; set; }
        public int y { get; set; } 


        public PositionModel move(PositionModel position, string direction)
        {
            
            int x = position.x;
            int y = position.y;
            switch (direction)
            {
                case "n":
                    y -= 1;
                    break;
                case "s":
                    y += 1;
                    break;
                case "w":
                    x -= 1;
                    break;
                case "e":
                    x += 1;
                    break;
                case "ne":
                    x += 1;
                    y -= 1;
                    break;
                case "nw":
                    x -= 1;
                    y -= 1;
                    break;

                case "se":
                    x += 1;
                    y += 1;
                    break;

                case "sw":
                    x -= 1;
                    y += 1;
                    break;

            }
            if (x < 1 || x > 1000 || y < 1 || y > 1000)
            {
                return position;
            }
            position.x = x;
            position.y = y;
            return position;
        }
    }
}