using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using BudgetPlanner8.DAL.Interfaces;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.DAL.Repositories;
using BudgetPlanner8.WPF.ViewModels.Base;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionListViewModel : ViewModelBase
    {
        private readonly IBudgetTransactionRepository repository;

        public ObservableCollection<TransactionItemsViewModel> Transactions { get; } = new();
        public ICollectionView TransactionsView { get; }

        private TransactionItemsViewModel? selectedTransaction;
        public TransactionItemsViewModel? SelectedTransaction
        {
            get => selectedTransaction;
            set
            {
                if (selectedTransaction != value)
                {
                    selectedTransaction = value;
                    RaisePropertyChanged(nameof(SelectedTransaction));
                }
            }
        }

        public TransactionListViewModel(IBudgetTransactionRepository? repo = null)
        {
            repository = repo ?? new BudgetTransactionRepository(new DAL.Data.BudgetDbContextFactory().CreateDbContext(new string[0]));
            TransactionsView = CollectionViewSource.GetDefaultView(Transactions);

            _ = LoadTransactionsAsync();
        }

        private async Task LoadTransactionsAsync()
        {
            var items = await repository.GetAllAsync();
            Transactions.Clear();
            foreach (var t in items)
            {
                Transactions.Add(new TransactionItemsViewModel(t));
            }
        }
    }
}
