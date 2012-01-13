using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CraftyLosers.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }

        [DisplayName("Comment Date")]
        public DateTime CommentDateTime { get; set; }

        [Required]
        [DisplayName("Comment")]
        public string CommentContent { get; set; }

        public int UserId { get; set; }

        public Post Post { get; set; }
        public User User { get; set; }
    }
}