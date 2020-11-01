using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database.Models
{
	public class RequestContext
	{
		public string UserId { get; set; }
		public DateTime RequestTime { get; set; }
	}
}
