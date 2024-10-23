namespace AdoptionHub.Services;

public class LogInLogService
{
    public void UpdateLogRegistry(string msg) 
    {
        string logEntry = "date: " + DateTime.Now + ", " + msg;
        string dir = Directory.GetCurrentDirectory();
        string path = Path.Combine(dir, "Output\\Log.txt");
        using (StreamWriter sw = new StreamWriter(path, true))
        {
            sw.WriteLine(logEntry);
        }
    }
}