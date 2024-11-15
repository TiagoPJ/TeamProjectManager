namespace Shared
{
    public class AppSettings
    {
        public Variables Variables { get; set; }
        public string AllowedHosts { get; set; }
    }

    public class Variables
    {
        public int MaxTasks { get; set; }
        public int DaysToReport { get; set; }
    }
}
