using Dapper.Contrib.Extensions;
using Blog.Models.Entities;

namespace Blog.Models
{
    [Table("Tag")]
    public class Tag : Base
    {
        public string Name { get; set; }        

        public string Slug { get; set; }

        [Write(false)]
        public List<Post> Posts { get; set; }
    }
}