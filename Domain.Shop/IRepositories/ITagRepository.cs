using Domain.Shop.Dto.Tags;
using Domain.Shop.Entities;
using Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Shop.IRepositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        IEnumerable<TagViewModel> GetTagViewModels();
        TagViewModel GetTagViewModel(string Id);
    }
}
