namespace Entities.Entities
{
    public class ProjectReportUser
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int QtdProjects { get; set; }
        public int QtdTasks { get; set; }
        public int QtdComments { get; set; }
        public int QtdDays { get; set; }
        public string InformationProjects => $"User have {QtdProjects} project(s) on their name in the last {QtdDays}.";
        public string InformationTasks => $"User have {QtdTasks} task(s) on their name in the last {QtdDays}.";
        public string InformationComments => $"User have {QtdComments} comment(s) on their name in the last {QtdDays}.";
    }
}
