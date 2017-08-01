using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;

using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;

namespace ScheduleConf
{
  //  [LuisModel("46ccffb8-4076-4e82-9c5c-2f3210685ddf", "53663e3dc7254912ac216f07c7ce0e14")]
    [LuisModel("46ccffb8-4076-4e82-9c5c-2f3210685ddf", "f288bbb92f8e4e2182f4607a4f1bfd35", LuisApiVersion.V2)]
    [Serializable]
    public class MyDIalog : LuisDialog<object>
    {

        internal static IFormDialog<ReservationDialog> MakeRootDialog()
        {


            return Microsoft.Bot.Builder.FormFlow.FormDialog.FromForm(ReservationDialog.BuildForm,FormOptions.PromptInStart);
        }



        BusinessClass bClass = null;
        /*
        * constants for the dialog.
        */
        public const string entity_building = "building";
        public const string ROOM = "conference";
        public const string TIME = "builtin.datetime.time";
        public const string DATE = "builtin.datetime.date";

        


        private List<Reservation> reservationCollection = null;
        private QuestionandAnswer qanda;



        public MyDIalog() {
            bClass = new BusinessClass();
            qanda = new QuestionandAnswer();
            reservationCollection = new List<Reservation>();
            


        }

        //constructor
        public MyDIalog(ILuisService service): base(service) {
            bClass = new BusinessClass();
            qanda = new QuestionandAnswer();
            reservationCollection = new List<Reservation>();
        }

        [LuisIntent("list_conference")]
        public async Task ListConferenceRooms(IDialogContext context, LuisResult result)
        {
            string buildingName = string.Empty;
            EntityRecommendation building;
            
            if (result.TryFindEntity(entity_building, out building))
            {
                buildingName = building.Entity;
            }

            if(!string.IsNullOrEmpty(buildingName))

            {
               
                string replyMessage = string.Empty;
                Building build = bClass.ListOfBuildings()[buildingName];
                if (build != null)
                {
                    replyMessage += $"Bechtel's has the following Conference Rooms in building {build.full_name} \n\n";
                    foreach (ConferenceRoom room in build.confrooms)
                    {
                        replyMessage += $"* {room.confroom} is on floor {room.floor} with a capacity of {room.capacity}\n";
                    }
                    
                }else
                {
                    replyMessage = $"The Building with the code:{buildingName} does not exist";
                }
                await context.PostAsync(replyMessage);
            }
            else
            {
                await context.PostAsync("please specify the building code");
            }

            context.Wait(MessageReceived);
        }//end of ListConferenceRooms


        [LuisIntent("aboutme")]
        public async Task AboutMe(IDialogContext context, LuisResult result)
        {
            // await context.PostAsync(@"This is a Bechtel Bot that assists you with listing of Bechtel properties in Reston and it's conference rooms. It can assist you with scheduling/cancelling a Meeting/Conference Room");
            // await context.PostAsync(@"Bechtel is truly becoming a software company and it's Digital Transformation starts with its Digital Alternate, the BECHTEL BOT.");
            string query = result.Query;
            await context.PostAsync(qanda.callKnowledgebase(result.Query));

            context.Wait(MessageReceived);
        }

        /**
        * This is the most important method, need to find all the entity, that describes a Reservation.
*/


        [LuisIntent("create_reservation")]
        public async Task createReservationWithDialog(IDialogContext context, LuisResult result)
        {
            IFormDialog<ReservationDialog> tmp = MakeRootDialog();
            context.Call<ReservationDialog>(tmp, ReservationDataCollected);

        }

        private async Task ReservationDataCollected(IDialogContext context, IAwaitable<ReservationDialog> result)
        {
            ReservationDialog r = await result;
            Reservation reserve = new Reservation();
            reserve.reservation_name = r.subject;
            reserve.reservation_under = r.Name;
            reserve.numberOfPeople = r.NumberOfPeople;
            reserve.building = r.building_name;
            reserve.Conference = r.conference_room;
            reserve.When_date = r.ReservationDate.ToShortDateString();
            reserve.When_time = r.ReservationTime.ToShortTimeString();
          
            if (reservationCollection.Contains(reserve))
            {
                await context.PostAsync($" A reservation with in the building {reserve.building} in conference room {reserve.Conference} on {reserve.When_date} at {reserve.When_time} already exists. Please use a different conference room. Thank you.");
            }
            else
            {
                reservationCollection.Add(reserve);
                await context.PostAsync($"Your Reservation {reserve.reservation_under} named {reserve.reservation_name} in Building {reserve.building} for conference room {reserve.Conference}  on {reserve.When_date}  at {reserve.When_time} has been sucessfully scheduled. Thank You.");

            }


           // await context.PostAsync($"the reservation is {r.Name}");
            context.Wait(MessageReceived);
        }


        [LuisIntent("list_buildings")]
        public async Task ListBuildings(IDialogContext context, LuisResult result)
        {
            string replyMessage = string.Empty;
            replyMessage += $"Bechtel's has the following properties in Reston \n\n";
            foreach (KeyValuePair<string, Building> entry in bClass.ListOfBuildings())
            {
                replyMessage += $"* {entry.Value.full_name}\n";
            }

            await context.PostAsync(replyMessage);
            context.Wait(MessageReceived);
        }



        [LuisIntent("list_reservation_building")]
        public async Task listReservationBuilding(IDialogContext context, LuisResult result)
        {
            string buildingName = string.Empty;
            //1
            EntityRecommendation building;

            if (result.TryFindEntity(entity_building, out building))
            {
                buildingName = building.Entity;
            }
            if (string.IsNullOrEmpty(buildingName))
            {
                await context.PostAsync("Please specify a building within which to list all the reservations to avoid long answers.");

            }else
            {
                int count = 0;
                foreach(Reservation reservation in reservationCollection)
                {
                    if (reservation.building.Equals(buildingName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await context.PostAsync($"Conference Room {reservation.Conference} on date {reservation.When_date} at time {reservation.When_time}");
                        count += 1;
                    }
                    if (count == 0)
                    {
                        await context.PostAsync($"There are no reservations in the building {buildingName}");
                    }
                }
            }


            context.Wait(MessageReceived);
        }

        [LuisIntent("list_reservation_date")]
        public async Task listReservationDate(IDialogContext context, LuisResult result)
        {
           // string buildingName = string.Empty;
            string date_value = string.Empty;
            string date_1 = string.Empty;

            //3- date

            EntityRecommendation date;

            if (result.TryFindEntity(DATE, out date))
            {
                date_1 = date.Entity;
                date.Resolution.TryGetValue("date", out date_value);
            }

            if (string.IsNullOrEmpty(date_1))

            {
                await context.PostAsync("please specify the date");
            }   else
            {
                int count = 0;
                foreach (Reservation reservation in reservationCollection)
                {
                    if (reservation.When_date.Equals(date_value, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await context.PostAsync($"Reservation in Building {reservation.building} in Conference Room {reservation.Conference} on date {reservation.When_date} at time {reservation.When_time}");
                        count += 1;
                    }
                }//end of for
                    if (count == 0)
                    {
                        await context.PostAsync($"There are no reservations for the date  {date_value} in any of the Bechtel buildings");
                    }
                }
            


            context.Wait(MessageReceived);
        }
        [LuisIntent("welcome")]
        public async Task welcome(IDialogContext context, LuisResult result)
        {

            await context.PostAsync("I am glad to be any assistance to you. Welcome!");

            context.Wait(MessageReceived);

        }





        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry i did not understand what you said";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }


    }//end of the class
}//end of namespace
