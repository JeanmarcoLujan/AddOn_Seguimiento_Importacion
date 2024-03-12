using System.Collections.Generic;
using System.Xml.Serialization;

namespace sapping.Entities
{
	[XmlRoot(ElementName = "Cell")]
	public class Cell
	{

		[XmlElement(ElementName = "ColumnUid")]
		public string ColumnUid { get; set; }

		[XmlElement(ElementName = "Value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "Cells")]
	public class Cells
	{

		[XmlElement(ElementName = "Cell")]
		public List<Cell> Cell { get; set; }
	}

	[XmlRoot(ElementName = "Row")]
	public class Row
	{

		[XmlElement(ElementName = "Cells")]
		public Cells Cells { get; set; }
	}

	[XmlRoot(ElementName = "Rows")]
	public class Rows
	{

		[XmlElement(ElementName = "Row")]
		public Row Row { get; set; }
	}

	[XmlRoot(ElementName = "DataTable")]
	public class DataTable
	{

		[XmlElement(ElementName = "Rows")]
		public Rows Rows { get; set; }

		[XmlAttribute(AttributeName = "Uid")]
		public string Uid { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

}
