using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Blog.Models.Entities;
using Blog.Repository.Interfaces;

namespace Blog.Repository
{
    public class Repository<T> : IRepository<T> where T : Base
    {
        private readonly SqlConnection _connection;

        public Repository(SqlConnection connection) => _connection = connection;

        public IEnumerable<T> Get()  => _connection.GetAll<T>();

        public T Get(int id) => _connection.Get<T>(id);

        public virtual void Create(T data) => _connection.Insert(data);

        public virtual void Update(T data) 
        {
            if(data.Id != 0)
            {
                _connection.Update(data);   
            }
        }

        public virtual void Delete(T data)
        {
            if(data.Id != 0)
                _connection.Delete<T>(data);
        }

        public virtual void Delete(int id)
        {
            if(id != 0)
            {
                var data = _connection.Get<T>(id);
                _connection.Delete<T>(data);
            }
        } 
    }
}