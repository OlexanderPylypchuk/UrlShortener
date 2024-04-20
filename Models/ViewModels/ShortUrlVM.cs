using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models.ViewModels
{
	public class ShortUrlVM
	{
		public UrlRequest UrlRequest { get; set; }
		[ValidateNever]
		public List<ShortenedUrl> ShortUrls {  get; set; }
	}
}
