using Domain.Application;
using Domain.Application.Dto.Login;
using Domain.Application.Dto.Users;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Domain.Common.Enums;
using Domain.Common.Security;
using Infrastructure.Database;
using Infrastructure.Database.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Application.Repositories
{
	public class UserRepository : Repository<ApplicationDBContext, User>, IUserRepository
	{
		public UserRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}

		public User GetUserByEmail(string Email)
		{
			return this.All.Where(user => user.Email == Email).FirstOrDefault();
		}


		public UserViewModel GetUserViewModel(string Id)
		{
			return this.All.Where(p=>p.Id == Id).Select(p => new UserViewModel
			{
				Id = p.Id,
				FullName = p.FullName,
				UserName = p.UserName,
				Password = p.Password,
				Email = p.Email,
				DayOfBirth = p.DayOfBirth,
				Gender = p.Gender,
				PhoneNo = p.PhoneNo,
				Roles = p.UserRole.Select(p => p.RoleId).ToList(),
				ProfileImage = p.ProfileImage
			}).FirstOrDefault();
		}

		public DatatableResult<UserGridViewModel> GetUserViewModels(DatatableRequest request)
		{
			var query = this.All.Select(p => new UserGridViewModel
			{
				Id = p.Id,
				FullName = p.FullName,
				UserName = p.UserName,
				Email = p.Email,
				PhoneNo = p.PhoneNo,
				ProfileImage = p.ProfileImage,
				Roles = p.UserRole.Select(r=> new UserRoleGridViewModel { RoleId = r.RoleId, RoleName = r.Role.RoleName })
			});

			return query.ToDatatableResult(request);
		}

		public async Task ResetPassword(ResetPasswordViewModel resetPassword)
		{
			var user = this.GetUserByEmail(resetPassword.Email);
			user.Password = Security.EncryptPassword(resetPassword.Password);
			await this.SaveAsync();
		}


		public Dictionary<string, string> Validate(UserViewModel model)
		{
			var query = this.All;
			if (!string.IsNullOrEmpty(model.Id))
				query = query.Where(p => p.Id != model.Id);
			Dictionary<string, string> dicError = new Dictionary<string, string>();
			if (query.Any(p => p.UserName == model.UserName))
			{
				dicError.Add("UserName", "Tên đăng nhập đã tồn tại");
			}
			if (query.Any(p => p.Email == model.Email))
			{
				dicError.Add("RoleName", "Email đã tồn tại");
			}
			return dicError;
		}
	}
}
