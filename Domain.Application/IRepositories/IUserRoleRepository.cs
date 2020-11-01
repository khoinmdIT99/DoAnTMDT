using Domain.Application.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Application.IRepositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
    }
}
