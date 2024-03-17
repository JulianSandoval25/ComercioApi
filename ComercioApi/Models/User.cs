using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ComercioApi.Models
{
    [Index(nameof(userName) ,IsUnique = true)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string userName { get; set; }

        public required string password { get; set; }

        public required string rol { get; set; }
    }
}
