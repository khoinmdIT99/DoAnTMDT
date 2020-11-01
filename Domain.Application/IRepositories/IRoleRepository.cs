using Domain.Application.Dto.Roles;
using Domain.Application.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Application.IRepositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Dictionary<string, string> Validate(RoleViewModel model);
        IEnumerable<RoleViewModel> GetRoleViewModels();
        RoleViewModel GetRoleViewModel(string Id);
    }
}
