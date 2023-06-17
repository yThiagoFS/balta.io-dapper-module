using Blog.Models.Entities;
using Dapper.Contrib.Extensions;

namespace Blog.Models
{
    [Table("[Role]")]
    public class Role : Base
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }
}