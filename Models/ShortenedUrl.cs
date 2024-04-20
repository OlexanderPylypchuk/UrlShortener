using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models
{
	public class ShortenedUrl
	{
		[Key]
		public string Code { get; set; }
		[Required]
		public string LongUrl { get; set; }
		[ValidateNever]
		public string ShortUrl { get; set; }
		public DateTime CreatedDate { get; set; }
		[ForeignKey(nameof(CreatedBy))]
		public string UserId { get; set; }
		[ValidateNever]
		public ApplicationUser CreatedBy { get; set; }
	}
}
