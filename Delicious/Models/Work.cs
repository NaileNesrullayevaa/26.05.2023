namespace Delicious.Models
{
    public class Work
    {
        public Work()
        {
            Teams=new List<Team>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Team> Teams { get; set; }=null!;
    }
}
