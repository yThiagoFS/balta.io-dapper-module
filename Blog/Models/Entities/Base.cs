using Dapper.Contrib.Extensions;

namespace Blog.Models.Entities 
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }
    }
}