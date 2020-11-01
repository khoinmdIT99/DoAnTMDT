using Infrastructure.Database.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.Database.Entities
{
	public class BaseAttachment : BaseEntity
	{
		public BaseAttachment() { }
		public BaseAttachment(UploadFileModel item)
		{
			this.Id = item.Uid;
			this.LastUpdateAt = item.LastModified;
			this.FileName = item.Name;
			this.FileSize = item.Size;
			this.FileType = item.Type;
			this.Url = item.Url;
			this.ThumbUrl = item.ThumbUrl;
		}
		[Key]
		[MaxLength(50)]
		[Column("ID")]
		public string Id { get; set; }
		[Column("URL")]
		public string Url { get; set; }
		[Column("THUMB_URL")]
		public string ThumbUrl { get; set; }
		[Column("FILE_NAME")]
		public string FileName { get; set; }
		[Column("FILE_SIZE")]
		public long? FileSize { get; set; }
		[Column("FILE_TYPE")]
		public string FileType { get; set; }
	}
}
