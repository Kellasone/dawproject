﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkIT.Models
{
    public class SavedBookmarks
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "User Id is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Bookmark Id is required")]
        public int BookmarkId { get; set; }
		[Required(ErrorMessage = "Category Id is required")]
		public int CategoryId { get; set; }


		public virtual ApplicationUser User { get; set; }
	}
}