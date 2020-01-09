using System;
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
        [Required]
        public string UserId { get; set; }
        [Required]
        public int BookmarkId { get; set; }
		[Required]
		public int CategoryId { get; set; }


		public virtual ApplicationUser User { get; set; }
	}
}