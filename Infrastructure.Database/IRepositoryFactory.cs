using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database
{
    public interface IRepositoryFactory
    {
        IRepository<T> GetRepository<T>() where T : class;
    }
}
