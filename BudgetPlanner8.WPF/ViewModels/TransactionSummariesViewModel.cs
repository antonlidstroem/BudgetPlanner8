using System.Collections.Specialized;
using System.ComponentModel;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.WPF.ViewModels;
using BudgetPlanner8.WPF.ViewModels.Base;

public class TransactionSummariesViewModel : ViewModelBase
{
    private readonly ICollectionView transactionsView;

    public TransactionSummariesViewModel(ICollectionView transactionsView)
    {
        this.transactionsView = transactionsView;

        // Lyssna på ändringar i collectionen (lägg till / ta bort)
        if (transactionsView is INotifyCollectionChanged incc)
            incc.CollectionChanged += (_, e) =>
            {
                if (e.NewItems != null)
                    foreach (TransactionItemsViewModel item in e.NewItems)
                        item.PropertyChanged += Item_PropertyChanged;

                if (e.OldItems != null)
                    foreach (TransactionItemsViewModel item in e.OldItems)
                        item.PropertyChanged -= Item_PropertyChanged;

                RecalculateTotal();
            };

        // Lägg till PropertyChanged på befintliga objekt
        foreach (TransactionItemsViewModel item in transactionsView.Cast<TransactionItemsViewModel>())
            item.PropertyChanged += Item_PropertyChanged;

        // Initial beräkning
        RecalculateTotal();
    }

    private void Item_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TransactionItemsViewModel.NetAmount) ||
            e.PropertyName == nameof(TransactionItemsViewModel.Type) ||
            e.PropertyName == nameof(TransactionItemsViewModel.Recurrence))
        {
            RecalculateTotal();
        }
    }

    private decimal monthlyIncome;
    public decimal MonthlyIncome
    {
        get => monthlyIncome;
        set { monthlyIncome = value; RaisePropertyChanged(); }
    }

    private decimal monthlyExpenses;
    public decimal MonthlyExpenses
    {
        get => monthlyExpenses;
        set { monthlyExpenses = value; RaisePropertyChanged(); }
    }

    private decimal monthlyTotal;
    public decimal MonthlyTotal
    {
        get => monthlyTotal;
        set { monthlyTotal = value; RaisePropertyChanged(); }
    }

    private void RecalculateTotal()
    {
        decimal income = 0;
        decimal expenses = 0;

        foreach (var item in transactionsView.Cast<TransactionItemsViewModel>())
        {
            // Beräkna månadsbelopp
            decimal monthlyAmount = item.Recurrence switch
            {
                Recurrence.Monthly => item.NetAmount,
                Recurrence.Yearly => item.NetAmount / 12,
                _ => 0
            };

            if (monthlyAmount >= 0)
                income += monthlyAmount;
            else
                expenses += -monthlyAmount; // ta absolutvärdet för utgifter
        }

        MonthlyIncome = income;
        MonthlyExpenses = expenses;
        MonthlyTotal = income - expenses;
    }
}
