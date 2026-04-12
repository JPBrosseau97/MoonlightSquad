using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoonlightSquad.Class.DAL
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        public bool IsActive { get; set; } = true;

        public int Order { get; set; } = 0;

        [Required]
        public int IdNewsCategory { get; set; }

        [ForeignKey("IdNewsCategory")]
        public NewsCategory? NewsCategory { get; set; }
    }
}
