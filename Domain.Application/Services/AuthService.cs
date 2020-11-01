using Domain.Application.IRepositories;
using Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Domain.Common;

namespace Domain.Application.Services
{
	public class AuthService
	{
		IUserRepository userRepository;
		public AuthService(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

        public UserProfileModel CheckLogin(string Username, string Password)
        {
            var query = from obj in userRepository.All
                        where Username == obj.UserName
                        && Password == obj.Password
                        select obj;
            return query.Select(o => new UserProfileModel()
            {
                id = o.Id,
                fullname = o.FullName,
                username = o.UserName,
                phone = o.PhoneNo,
                day_of_birth = o.DayOfBirth,
                Gender = o.Gender,
                email = o.Email,
                //avatar_url = attachmentQuery.Where(p => p.ItemId == o.Id && p.FieldName == "avatar").Select(p => p.Url).FirstOrDefault(),
                //thumb_avatar_url = attachmentQuery.Where(p => p.ItemId == o.Id && p.FieldName == "avatar").Select(p => p.ThumbUrl).FirstOrDefault(),
            }).FirstOrDefault();
        }
    }
}
