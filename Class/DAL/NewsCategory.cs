using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoonlightSquad.Class.DAL
{
    public class NewsCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NewsCategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public required string CategoryName { get; set; }

        [Required]
        [StringLength(150)]
        public required string CategoryDescription { get; set; }

        public bool IsEnabled { get; set; } = false;
    }
}
