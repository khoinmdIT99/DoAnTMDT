using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Shop.Dto;
using Domain.Shop.Dto.Slider;
using Domain.Shop.Entities;
using Infrastructure.Database;

namespace Domain.Shop.IRepositories
{
    public interface ISliderRepository : IRepository<Slider>
    {
        Task<SliderViewModel> GetDataByIdAsync(string id);
        Task CreateDataAsync(Slider slider);
        Task UpdateDataAsync(Slider slider);
        Task DeleteDataAsync(string id);
    }
}
