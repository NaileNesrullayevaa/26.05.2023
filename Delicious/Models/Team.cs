namespace Delicious.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string ImageName { get; set; } = null!;
        public int WorkId { get; set; }
        public Work Work { get; set; }=null!;
    }
}
