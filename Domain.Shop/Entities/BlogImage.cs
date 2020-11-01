using Infrastructure.Database.Dto;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
    [Table("BLOG_IMAGE")]
    public class BlogImage : BaseAttachment
    {
        public BlogImage(string BlogId, UploadFileModel item) : base(item)
        {
            this.BlogId = BlogId;
        }
        public BlogImage() { }

        [Column("BLOG_ID")]
        [MaxLength(50)]
        public string BlogId { get; set; }
    }
}