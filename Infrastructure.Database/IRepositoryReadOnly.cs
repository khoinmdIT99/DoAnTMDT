using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database
{
    public interface IRepositoryReadOnly<T> : IReadRepository<T> where T : class
    {

    }
}
