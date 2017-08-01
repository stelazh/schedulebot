using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleConf
{

    [Serializable]
    class BusinessClass


    {


        public BusinessClass()
        {
            this.initializeState();
        }
        Building b1;
        Building b2;
        Building b3;
        Dictionary<String, Building> entity = new Dictionary<string, Building>(StringComparer.InvariantCultureIgnoreCase);

        public void initializeState()
        {
            b1 = new Building("REBRO1", "Reston RO1 Building");
            List<ConferenceRoom> coll1 = new List<ConferenceRoom>();
            coll1.Add(new ConferenceRoom("REBRO1", 2, "223(VC)", 18));
            coll1.Add(new ConferenceRoom("REBRO1", 3, "318", 10));
            coll1.Add(new ConferenceRoom("REBRO1", 3, "343(VC)", 10));
            b1.confrooms = coll1;



            b2 = new Building("REBRO2", "Reston RO2 Building");
            List<ConferenceRoom> coll2 = new List<ConferenceRoom>();
            coll2.Add(new ConferenceRoom("REBRO2", 1, "102A(VC)", 25));
            coll2.Add(new ConferenceRoom("REBRO2", 1, "102A(VC)", 25));
            coll2.Add(new ConferenceRoom("REBRO2", 1, "102B(25)", 25));

            b2.confrooms = coll2;


            b3 = new Building("REBRO3", "Reston R03 Building");
            List<ConferenceRoom> coll3 = new List<ConferenceRoom>();
            coll3.Add(new ConferenceRoom("REBRO3", 9, "CR1(VC)", 12));
            coll3.Add(new ConferenceRoom("REBRO3", 10, "CR2(VC)", 10));
            coll3.Add(new ConferenceRoom("REBRO3", 10, "CR4(VC)", 25));
            b3.confrooms = coll3;


            entity.Add("REBRO1", b1);
            entity.Add("REBRO2", b2);
            entity.Add("REBRO3", b3);





        }

        public Dictionary<string, Building> ListOfBuildings()
        {
            return entity;

        }

        public String listOfBuildings()
        {
            return b1.buildingname + " \n" + b2.buildingname + "\n" + b3.buildingname;

        }


        public string ListOfConfRoomsforABuilding(string buildingcode)
        {

            string output = "\n";

            if (entity.ContainsKey(buildingcode))
            {
                Building b1 = entity[buildingcode];
                foreach (ConferenceRoom s in b1.confrooms)
                {

                    output = output + s.confroom + "\n";

                }
            }
            else
            {
                output = System.String.Format("The building code {0} is not part of Bechtel Corporation", buildingcode);
            }

            return output;
        }

        public List<string> ListOfConfRoomsforABuildingasCollection(string buildingcode)
        {

            // string output = "\n";
            List<string> outputcoll = new List<string>();
            if (entity.ContainsKey(buildingcode))
            {
                Building b1 = entity[buildingcode];
                foreach (ConferenceRoom s in b1.confrooms)
                {
                    outputcoll.Add(s.confroom);
                    //  output = output + s.confroom + "\n";

                }
            }
            //else
            // {
            //   output = System.String.Format("The building code {0} is not part of Bechtel Corporation", buildingcode);
            //}

            return outputcoll;
        }
        public void createReservation(string nameunder, DateTime reservationstarttime, int hours, string buildingname, string conferenceroom)
        {

        }

        public String ListAllReservationsForABuilding(string buildingcode)
        {
            return "";
        }

    }//end of business classs


    //  Show me all the reservation for rooms in building 1
    //Reserve a conference room 1020 from 2.00clock until 4.00 clock under the name arief
    //Reserve a conference room 1020 from 2.00 clock for 2 hours under the name roy

    //Show all the conference rooms reserved by arief
    //Cancel the 2.00clock reservation.

}