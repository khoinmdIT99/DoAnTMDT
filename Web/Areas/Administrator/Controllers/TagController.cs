using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application.Entities;
using Domain.Shop.Dto.Tags;
using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Infrastructure.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class TagController : BaseController
    {
        private readonly ITagRepository _tagRepository;
        private readonly IProductTagRepository _productTagRepository;
        public TagController(ITagRepository tagRepository, IProductTagRepository productTagRepository)
        {
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
        }
        public IActionResult Index()
        {
            return View(_tagRepository.GetTagViewModels());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _tagRepository.Add(new Tag()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.Name
                    });
                    _tagRepository.Save(RequestContext);

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                Tag tag = _tagRepository.Get(id);
                var productTagList = _productTagRepository.GetProductTagViewModelsByTagId(id).Select(s => s.Id).ToList();
                if (productTagList.Count > 0)
                {
                    foreach (var item in productTagList)
                    {
                        _productTagRepository.Delete(item);
                    }
                }
                _productTagRepository.Save(RequestContext);
                _tagRepository.Delete(tag);
                _tagRepository.Save(RequestContext);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IActionResult Update(string id)
        {
            var model = _tagRepository.GetTagViewModel(id);
            if (model == null)
            {
                return View();
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(TagViewModel tag)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Tag t = _tagRepository.All.Where(s => s.Id == tag.Id).First();
                    t.Name = tag.Name;
                    _tagRepository.Save(RequestContext);
                }
                catch (Exception)
                {
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
