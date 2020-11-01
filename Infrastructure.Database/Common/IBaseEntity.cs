using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database.Common
{
    public interface IBaseEntity
    {
        DateTime? CreateAt { get; set; }
        DateTime? LastUpdateAt { get; set; }
        string CreateBy { get; set; }
        string LastUpdateBy { get; set; }
    }
}
