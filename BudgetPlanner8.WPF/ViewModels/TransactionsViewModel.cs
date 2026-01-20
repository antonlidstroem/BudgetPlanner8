using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using BudgetPlanner8.DAL.Data;
using BudgetPlanner8.DAL.Interfaces;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.DAL.Repositories;
using BudgetPlanner8.WPF.Commands;
using BudgetPlanner8.WPF.ViewModels;
using BudgetPlanner8.WPF.ViewModels.Base;
using BudgetPlanner8.WPF.ViewModels.Filter;
using BudgetPlanner8.WPF.Views;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionsViewModel : ViewModelBase
    {
        #region Properties
        // PROPERTIES
        private readonly IBudgetTransactionRepository repository;

        // Transaktioner och kategorier
        public ObservableCollection<TransactionItemsViewModel> Transactions { get; } = new();

        private ObservableCollection<Category> categories = new();
        public ObservableCollection<Category> Categories
        {
            get => categories;
            set
            {
                categories = value;
                RaisePropertyChanged(nameof(Categories));
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        // Läser in ViewModels
        public TransactionsFormViewModel Form { get; } = new();
        public FormFilterViewModel FormFilter { get; } = new();
        public ListViewFilterViewModel ListViewFilter { get; } = new();
        public TransactionSummariesViewModel SummariesVM { get; }

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
                    Form.LoadFromTransaction(selectedTransaction, categories);

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

            TransactionsView.Filter = t =>
            {
                if (t is not TransactionItemsViewModel vm) return false;
                return FormFilter.Matches(vm);
            };

            // När en checkbox ändras i FormFilter
            FormFilter.PropertyChanged += (_, __) => TransactionsView.Refresh();

            // När ett formulärfält ändras, uppdatera filtervärdet och refresh
            Form.PropertyChanged += (_, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(Form.StartDate): FormFilter.FilterStartDate = Form.StartDate; break;
                    case nameof(Form.EndDate): FormFilter.FilterEndDate = Form.EndDate; break;
                    case nameof(Form.Description): FormFilter.FilterDescription = Form.Description; break;
                    case nameof(Form.NetAmount): FormFilter.FilterAmount = Form.NetAmount; break;
                    case nameof(Form.Category): FormFilter.FilterCategory = Form.Category; break;
                    case nameof(Form.Recurrence): FormFilter.FilterRecurrence = Form.Recurrence; break;
                    case nameof(Form.Month): FormFilter.FilterMonth = Form.Month; break;
                }
                TransactionsView.Refresh();
            };


            AddCommand = new DelegateCommand(async param => await AddTransaction(param), _ => Form.Category != null);
            UpdateCommand = new DelegateCommand(async param => await UpdateTransaction(param), _ => SelectedTransaction != null);
            DeleteCommand = new DelegateCommand(async param => await DeleteTransaction(param), _ => SelectedTransaction != null);
            CancelEditCommand = new DelegateCommand(_ =>
            {
                SelectedTransaction = null;
                Form.Clear();
            });
            ClearFilterCommand = new DelegateCommand(_ =>
            {
                FormFilter.FilterByStartDate = false;
                FormFilter.FilterByEndDate = false;
                FormFilter.FilterByDescription = false;
                FormFilter.FilterByAmount = false;
                FormFilter.FilterByCategory = false;
                FormFilter.FilterByRecurrence = false;
                FormFilter.FilterByMonth = false;

                TransactionsView.Refresh();
            });

            Form.CategoryChanged += () => AddCommand.RaiseCanExecuteChanged();

            SummariesVM = new TransactionSummariesViewModel(TransactionsView);

            _ = LoadAsync();
   

        }
        #endregion



        private async Task LoadAsync()
        {
            var categoriesFromDb = await repository.GetCategoriesAsync();

            Categories.Clear();
            foreach (var c in categoriesFromDb)
                Categories.Add(c);

            // Fyll transaktioner
            Transactions.Clear();
            var transactions = await repository.GetAllAsync();
            foreach (var t in transactions)
                Transactions.Add(new TransactionItemsViewModel(t));

            TransactionsView.MoveCurrentToFirst();
        }




        #region CRUD-metoder
        private async Task AddTransaction(object? _)
        {
            if (Form.Category == null)
                return;

            var t = new Transaction
            {
                StartDate = Form.StartDate,
                EndDate = Form.EndDate,
                NetAmount = Form.NetAmount,
                GrossAmount = Form.GrossAmount,
                Description = Form.Description,
                CategoryId = Form.Category.Id,
                Recurrence = Form.Recurrence,
                Month = Form.Month,
                Rate = Form.Rate,
                Type = Form.Type,
                IsActive = Form.IsActive
            };

            await repository.AddAsync(t);
            var vm = new TransactionItemsViewModel(t);
            Transactions.Add(vm);
            Form.Clear();
            SelectedTransaction = vm;
        }

        private async Task UpdateTransaction(object? _)
        {
            if (SelectedTransaction == null) return;

            var t = SelectedTransaction.Model;
            t.StartDate = Form.StartDate;
            t.EndDate = Form.EndDate;

            // Korrigera NetAmount efter kategori
            var netAmount = Form.NetAmount;
            if (Form.Category != null)
            {
                if (Form.Category.Type == TransactionType.Expense)
                    netAmount = -Math.Abs(netAmount);
                else
                    netAmount = Math.Abs(netAmount);
            }
            t.NetAmount = netAmount;

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


        private async Task DeleteTransaction(object? _)
        {
            if (SelectedTransaction == null) return;

            await repository.DeleteAsync(SelectedTransaction.Model);
            Transactions.Remove(SelectedTransaction);
            SelectedTransaction = null;
        }
        #endregion



    }
}
