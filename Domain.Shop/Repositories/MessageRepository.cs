using System;
using System.Collections.Generic;
using System.Text;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class MessageRepository: Repository<ShopDBContext, Message>, IMessageRepository
    {
        public MessageRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
