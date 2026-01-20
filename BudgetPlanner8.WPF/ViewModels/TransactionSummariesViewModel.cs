using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.WPF.ViewModels.Base;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionSummariesViewModel : ViewModelBase
    {
        private readonly ICollectionView transactionsView;

        public TransactionSummariesViewModel(ICollectionView transactionsView)
        {
            this.transactionsView = transactionsView;

            // Lyssna på ändringar
            transactionsView.CollectionChanged += (_, __) => RecalculateTotal();
            transactionsView.CurrentChanged += (_, __) => RecalculateTotal();

            // Initial beräkning
            RecalculateTotal();
        }

        private decimal monthlyTotal;
        public decimal MonthlyTotal
        {
            get => monthlyTotal;
            set
            {
                monthlyTotal = value;
                RaisePropertyChanged(nameof(MonthlyTotal));
            }
        }

        private void RecalculateTotal()
        {
            if (transactionsView == null) return;

            decimal total = 0;

            foreach (var item in transactionsView.Cast<TransactionItemsViewModel>())
            {
                // Endast synliga
                if (!transactionsView.Contains(item)) continue;

                total += item.Recurrence switch
                {
                    Recurrence.Monthly => item.NetAmount,
                    Recurrence.Yearly => item.NetAmount / 12,
                    _ => 0
                };
            }

            MonthlyTotal = total;
        }
    }
}
