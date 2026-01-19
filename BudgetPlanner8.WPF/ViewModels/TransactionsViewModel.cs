using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using BudgetPlanner8.DAL.Repositories;
using BudgetPlanner8.DAL.Data;
using BudgetPlanner8.DAL.Interfaces;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.WPF.Commands;
using BudgetPlanner8.WPF.ViewModels;
using BudgetPlanner8.WPF.ViewModels.Base;
using BudgetPlanner8.WPF.ViewModels.Filter;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        #region Properties
        // PROPERTIES
        private readonly IBudgetTransactionRepository repository;

        // Transaktioner och kategorier
        public ObservableCollection<TransactionItemsViewModel> Transactions { get; } = new();
        public ObservableCollection<Category> Categories { get; } = new();

        // Läser in ViewModels
        public TransactionsFormViewModel Form { get; } = new();
        public FormFilterViewModel FormFilter { get; } = new();
        public ListViewFilterViewModel ListViewFilter { get; } = new();


        // ListView
        public ICollectionView TransactionsView { get; }

        // Commands
        public DelegateCommand AddCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand UpdateCommand { get; }
        public DelegateCommand CancelEditCommand { get; }
        public DelegateCommand ClearFilterCommand { get; }


        // Edit mode
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

                    Form.SelectedTransaction = selectedTransaction;

                    UpdateCommand.RaiseCanExecuteChanged();
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }


        #endregion

        #region Constructor
        // KONSTRUKTOR
        public TransactionsViewModel(IBudgetTransactionRepository? repo = null)
        {
            var factory = new BudgetDbContextFactory();
            repository = repo ?? new BudgetTransactionRepository(factory.CreateDbContext(null));
            TransactionsView = CollectionViewSource.GetDefaultView(Transactions);
            //FormFilter.Filter = object => Filter.Matches((TransactionItemsViewModel)o);

            AddCommand = new DelegateCommand(AddTransaction);
            UpdateCommand = new DelegateCommand(UpdateTransaction, _ => SelectedTransaction != null);
            DeleteCommand = new DelegateCommand(DeleteTransaction, _ => SelectedTransaction != null);
            CancelEditCommand = new DelegateCommand(_ =>
            {
                SelectedTransaction = null;
                Form.Clear();
            });

            //ClearFilterCommand = new DelegateCommand(_ => { /* ... */ });

             _ = LoadAsync();
        }
        #endregion

        private async Task LoadAsync()
        {
            var categories = await repository.GetCategoriesAsync();
            foreach (var c in categories) Categories.Add(c);

            var transactions = await repository.GetAllAsync();
            foreach (var t in transactions) Transactions.Add(new TransactionItemsViewModel(t));
        }


        #region CRUD-metoder
        private async void AddTransaction(object? _)
        {
            var t = new Transaction
            {
                StartDate = Form.StartDate,
                EndDate = Form.EndDate,
                NetAmount = Form.NetAmount,
                GrossAmount = Form.GrossAmount,
                Description = Form.Description,
                CategoryId = Form.Category?.Id ?? 0,
                Recurrence = Form.Recurrence,
                Month = Form.Month,
                Rate = Form.Rate,
                Type = Form.Type,
                IsActive = Form.IsActive
            };

            await repository.AddAsync(t);
            var vm = new TransactionItemsViewModel(t);
            Transactions.Add(vm);
            SelectedTransaction = vm;
        }

        private async void UpdateTransaction(object? _)
        {
            if (SelectedTransaction == null) return;

            var t = SelectedTransaction.Model;
            t.StartDate = Form.StartDate;
            t.EndDate = Form.EndDate;
            t.NetAmount = Form.NetAmount;
            t.GrossAmount = Form.GrossAmount;
            t.Description = Form.Description;
            t.CategoryId = Form.Category?.Id ?? 0;
            t.Recurrence = Form.Recurrence;
            t.Month = Form.Month;
            t.Rate = Form.Rate;
            t.Type = Form.Type;
            t.IsActive = Form.IsActive;

            await repository.UpdateAsync(t);
            SelectedTransaction.RefreshFromModel();
        }

        private async void DeleteTransaction(object? _)
        {
            if (SelectedTransaction == null) return;

            await repository.DeleteAsync(SelectedTransaction.Model);
            Transactions.Remove(SelectedTransaction);
            SelectedTransaction = null;
        }
        #endregion

    }
}
