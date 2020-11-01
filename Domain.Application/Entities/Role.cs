using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Application.Entities
{
    [Table("ROLES")]
    public class Role : BaseEntity
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("ROLE_CODE")]
        public string RoleCode { get; set; }
        [MaxLength(255)]
        [Column("ROLE_NAME")]
        public string RoleName { get; set; }
    }
}
