using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
    [Table("USER_ROLE")]
    public class UserRole
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        public Role Role { get; set; }
        [MaxLength(50)]
        [Column("ROLE_ID")]
        public string RoleId { get; set; }
        public User User { get; set; }
        [MaxLength(50)]
        [Column("USER_ID")]
        public string UserId { get; set; }
    }
}
