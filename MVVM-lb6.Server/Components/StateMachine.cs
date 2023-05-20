namespace MVVM_lb6.Server.Helpers;

public static class StateMachine
{
    public static HotelState HotelState { get; set; } =
        HotelState.HolidaySeason;
}

public enum HotelState
{
    HolidaySeason = 1,
    GuestAccommodationStop = 2
}