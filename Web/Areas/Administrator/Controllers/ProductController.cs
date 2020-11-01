using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Shop.Dto.ProductImage;
using Domain.Shop.Dto.Products;
using Domain.Shop.Entities;
using Domain.Shop.Enums;
using Domain.Shop.IRepositories;
using Infrastructure.Common;
using Infrastructure.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [BaseAuthorization]
    public class ProductController : BaseController
    {
        private string WarningCreateMaterial = "Warning : Material have not been created";
        private string WarningCreateProductType = "Warning : Product Type have not been created";
        private string WarningCreateCategory = "Warning : Product Category have not been created";
        private readonly IProductRepository _productRepository;
        private readonly IProductTypeRepository _productTypeRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductReViewRepository _productReViewRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        public ProductController(IProductRepository productRepository, IProductTypeRepository productTypeRepository,
            IMaterialRepository materialRepository, ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment, IConfiguration config, IProductImageRepository productImageRepository,
            ITagRepository tagRepository, IProductTagRepository productTagRepository,
            IAccountRepository accountRepository, IProductReViewRepository productReViewRepository)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
            _productTypeRepository = productTypeRepository;
            _productRepository = productRepository;
            _materialRepository = materialRepository;
            _categoryRepository = categoryRepository;
            _productTagRepository = productTagRepository;
            this._accountRepository = accountRepository;
            this._productReViewRepository = productReViewRepository;
            _productImageRepository = productImageRepository;
            _tagRepository = tagRepository;
        }
        public ActionResult Index()
        {
            IEnumerable<ProductViewModel> list = _productRepository.GetProductViewModels();
            foreach (var item in list)
            {
                item.PriceType = Enum.GetName(typeof(PriceType), int.Parse(item.PriceType));
            }
            return View(list);
        }
        private void SetComboData()
        {
            TempData["WarningCreateCategory"] = null;
            TempData["WarningCreateMaterial"] = null;
            TempData["WarningCreateProductType"] = null;
            if (!_categoryRepository.All.Any())
            {
                TempData["WarningCreateCategory"] = WarningCreateCategory;
            }
            else if (_materialRepository.All.Count() == 0)
            {
                TempData["WarningCreateMaterial"] = WarningCreateMaterial;
            }
            else if (_productTypeRepository.All.Count() == 0)
            {
                TempData["WarningCreateProductType"] = WarningCreateProductType;
            }
            else
            {
                var tagRepository = _tagRepository.All;
                var productTypeRepository = _productTypeRepository.All;
                var categoryRepository = _categoryRepository.All;
                var materialRepository = _materialRepository.All;
                ViewBag.productTypeRepository = productTypeRepository.Select(p => new SelectListItem
                {
                    Text = p.TypeName,
                    Value = p.Id
                }).ToList();
                ViewBag.categoryRepository = categoryRepository.Select(p => new SelectListItem
                {
                    Text = p.CategoryName,
                    Value = p.Id
                }).ToList();
                ViewBag.materialRepository = materialRepository.Select(p => new SelectListItem
                {
                    Text = p.MaterialName,
                    Value = p.Id
                }).ToList();
                ViewBag.tagRepository = tagRepository.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id
                }).ToList();
                var priceTypeList = new List<SelectListItem>();
                foreach (int priceType in Enum.GetValues(typeof(PriceType)))
                {
                    priceTypeList.Add(new SelectListItem { Text = Enum.GetName(typeof(PriceType), priceType), Value = priceType.ToString() });
                }
                ViewBag.priceType = priceTypeList;
            }
        }
        private List<string> ProcessUploadedFile(ProductViewModel model)
        {
            List<string> uniqueFileNameList = new List<string>();
            if (model.ProductImages != null && model.ProductImages.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imageUpload");
                string uniqueFileName = null;
                foreach (var image in model.ProductImages)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "\\" + image.FileName;
                    uniqueFileNameList.Add(uniqueFileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    image.CopyTo(fileStream);
                }
            }
            return uniqueFileNameList;
        }
        public ActionResult Create()
        {
            SetComboData();
            if (TempData["WarningCreateCategory"] != null)
            {
                return RedirectToAction("Create", "Category", new { area = "Administrator" });
            }
            else if (TempData["WarningCreateMaterial"] != null)
            {
                return RedirectToAction("Create", "Material", new { area = "Administrator" });
            }
            else if (TempData["WarningCreateProductType"] != null)
            {
                return RedirectToAction("Create", "ProductType", new { area = "Administrator" });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ProductId = Guid.NewGuid().ToString();
                    List<string> productImageList = ProcessUploadedFile(model);
                    _productRepository.Add(new Product()
                    {
                        Id = ProductId,
                        ProductCode = model.ProductCode,
                        ProductName = model.ProductName,
                        Slug = model.Slug,
                        Description = model.Description,
                        ProductTypeId = model.ProductTypeId,
                        MaterialId = model.MaterialId,
                        CategoryId = model.CategoryId,
                        PriceType = int.Parse(model.PriceType),
                        Price = model.Price
                    });
                    _productRepository.Save(RequestContext);

                    if (productImageList != null && productImageList.Count > 0)
                    {
                        foreach (var image in productImageList)
                        {
                            _productImageRepository.Add(new ProductImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = ProductId,
                                Url = image,
                                CreateAt = DateTime.UtcNow
                            });
                        }
                        _productImageRepository.Save(RequestContext);
                    }
                    if (model.TagList != null && model.TagList.Count > 0)
                    {
                        foreach (var item in model.TagList)
                        {
                            _productTagRepository.Add(new ProductTag()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = ProductId,
                                TagId = item
                            });
                        }
                    }
                    _productTagRepository.Save(RequestContext);
                }
                catch (Exception)
                {
                    SetComboData();
                    return View();
                }
                return RedirectToAction("Index");
            }
            SetComboData();
            return View();
        }

        public ActionResult Update(string id)
        {
            var model = _productRepository.GetProductViewModelById(id);
            SetComboData();
            ViewBag.checkedTag = _productTagRepository.GetProductTagViewModelsByProductId(id).Select(s => s.TagId).ToList();
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
        public ActionResult Update(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<string> productImageList = ProcessUploadedFile(model);
                    var product = _productRepository.GetProductById(model.Id);
                    PropertyCopy.Copy(model, product);
                    _productRepository.Update(product);
                    _productRepository.Save(RequestContext);
                    if (productImageList != null && productImageList.Count > 0)
                    {
                        foreach (var image in productImageList)
                        {
                            _productImageRepository.Add(new ProductImage()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProductId = model.Id,
                                Url = image,
                                CreateAt = DateTime.UtcNow
                            });
                        }
                        _productImageRepository.Save(RequestContext);
                    }
                    var listProductTag = _productTagRepository.GetProductTagViewModelsByProductId(model.Id).Select(s => s.TagId).ToList();
                    if (model.TagList != null && model.TagList.Count > 0)
                    {
                        foreach (var item in model.TagList)
                        {
                            if (!listProductTag.Contains(item))
                            {
                                _productTagRepository.Add(new ProductTag()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ProductId = model.Id,
                                    TagId = item
                                });
                            }
                        }
                        _productTagRepository.Save(RequestContext);
                        if (listProductTag != null && listProductTag.Count > 0)
                        {
                            foreach (var item in listProductTag)
                            {
                                if (!model.TagList.Contains(item))
                                {
                                    DeleteProductTag(model.Id, item);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (listProductTag != null && listProductTag.Count > 0)
                        {
                            foreach (var tagId in listProductTag)
                            {
                                DeleteProductTag(model.Id, tagId);
                            }
                        }
                    }
                }
                catch (Exception )
                {
                    SetComboData();
                    return View();
                }

                return RedirectToAction("Index");
            }
            SetComboData();
            return View();
        }
        [HttpPost]
        public bool Delete(string id)
        {
            try
            {
                Product product = _productRepository.Get(id);
                var productImageList = _productImageRepository.All.Where(x => x.ProductId == id).ToList();
                var productTag = _productTagRepository.GetProductTagViewModelsByProductId(id).Select(s => s.TagName).ToList();
                if (productImageList != null && productImageList.Count > 0)
                {
                    foreach (var image in productImageList)
                    {
                        DeleteImageAndFolder(image.Id, image.Url.Split("\\")[0]);
                    }
                }
                if (productTag != null && productTag.Count > 0)
                {
                    foreach (var item in productTag)
                    {
                        DeleteProductTag(id, item);
                    }
                }
                _productRepository.Delete(product);
                _productRepository.Save(RequestContext);
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

        public void DeleteProductTag(string productId, string tagId)
        {
            try
            {
                string id = _productTagRepository.All.Where(w => w.TagId == tagId && w.ProductId == productId).Select(s => s.Id).FirstOrDefault();
                ProductTag productTag = _productTagRepository.Get(id);
                _productTagRepository.Delete(productTag);
                _productTagRepository.Save(RequestContext);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [HttpPost]
        public bool DeleteImage(string image)
        {
            if (!String.IsNullOrEmpty(image))
            {
                try
                {
                    string imageProductId = _productImageRepository.All.Where(m => m.Url.StartsWith(image)).Select(s => s.Id).FirstOrDefault();
                    DeleteImageAndFolder(imageProductId, image);
                    return true;
                }
                catch (Exception )
                {
                    return false;
                }
            }
            return false;
        }
        public void DeleteImageAndFolder(string imageId, string folderName)
        {
            ProductImage productImage = _productImageRepository.Get(imageId);
            _productImageRepository.Delete(productImage);
            _productImageRepository.Save(RequestContext);
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imageUpload");
            Directory.Delete(Path.Combine(uploadsFolder, folderName), true);
        }
        public ActionResult Review(string id)
        {
            var model = _productReViewRepository.GetProductReviewViewModels(id);
            foreach (var item in model)
            {
                item.Customer = _accountRepository.GetCustomerViewModel(item.CustomerId);
                item.Product = _productRepository.GetProductViewModelById(item.ProductId);
            }
            return View(model);
        }
        [HttpPost]
        public bool DeleteReview(string id)
        {
            try
            {
                var productRv = _productReViewRepository.All.FirstOrDefault(p => p.Id == id);
                _productReViewRepository.Delete(productRv);
                _productReViewRepository.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
