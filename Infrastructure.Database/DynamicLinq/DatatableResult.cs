using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Infrastructure.Database.DynamicLinq
{
	[KnownType("GetKnownTypes")]
	public class DatatableResult<T>
	{
		/// <summary>
		/// Represents a single page of processed data.
		/// </summary>
		public IEnumerable<T> data { get; set; }

		public int draw { get; set; }

		public int recordsTotal { get; set; }

		public int recordsFiltered { get; set; }
		public string error { get; set; }
	}
}
