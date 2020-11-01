using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Repositories
{
	class SiteSettingRepository : Repository<ApplicationDBContext, SiteSetting>, ISiteSettingRepository
	{
		public SiteSettingRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}
	}
}
