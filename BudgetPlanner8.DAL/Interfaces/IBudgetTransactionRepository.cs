using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetPlanner8.DAL.Models;

namespace BudgetPlanner8.DAL.Interfaces
{
    public interface IBudgetTransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task DeleteMultipleAsync(IEnumerable<Transaction> transactions); 
        Task<List<Category>> GetCategoriesAsync();
    }
}