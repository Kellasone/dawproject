using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkIT.Models
{
	public class Comment
	{
		[Key]
		public int CommentId { get; set; }
		[Required(ErrorMessage = "Content is required")]
		public string Content { get; set; }
		[Required(ErrorMessage = "Username is required")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Bookmark Id is required")]
		public int BookmarkId { get; set; }

		public virtual Bookmark Bookmark { get; set; }
	}
}