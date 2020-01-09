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
		[Required]
		public string Content { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
		public int BookmarkId { get; set; }

		public virtual Bookmark Bookmark { get; set; }
	}
}