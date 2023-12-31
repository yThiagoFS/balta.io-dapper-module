using Blog.Models.Entities;
using Dapper.Contrib.Extensions;

namespace Blog.Models
{
    [Table("Post")]
    public class Post : Base
    {
        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Body { get; set; }

        public string Slug { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public List<Tag> Tags { get; set; }
    }
}