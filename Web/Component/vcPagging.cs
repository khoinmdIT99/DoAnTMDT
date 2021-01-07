using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Web.HelperTool;
using Microsoft.AspNetCore.Mvc;

namespace Web.Component
{
    public class vcPagging : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int totals, int pageSize, int page, string url, int maxPage)
        {
            int totalPages = (int)Math.Ceiling((double)totals / pageSize);
            int pageFirst = 1;
            int pageLast = totalPages;

            int pageBack = page - 1;
            if (pageBack == 0)
            {
                pageBack = 1;
            }

            int pageNext = page + 1;
            if (pageNext > pageLast)
            {
                pageNext = pageLast;
            }

            string pageFirstUrl = string.Format(url, pageFirst);
            string pageLastUrl = string.Format(url, pageLast);
            string pageBackUrl = string.Format(url, pageBack);
            string pageNextUrl = string.Format(url, pageNext);

            Dictionary<int, string> pageNumbers = new Dictionary<int, string>();
            for (int i = pageFirst; i <= pageLast; i++)
            {
                pageNumbers.Add(i, string.Format(url, i));
            }

            PaggingDatasource dataSource = new PaggingDatasource
            {
                PageFirstUrl = pageFirstUrl,
                PageLastUrl = pageLastUrl,
                PageBackUrl = pageBackUrl,
                PageNextUrl = pageNextUrl,
                page = page,
                PageNumbers = pageNumbers
            };
            return await Task.Run(()
                => View(dataSource));
        }
    }
}
