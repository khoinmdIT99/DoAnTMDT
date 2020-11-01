using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.Database.Entities
{
    public class BaseEntity : IBaseEntity
    {
        [Column("CREATE_AT")]
        public DateTime? CreateAt { get; set; }
        [Column("LAST_UPDATE_AT")]
        public DateTime? LastUpdateAt { get; set; }
        [MaxLength(50)]
        [Column("CREATE_BY")]
        public string CreateBy { get; set; }
        [MaxLength(50)]
        [Column("LAST_UPDATE_BY")]
        public string LastUpdateBy { get; set; }
    }
}
