﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkIT.Models
{
	public class Category
	{

		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "UserId is required")]
		public string UserId { get; set; }
		[Required(ErrorMessage = "Title is required")]
		public string Title { get; set; }
	}
}