using Delicious.Models;

namespace Delicious.ViewModels.TeamViewModels
{
    public class TeamEditVM
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? ImageName { get; set; } 
        public IFormFile? Image { get; set; }
        public int WorkId { get; set; }
        public List<Work>? Works { get; set; } 
    }
}
