namespace MKR1_AspNet.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }

        public int ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }
    }
}
