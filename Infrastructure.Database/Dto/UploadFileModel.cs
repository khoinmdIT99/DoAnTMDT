using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Database.Dto
{
    public class UploadFileModel
    {
        public string Uid { get; set; }
        public DateTime? LastModified { get; set; }
        public string Name { get; set; }
        public long? Size { get; set; }
        public string Type { get; set; }
        public int Percent { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string ThumbUrl { get; set; }
        public bool IsOldFile { get; set; }
    }
}
