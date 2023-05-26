using MVVM_lb6.Server.Helpers;
using Newtonsoft.Json;

namespace MVVM_Lb4.Json.Commands.AddCommands;

public static class LogMachine
{
    const string LogFileName = @"log.json";
    
    public static async Task LogDataToJson(HotelState hotelState)
    {
        if (!File.Exists(LogFileName))
        {
            File.Create(LogFileName);
        }

        string logData = $"{DateTime.Now} - State changed to {hotelState.ToString()}";
        
        //If GroupId have only zeros create new guid
        string json = JsonConvert.SerializeObject(logData, Formatting.Indented);

        await File.AppendAllTextAsync(LogFileName, json);
    }
}