using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mmx.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetGradeItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetLessonsItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetSentenceItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetWordItemsAsync(bool forceRefresh = false);
    }
}
