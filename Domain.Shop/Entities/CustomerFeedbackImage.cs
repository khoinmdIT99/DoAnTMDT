using Infrastructure.Database.Dto;
using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Shop.Entities
{
	[Table("CUSTOMER_FEEDBACK_IMAGE")]
	public class CustomerFeedbackImage : BaseAttachment
	{
		public CustomerFeedbackImage(string CustomerFeedbackId, UploadFileModel item) : base(item)
		{
			this.CustomerFeedbackId = CustomerFeedbackId;
		}

		public CustomerFeedbackImage() { }

		[Column("CUSTOMER_FEEDBACK_ID")]
		public string CustomerFeedbackId { get; set; }
	}
}
