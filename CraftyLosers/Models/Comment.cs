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

        public int LineCount
        {
            get
            {
                int count = 1;
                int charLineCount = 1;

                if (!string.IsNullOrEmpty(CommentContent))
                {
                    //line count based on line breaks
                    count = 1;
                    int start = 0;
                    while ((start = CommentContent.IndexOf('\n', start)) != -1)
                    {
                        count++;
                        start++;
                    }

                    //char count lines (+1)
                    charLineCount = CommentContent.Length / 100 + 1;
                }

                return Math.Max(5, Math.Max(count, charLineCount));
            }
        }
    }
}