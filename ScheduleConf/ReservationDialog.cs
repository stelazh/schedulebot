using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace ScheduleConf
{


   


    [Serializable]
    public class ReservationDialog

    {

        BusinessClass bclass = null;
        public ReservationDialog()
        {
            bclass = new BusinessClass();
        }


        public enum BuildingOptions
        {
            RestonBuilding1, RestonBuilding2, RestonBuilding3
        }
        static Dictionary<string, Building> buildcollection = new BusinessClass().ListOfBuildings();


        [Prompt(new String[] { "Under whose name this Reservation should be created" })]
        public string Name { get; set; }

        [Prompt(new String[] { "What is the Subject or Title of the Meeting?" })]
        public string subject { get; set; }


        [Prompt(new String[] { "Please select one of the Building in Reston." })]
        public string building_name;

        [Prompt(new String[] { "Please select one of the conference rooms from the list." })]
        public string conference_room;


        [Prompt("What date would you like to make this Reservation for? example: today, tomorrow, or any date like 04-06-2017 {||}", AllowDefault = BoolDefault.True)]
        [Describe("Reservation date, example: today, tomorrow, or any date like 04-06-2017")]

        public DateTime ReservationDate { get; set; }

        public DateTime ReservationTime { get; set; }

        [Prompt("How many people will be joining you for the meeting?")]
        [Numeric(1, 25)]
        public int NumberOfPeople;

        // [Numeric(1, 5)]
        // [Optional]
        //  [Describe("for how you enjoyed your experience with Reservation Bot today (optional)")]
        //   public double? Rating;


        public static IForm<ReservationDialog> BuildForm()
        {

            OnCompletionAsyncDelegate<ReservationDialog> processOrder = async (context, state) =>
            {

                Reservation reserve = new Reservation();
                reserve.reservation_name = state.subject;
                reserve.reservation_under = state.Name;
                reserve.numberOfPeople = state.NumberOfPeople;
                reserve.building = state.building_name;
                reserve.Conference = state.conference_room;
                reserve.When_date = state.ReservationDate.ToShortDateString();
                reserve.When_time = state.ReservationTime.ToShortTimeString();




                await context.PostAsync($"Reservation {reserve.reservation_name} and {reserve.reservation_under} and {reserve.numberOfPeople}, {reserve.Conference}, {reserve.When_date} and {reserve.When_time}");
            };



            return new FormBuilder<ReservationDialog>()
                .Message("Bechtel's Scheduling Bot would will assist you in making a reservation!, Please provide me with the following information.")
                .Field(nameof(Name))
                .Field(nameof(subject))
                .Field(new FieldReflector<ReservationDialog>(nameof(building_name))
                             .SetType(null)
                             .SetPrompt(new PromptAttribute("please select one of the following options:{||}"){
                                 ChoiceStyle = ChoiceStyleOptions.Buttons
                               })
                             .SetDefine((state, field) =>
                             {

                                 foreach (KeyValuePair<string, Building> entry in buildcollection)
                                 {
                                     field.AddDescription(entry.Value.buildingname, entry.Value.buildingname);
                                     field.AddTerms(entry.Key, entry.Key);
                                 }
                                 return Task.FromResult(true);
                             }))
                .Field(nameof(NumberOfPeople))
                .Field(new FieldReflector<ReservationDialog>(nameof(conference_room))
                            .SetType(null)
                            .SetPrompt(new PromptAttribute("please select one of the following options:{||}")
                            {
                                ChoiceStyle = ChoiceStyleOptions.Buttons
                            })
                            .SetDefine((state, field) =>
                            {

                                if (state.building_name != null)
                                {

                                    foreach (ConferenceRoom room in buildcollection[state.building_name].confrooms)

                                    {
                                        field.AddDescription(room.confroom.Trim(), room.confroom.Trim());
                                        field.AddTerms(room.confroom.Trim(), room.confroom.Trim());
                                    }

                                }
                                return Task.FromResult(true);


                            }))

                .Field(nameof(ReservationDate))
                .Field(new FieldReflector<ReservationDialog>(nameof(ReservationDialog.ReservationTime))
                    .SetPrompt(PerLinePromptAttribute("What time would you like to setup the meeting?"))
                    ).AddRemainingFields()
               // .OnCompletion(processOrder)

                .Build();
        }



        private static PromptAttribute PerLinePromptAttribute(string pattern)
        {
            return new PromptAttribute(pattern)
            {
                ChoiceStyle = ChoiceStyleOptions.PerLine
            };
        }








    }
}