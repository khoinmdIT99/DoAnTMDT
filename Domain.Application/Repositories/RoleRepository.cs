using Domain.Application;
using Domain.Application.Dto.Roles;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Application.Repositories
{
	public class RoleRepository : Repository<ApplicationDBContext, Role>, IRoleRepository
	{
		public RoleRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}

		public IEnumerable<RoleViewModel> GetRoleViewModels()
		{
			return this.All.Select(prop => new RoleViewModel
			{
				Id = prop.Id,
				RoleCode = prop.RoleCode,
				RoleName = prop.RoleName
			}).ToList();
		}

		public RoleViewModel GetRoleViewModel(string Id)
		{
			return this.All.Where(p => p.Id == Id).Select(prop => new RoleViewModel
			{
				Id = prop.Id,
				RoleCode = prop.RoleCode,
				RoleName = prop.RoleName
			}).FirstOrDefault();
		}

		public Dictionary<string, string> Validate(RoleViewModel model)
		{
			var query = this.All;
			if (!string.IsNullOrEmpty(model.Id))
				query = query.Where(p => p.Id != model.Id);
			Dictionary<string, string> dicError = new Dictionary<string, string>();
			if (query.Any(p => p.RoleCode == model.RoleCode))
			{
				dicError.Add("RoleCode", "Mã vai trò đã tồn tại");
			}
			if (query.Any(p => p.RoleName == model.RoleName))
			{
				dicError.Add("RoleName", "Tên vai trò đã tồn tại");
			}
			return dicError;
		}
	}
}
