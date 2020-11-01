using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Domain.Common.Enums;
using Infrastructure.Common.Enums;

namespace Infrastructure.Web.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? DayOfBirth { get; set; }
        public int? Gender { get; set; }
        public string gender {

            get
            {
                try
                {
                    var gen = (Gender)Gender;
                    return gen.TextValue();
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        public IEnumerable<RoleInfo> RoleInfo { get; set; }

        public bool IsRole(string RoleId)
        {
            return this.RoleInfo.Any(p => p.Id == RoleId);
        }
    }

    public class RoleInfo
    {
        public string Id { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
    }
}
