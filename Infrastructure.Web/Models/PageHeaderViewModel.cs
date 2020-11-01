using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Web.Models
{
	public class PageHeaderViewModel
	{
		public string Title { get; set; }
		public List<PageHeaderPath> Path { get; set; }
	}

	public class PageHeaderPath
	{
		public string Name { get; set; }
		public string Controller { get; set; }
	}
}
