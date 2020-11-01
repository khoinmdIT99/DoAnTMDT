using Domain.Application.Dto.Login;
using Domain.Application.Dto.Users;
using Domain.Application.Entities;
using Infrastructure.Database;
using Infrastructure.Database.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Application.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Dictionary<string, string> Validate(UserViewModel model);
        DatatableResult<UserGridViewModel> GetUserViewModels(DatatableRequest request);
        UserViewModel GetUserViewModel(string Id);
        User GetUserByEmail(string Email);
        Task ResetPassword(ResetPasswordViewModel resetPassword);

    }
}
