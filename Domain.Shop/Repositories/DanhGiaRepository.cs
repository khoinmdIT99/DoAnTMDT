using System;
using System.Collections.Generic;
using System.Text;
using Domain.Shop.Entities.SystemManage;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class DanhGiaRepository: Repository<ShopDBContext, DanhGia>, IDanhGiaRepository
    {
        public DanhGiaRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
