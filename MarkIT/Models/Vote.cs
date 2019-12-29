using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MarkIT.Models
{
	public class Vote
	{
		[Key]
		public int VoteId { get; set; }
		[Required]
		public string UserId { get; set; }
		[Required]
		public int BookmarkId { get; set; }

		public virtual Bookmark Bookmark { get; set; }
	}
}