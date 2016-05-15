using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Move
    {   
        //public int ChampionshipId { get; set; }
        public int MoveId { get; set; }
        public int PlayerId { get; set; }
        public User Player { get; set; }
        public Strategy PlayerStrategy { get; set; }
        //public virtual Championship Championship { get; set; }
    }
}
