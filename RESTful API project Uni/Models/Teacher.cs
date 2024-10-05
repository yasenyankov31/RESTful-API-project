namespace RESTful_API_project_Uni.Models
{
    public class Teacher
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public int? SchoolId { get; set; }
        public School? School { get; set; }
        public List<Class>? Classes { get; set; }
    }
}
