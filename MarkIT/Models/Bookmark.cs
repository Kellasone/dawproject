using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MarkIT.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Image link is required")]
        public string ImageLink { get; set; }

        [Required(ErrorMessage = "Redirect link is required")]
        public string Link { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(20, ErrorMessage = "Title can not be longer than 20 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }

        public int UserId { get; set; }
    }

    public class BookmarkDBContext : DbContext
    {
        public BookmarkDBContext() : base("DBConnectionString") { }
        public DbSet<Bookmark> Bookmarks { get; set; }
    }
}