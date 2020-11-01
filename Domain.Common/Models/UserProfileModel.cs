using Domain.Common.Enums;
using Infrastructure.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Models
{
	public class UserProfileModel
	{
        public string id { get; set; }
        public string fullname { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public DateTime? day_of_birth { get; set; }
        public int? Gender { get; set; }
		public string gender
		{
			get
			{
				try
				{
					var gen = (Gender)Gender;
					return CustomEnumUtility.TextValue(gen);
				}
				catch (Exception)
				{
					return "";
				}
			}
		}
		public string email { get; set; }
        public string avatar_url { get; set; }
        public string thumb_avatar_url { get; set; }
    }
}
