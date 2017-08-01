using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleConf
{
    [Serializable]
    class Building
    {

        public Building(string p_name, string full_name)
        {

            this.buildingname = p_name;
            this.full_name = full_name;


        }
        public string buildingname { get; set; }
        public List<ConferenceRoom> confrooms { get; set; }
        public string full_name { get; set; }



    }//end of Building class

    [Serializable]
    public class ConferenceRoom
    {


        public ConferenceRoom(String p_id, int p_floor, string p_confroom, int capacity)
        {
            this.id = p_id;
            this.floor = p_floor;
            this.confroom = p_confroom;
            this.capacity = capacity;


        }



        Boolean occupied { get; set; }
       public string id { get; set; }
       public  int floor { get; set; }
        public string confroom { get; set; }

      public  int capacity { get; set; }


    }//end of conferenceroomclass



}