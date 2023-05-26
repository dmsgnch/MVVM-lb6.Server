using MVVM_Lb4.Json.Commands.AddCommands;

namespace MVVM_lb6.Server.Helpers;

 public static class StateMachine
 {
     public static HotelState HotelState { get; set; } =
         HotelState.HolidaySeason;

     public static async void ChangeState(HotelState hotelState)
     {
         switch (hotelState)
         {
             case HotelState.HolidaySeason:
                 HotelState = HotelState.HolidaySeason;
                 await Loger(HotelState.HolidaySeason);
                 break;
             case HotelState.GuestAccommodationStop:
                 HotelState = HotelState.GuestAccommodationStop;
                 await Loger(HotelState.HolidaySeason);
                 break;
             default:
                 throw new Exception();
         }
     }

     private static async Task Loger(HotelState hotelState)
     {
         await LogMachine.LogDataToJson(hotelState);
     }
 }

 public enum HotelState
 {
     HolidaySeason = 1,
     GuestAccommodationStop = 2
 }