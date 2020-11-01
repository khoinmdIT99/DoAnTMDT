using Domain.Application;
using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Repositories
{
	public class UserRoleRepository : Repository<ApplicationDBContext, UserRole>, IUserRoleRepository
	{
		public UserRoleRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}
	}
}
