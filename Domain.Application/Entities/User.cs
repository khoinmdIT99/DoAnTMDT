using System.ComponentModel.DataAnnotations;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Application.Entities
{
    [Table("USERS")]
    public class User : BaseEntity
    {
        [Key]
        [MaxLength(50)]
        [Column("ID")]
        public string Id { get; set; }
        [MaxLength(50)]
        [Column("FULL_NAME")]
        public string FullName { get; set; }
        [MaxLength(50)]
        [Column("USER_NAME")]
        public string UserName { get; set; }
        [MaxLength(255)]
        [Column("PASSWORD")]
        public string Password { get; set; }
        [MaxLength(255)]
        [Column("EMAIL")]
        public string Email { get; set; }
        [MaxLength(20)]
        [Column("PHONE_NO")]
        public string PhoneNo { get; set; }
        [Column("DAY_OF_BIRTH")]
        public DateTime? DayOfBirth { get; set; }
        [Column("GENDER")]
        public int? Gender { get; set; }
        public ICollection<UserRole> UserRole { get; set; }

        [Column("PROFILE_IMAGE")]
        public string ProfileImage { get; set; }
    }
}
