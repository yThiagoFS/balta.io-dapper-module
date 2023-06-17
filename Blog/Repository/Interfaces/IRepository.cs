using Blog.Models.Entities;  

namespace Blog.Repository.Interfaces 
{
    public interface IRepository<T> where T : Base
    {
        public IEnumerable<T> Get();

        public T Get(int id);

        public void Create(T data);

        public void Update(T data);

        public void Delete(T data);

        public void Delete(int id);
    }
}