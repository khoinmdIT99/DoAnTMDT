using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Web
{
	public class AppSettings
	{
		private readonly IConfiguration _configuration;

		public AppSettings(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string TempFolderUpload => _configuration["TempFolderUpload"];

        public string DataFolderUpload => _configuration["DataFolderUpload"];
    }
}
