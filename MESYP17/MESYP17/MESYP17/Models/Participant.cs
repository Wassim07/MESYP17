using System;

namespace MESYP17.Models
{
    public class Participant:IComparable
    {
        public string idParticipant { get; set; }
        public string fullName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string avatar { get; set; }
        public string country { get; set; }
        public int scoreBonus { get; set; }
        public int scoreWorkshop { get; set; }
        public int score { get; set; }



        public int CompareTo(object obj)
        {
            Participant p = (Participant) obj;
            if (score == p.score)
                return 0;
            return this.score > p.score ? -1 : 1;
        }
    }
}
