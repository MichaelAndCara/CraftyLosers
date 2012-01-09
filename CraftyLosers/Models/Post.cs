using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CraftyLosers.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserId { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}