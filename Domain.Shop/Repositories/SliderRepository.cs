using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.Dto.Slider;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shop.Application;

namespace Domain.Shop.Repositories
{
    public class SliderRepository : Repository<ShopDBContext, Slider>, ISliderRepository
    {
        public SliderRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<SliderViewModel> GetDataByIdAsync(string id)
        {
            var item = await this.All.AsNoTracking().Select(w => new SliderViewModel()
                {
                    Id = w.Id,
                    PhotoName = w.PhotoName,
                    Status = w.Status

                })
                .SingleOrDefaultAsync(x => x.Id.Equals(id));

            return item;
        }

        public async Task CreateDataAsync(Slider slider)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDataAsync(Slider slider)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDataAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
