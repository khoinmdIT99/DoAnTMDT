using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Repositories
{
	class SSOSettingRepository : Repository<ApplicationDBContext, SSOSetting>, ISSOSettingRepository
	{
		public SSOSettingRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}
	}
}
