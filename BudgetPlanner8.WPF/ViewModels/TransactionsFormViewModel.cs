using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Text;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.WPF.ViewModels.Base;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionsFormViewModel : ViewModelBase
    {
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
                    //LoadFromTransaction(selectedTransaction, categories);
                }
            }
        }

        public ObservableCollection<Category> Categories { get; } = new();

        #region Properties
        private DateTime startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    RaisePropertyChanged(nameof(StartDate));
                }
            }
        }

        public Array RecurrenceValues { get; } =
        Enum.GetValues(typeof(Recurrence));

        public Array Months { get; } =
        Enum.GetValues(typeof(Month));



        private DateTime? endDate;
        public DateTime? EndDate
        {
            get => endDate;
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    RaisePropertyChanged(nameof(EndDate));
                }
            }
        }

        private decimal netAmount;
        public decimal NetAmount
        {
            get => netAmount;
            set
            {
                if (netAmount != value)
                {
                    netAmount = value;
                    RaisePropertyChanged(nameof(NetAmount));
                }
            }
        }

        private decimal? grossAmount;
        public decimal? GrossAmount
        {
            get => grossAmount;
            set
            {
                if (grossAmount != value)
                {
                    grossAmount = value;
                    RaisePropertyChanged(nameof(GrossAmount));
                }
            }
        }

        private string? description;
        public string? Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        private Category? category;
        public Category? Category
        {
            get => category;
            set
            {
                if (category != value)
                {
                    category = value;
                    RaisePropertyChanged(nameof(Category));
                }
            }
        }

        private Recurrence recurrence = Recurrence.OneTime;
        public Recurrence Recurrence
        {
            get => recurrence;
            set
            {
                if (recurrence != value)
                {
                    recurrence = value;
                    RaisePropertyChanged(nameof(Recurrence));
                }
            }
        }

        private Month? month;
        public Month? Month
        {
            get => month;
            set
            {
                if (month != value)
                {
                    month = value;
                    RaisePropertyChanged(nameof(Month));
                }
            }
        }

        private decimal? rate;
        public decimal? Rate
        {
            get => rate;
            set
            {
                if (rate != value)
                {
                    rate = value;
                    RaisePropertyChanged(nameof(Rate));
                }
            }
        }

        private TransactionType type = TransactionType.Expense;
        public TransactionType Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    RaisePropertyChanged(nameof(Type));
                }
            }
        }

        private bool isActive = true;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    RaisePropertyChanged(nameof(IsActive));
                }
            }
        }
        #endregion
        public void Clear()
        {
            SelectedTransaction = null;
            StartDate = DateTime.Today;
            EndDate = null;
            NetAmount = 0;
            GrossAmount = null;
            Description = null;
            Category = null;
            Recurrence = Recurrence.OneTime;
            Month = null;
            Rate = null;
            Type = TransactionType.Expense;
            IsActive = true;
            RaiseAllProperties();
        }

        public void LoadFromTransaction(TransactionItemsViewModel? transaction, ObservableCollection<Category> categories)
        {
            if (transaction == null) return;

            StartDate = transaction.StartDate;
            EndDate = transaction.EndDate;
            NetAmount = transaction.NetAmount;
            GrossAmount = transaction.GrossAmount;
            Description = transaction.Description;

            Category = categories.FirstOrDefault(c => c.Id == transaction.Category?.Id);

            Recurrence = transaction.Recurrence;
            Month = transaction.Month;
            Rate = transaction.Rate;
            Type = transaction.Type;
            IsActive = transaction.IsActive;

            RaiseAllProperties();
        }



        private void RaiseAllProperties()
        {
            RaisePropertyChanged(nameof(StartDate));
            RaisePropertyChanged(nameof(EndDate));
            RaisePropertyChanged(nameof(NetAmount));
            RaisePropertyChanged(nameof(GrossAmount));
            RaisePropertyChanged(nameof(Description));
            RaisePropertyChanged(nameof(Category));
            RaisePropertyChanged(nameof(Recurrence));
            RaisePropertyChanged(nameof(Month));
            RaisePropertyChanged(nameof(Rate));
            RaisePropertyChanged(nameof(Type));
            RaisePropertyChanged(nameof(IsActive));
        }

    }
}
