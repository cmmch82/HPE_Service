using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPE_Service.Models
{
    public class Round
    {
        public int Id { get; set; }
        public Player  Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player Winner { get; set; }
    }
}