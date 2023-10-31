using Serilog;
using System.Diagnostics;

namespace HangFireApp.BackGroundJobs
{
    public class BackGroundJobRunGetStocksService
    {
        public static int counter=0;

        public void RunGetStocksService(string getStocksServicePath)
        {
            try
            {
                Process.Start(getStocksServicePath);
                Log.Information($"GetStocksService {counter}. kere çalışıyor");
                counter++;
            }
            catch (Exception ex)
            {
                Log.Warning("FileNotFound or File is used");
            }
        }

    }
}
