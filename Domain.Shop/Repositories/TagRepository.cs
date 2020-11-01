using Domain.Shop.Dto.Tags;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Database;
using Shop.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Shop.Repositories
{
    public class TagRepository : Repository<ShopDBContext, Tag>, ITagRepository
    {
        public TagRepository(IUnitOfWork<ShopDBContext> unitOfWork) : base(unitOfWork)
        {

        }
        public IEnumerable<TagViewModel> GetTagViewModels()
        {
            return this.All.Select(p => new TagViewModel
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
        public TagViewModel GetTagViewModel(string Id)
        {
            return this.All.Where(p => p.Id == Id).Select(prop => new TagViewModel
            {
                Id = prop.Id,
                Name = prop.Name
            }).FirstOrDefault();
        }
    }
}
