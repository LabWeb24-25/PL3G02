using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LibSpace_Aspnet.Models
{
    [Table("PendingBibliotecarios")]
    public class BibliotecarioPendente
    {

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(450)]
        public string AspNetUserId { get; set; } = null!;

        [StringLength(450)]
        public string? AspNetUserIdAdmin { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsApproved { get; set; } = false;

        [ForeignKey("AspNetUserId")]
        public virtual IdentityUser AspNetUser { get; set; } = null!;

        [ForeignKey("AspNetUserIdAdmin")]
        public virtual IdentityUser? AspNetUserIdAdminNavigation { get; set; }

    }
}
