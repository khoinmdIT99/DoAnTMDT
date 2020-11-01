using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database.DynamicLinq
{
	public class DatatableRequest
	{
		public int draw { get; set; }
		public Column[] columns { get; set; }
		public Order[] order { get; set; }
		public int start { get; set; }
		public int length { get; set; }
		public CommonSearch search { get; set; }
	}

	public class CommonSearch
	{
		public string value { get; set; }
		public bool regex { get; set; }
	}

	public class Column
	{
		public string data { get; set; }
		public string name { get; set; }
		public bool searchable { get; set; }
		public bool orderable { get; set; }
		public ColumnSearch search { get; set; }
	}

	public class ColumnSearch
	{
		public string value { get; set; }
		public bool regex { get; set; }
		public string field { get; set; }
		public FilterOperator Operator { get; set; }
	}

	public class Order
	{
		public int column { get; set; }
		public string dir { get; set; }
	}

}
