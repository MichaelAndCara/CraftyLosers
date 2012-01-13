using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Title")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Post Title must not exceed 50.")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Post")]
        public string PostContent { get; set; }

        [DisplayName("Post Date")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Modified Date")]
        public DateTime DateModified { get; set; }
        
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}