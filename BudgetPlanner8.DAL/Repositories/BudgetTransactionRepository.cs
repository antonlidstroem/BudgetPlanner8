using System;
using System.Collections.Generic;
using System.Text;
using BudgetPlanner8.DAL.Data;
using BudgetPlanner8.DAL.Interfaces;
using BudgetPlanner8.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner8.DAL.Repositories
{
    public class BudgetTransactionRepository : IBudgetTransactionRepository
    {
        private readonly BudgetDbContext context;

        public BudgetTransactionRepository(BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await context.Transactions
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await context.Categories.ToListAsync();
        }
        public async Task UpdateAsync(Transaction transaction)
        {


            var existing = await context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == transaction.Id);

            if (existing == null)
                throw new KeyNotFoundException($"Transaction with Id {transaction.Id} not found.");


                // Uppdatera alla fält
                existing.StartDate = transaction.StartDate;
                existing.EndDate = transaction.EndDate;
                existing.NetAmount = transaction.NetAmount;
                existing.GrossAmount = transaction.GrossAmount;
                existing.Description = transaction.Description;
                existing.CategoryId = transaction.CategoryId;
                existing.Recurrence = transaction.Recurrence;
                existing.IsActive = transaction.IsActive;
                existing.Month = transaction.Month;
                existing.Rate = transaction.Rate;
                existing.Type = transaction.Type;

                await context.SaveChangesAsync();
            
        }

    }
}
