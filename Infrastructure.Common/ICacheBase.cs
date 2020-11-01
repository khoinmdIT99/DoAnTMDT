using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common
{
	public interface ICacheBase
	{
		T Get<T>(string key);
		void Set<T>(string key, T value);
		void Remove(string key);
	}
}
