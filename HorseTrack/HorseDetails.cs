using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorseTrack
{
    internal class HorseDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Odds { get; set; }
        public bool IsWinner { get; set; } = false;
    }
}
