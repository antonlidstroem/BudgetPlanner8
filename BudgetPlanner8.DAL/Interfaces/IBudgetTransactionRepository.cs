using System;
using System.Collections.Generic;
using System.Text;
using BudgetPlanner8.DAL.Models;

namespace BudgetPlanner8.DAL.Interfaces
{
    public interface IBudgetTransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task AddAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task<List<Category>> GetCategoriesAsync();

    }

}
