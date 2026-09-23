using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Web.Models
{
    public class ReliefProject
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public List<FieldUpdate> Updates { get; set; } = new();
    }

    public class FieldUpdate
    {
        public int Id { get; set; }

        public int ReliefProjectId { get; set; }
        public ReliefProject? ReliefProject { get; set; }

        [Required, StringLength(100)]
        public string ProjectName { get; set; } = string.Empty; // denormalised for easy dashboard display

        [Required]
        public string Message { get; set; } = string.Empty;

        public string PostedBy { get; set; } = string.Empty;

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}
