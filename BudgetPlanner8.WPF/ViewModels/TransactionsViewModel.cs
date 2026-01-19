using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using BudgetPlanner8.DAL.Interfaces;
using BudgetPlanner8.DAL.Models;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionsViewModel
    {
        private readonly IBudgetTransactionRepository repository;

        public ObservableCollection<TransactionItemsViewModel> Transactions { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();
    }
}
