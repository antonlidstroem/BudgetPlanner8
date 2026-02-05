using System.Collections.ObjectModel;
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
                }
            }
        }

        public event Action? CategoryChanged;

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
                    CalculateDaysCount();
                }
            }
        }

        public Array RecurrenceValues { get; } = Enum.GetValues(typeof(Recurrence));
        public Array Months { get; } = Enum.GetValues(typeof(Month));

        public bool ShowGrossNetToggle => Category?.ToggleGrossNet == true;
        public bool ShowEndDate => Category?.HasEndDate == true;
        public bool ShowMonth => Recurrence == Recurrence.Yearly;
        public bool ShowDaysCount => Category?.HasEndDate == true; // För VAB

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
                    CalculateDaysCount();
                }
            }
        }

        private decimal netAmount;
        public decimal NetAmount
        {
            get => netAmount;
            set
            {
                var adjustedValue = value;

                // Justera efter kategori
                if (Category != null)
                {
                    if (Category.Type == TransactionType.Expense)
                        adjustedValue = -Math.Abs(value);
                    else
                        adjustedValue = Math.Abs(value);
                }

                if (netAmount != adjustedValue)
                {
                    netAmount = adjustedValue;
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
                    CalculateNetFromGross();
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
                category = value;
                RaisePropertyChanged(nameof(Category));
                CategoryChanged?.Invoke();

                // Sätt defaultvärden från kategori
                if (category != null)
                {
                    if (category.DefaultRate.HasValue)
                        Rate = category.DefaultRate.Value;

                    // Justera NetAmount direkt när kategori ändras
                    if (category.Type == TransactionType.Expense)
                        NetAmount = -Math.Abs(NetAmount);
                    else if (category.Type == TransactionType.Income)
                        NetAmount = Math.Abs(NetAmount);
                }

                if (!ShowEndDate)
                {
                    EndDate = null;
                    DaysCount = null;
                }

                if (!ShowMonth)
                    Month = null;

                if (!ShowGrossNetToggle)
                {
                    GrossAmount = null;
                    Rate = null;
                }

                RaisePropertyChanged(nameof(ShowGrossNetToggle));
                RaisePropertyChanged(nameof(ShowEndDate));
                RaisePropertyChanged(nameof(ShowMonth));
                RaisePropertyChanged(nameof(ShowDaysCount));
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
                    RaisePropertyChanged(nameof(ShowMonth));
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
                    CalculateNetFromGross();
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

        // DaysCount för VAB
        private int? daysCount;
        public int? DaysCount
        {
            get => daysCount;
            set
            {
                if (daysCount != value)
                {
                    daysCount = value;
                    RaisePropertyChanged(nameof(DaysCount));
                    CalculateNetFromGross();
                }
            }
        }
        #endregion

        // Beräkna antal dagar automatiskt
        private void CalculateDaysCount()
        {
            if (ShowDaysCount && EndDate.HasValue)
            {
                var days = (EndDate.Value - StartDate).Days + 1; // +1 för att inkludera startdagen
                DaysCount = days > 0 ? days : null;
            }
        }

        // Beräkna netto från brutto
        private void CalculateNetFromGross()
        {
            if (!GrossAmount.HasValue || !Rate.HasValue || Category == null)
                return;

            decimal calculatedNet = 0;

            // För VAB: Antal dagar × Dagslön × Procent
            if (Category.Name == "VAB/Sjukfrånvaro" && DaysCount.HasValue)
            {
                calculatedNet = DaysCount.Value * GrossAmount.Value * (Rate.Value / 100);
            }
            // För vanliga transaktioner med brutto/netto
            else
            {
                if (Category.AdjustmentType == AdjustmentType.Deduction)
                {
                    // T.ex. Lön: Brutto - 30% skatt = Netto
                    calculatedNet = GrossAmount.Value * (1 - Rate.Value / 100);
                }
                else if (Category.AdjustmentType == AdjustmentType.Addition)
                {
                    // Om det behövs (t.ex. moms tillkommande)
                    calculatedNet = GrossAmount.Value * (1 + Rate.Value / 100);
                }
            }

            // Sätt NetAmount utan att trigga justering igen
            netAmount = Category.Type == TransactionType.Expense
                ? -Math.Abs(calculatedNet)
                : Math.Abs(calculatedNet);

            RaisePropertyChanged(nameof(NetAmount));
        }

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
            DaysCount = null;
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
            DaysCount = transaction.DaysCount;
        }
    }
}