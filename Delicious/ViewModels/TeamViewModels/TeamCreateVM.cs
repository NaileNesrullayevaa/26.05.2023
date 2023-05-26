using Delicious.Models;

namespace Delicious.ViewModels.TeamViewModels
{
    public class TeamCreateVM
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
        public int WorkId { get; set; }
        public List<Work> Works { get; set; } = null!;
    }
}
