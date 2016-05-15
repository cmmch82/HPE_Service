using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Championship
    {
        public int ChampionshipId { get; set; }
      //  public int FirstPlaceId { get; set; }
        public Move FirstPlace { get; set; }
       // public int SecondPlaceId { get; set; }
        public Move SecondPlace { get; set; }
       // public virtual Move FirstPlace { get; set; }
       // public virtual Move SecondPlace { get; set; }
    }
}
