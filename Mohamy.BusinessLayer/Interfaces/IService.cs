using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IService<T>
    {
        public Task<List<T>> GetAllAsync();
        public Task<T> GetByIdAsync(string id);
        public Task<int> AddAsync(T t);
        public Task<int> UpdateAsync(T t);
        public Task<int> DeleteAsync(string id);
    }
}
