using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleConf
{
    [Serializable]
    public class BusinessEntity
    {
    }

    [Serializable]
    public sealed class Reservation : IEquatable<Reservation>
    {
        public string When_date { get; set; }
        public string When_time { get; set; }
        public string building { get; set; }
        public string Conference { get; set; }
        public string reservation_name { get; set; }
        public string reservation_under { get; set; }
        public int numberOfPeople { get; set; }

        public Reservation()
        {

        }
        public Reservation(string p_building, string p_confroom, string p_date, string p_time, string reservation_name, string reservation_under, int numberOfPeople)
        {
            this.When_date = p_date;
            this.When_time = p_time;
            this.building = p_building;
            this.Conference = p_confroom;
            this.reservation_name = reservation_name;
            this.reservation_under = reservation_under;
            this.numberOfPeople = numberOfPeople;
        }

        public override string ToString()
        {
            return $"[Reservation is at date {this.When_date} and at time {this.When_time} for the building {this.building} in the conference room {this.Conference}]";
        }

        public bool Equals(Reservation other)
        {
            bool isEqual = false;
            // return ((other != null) && (this.When == other.When) && (this.Conference = other.Conference));
            if (other != null)
            {
                if (this.When_date.Equals(other.When_date) && this.Conference.Equals(other.Conference) && this.When_time.Equals(other.When_time) && this.building.Equals(other.building))
                {
                    isEqual = true;
                }
            }//end of outer if
            return isEqual;
        }//end of equals

        public override bool Equals(object other)
        {
            return Equals(other as Reservation);
        }

        public override int GetHashCode()
        {
            return this.When_date.GetHashCode() + this.When_time.GetHashCode();
        }
    }//end Reservation
}//end namespace