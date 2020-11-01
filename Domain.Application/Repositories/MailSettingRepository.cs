using Domain.Application.Entities;
using Domain.Application.IRepositories;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.Repositories
{
	public class MailSettingRepository : Repository<ApplicationDBContext, MailSetting>, IMailSettingRepository
	{
		public MailSettingRepository(IUnitOfWork<ApplicationDBContext> unitOfWork) : base(unitOfWork)
		{

		}
	}
}
