using System;
using System.Collections.Generic;
using System.Linq;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.WPF.ViewModels;
using BudgetPlanner8.WPF.ViewModels.Base;

namespace BudgetPlanner8.WPF.ViewModels.Filter
{
    public class FormFilterViewModel : ViewModelBase
    {
        // Filtervärden
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public string? FilterDescription { get; set; }
        public decimal? FilterAmount { get; set; }
        public Category? FilterCategory { get; set; }
        public Recurrence? FilterRecurrence { get; set; }
        public Month? FilterMonth { get; set; }

        // Checkboxar – vilka filter som är aktiva
        private bool _filterByStartDate;
        public bool FilterByStartDate { get => _filterByStartDate; set { _filterByStartDate = value; RaisePropertyChanged(); } }

        private bool _filterByEndDate;
        public bool FilterByEndDate { get => _filterByEndDate; set { _filterByEndDate = value; RaisePropertyChanged(); } }

        private bool _filterByDescription;
        public bool FilterByDescription { get => _filterByDescription; set { _filterByDescription = value; RaisePropertyChanged(); } }

        private bool _filterByAmount;
        public bool FilterByAmount { get => _filterByAmount; set { _filterByAmount = value; RaisePropertyChanged(); } }

        private bool _filterByCategory;
        public bool FilterByCategory { get => _filterByCategory; set { _filterByCategory = value; RaisePropertyChanged(); } }

        private bool _filterByRecurrence;
        public bool FilterByRecurrence { get => _filterByRecurrence; set { _filterByRecurrence = value; RaisePropertyChanged(); } }

        private bool _filterByMonth;
        public bool FilterByMonth { get => _filterByMonth; set { _filterByMonth = value; RaisePropertyChanged(); } }

        //public bool Matches(TransactionItemsViewModel vm)
        //{
        //    //bool startDateMatch = !FilterByStartDate || vm.StartDate.Date == FilterStartDate?.Date;
        //    //bool endDateMatch = !FilterByEndDate || vm.EndDate?.Date == FilterEndDate?.Date;
        //    bool startDateMatch = !FilterByStartDate || (FilterStartDate.HasValue && vm.StartDate.Date == FilterStartDate.Value.Date);
        //    bool endDateMatch = !FilterByEndDate || (!vm.EndDate.HasValue || FilterEndDate.HasValue && vm.EndDate.Value.Date == FilterEndDate.Value.Date);

        //    bool descriptionMatch = !FilterByDescription || (vm.Description?.Contains(FilterDescription ?? "", StringComparison.InvariantCultureIgnoreCase) ?? false);
        //    bool amountMatch = !FilterByAmount || vm.NetAmount == FilterAmount;
        //    bool categoryMatch = !FilterByCategory || (vm.Category?.Id == FilterCategory?.Id);
        //    bool recurrenceMatch = !FilterByRecurrence || vm.Recurrence == FilterRecurrence;
        //    bool monthMatch = !FilterByMonth || vm.Month == FilterMonth;

        //    return startDateMatch && endDateMatch && descriptionMatch && amountMatch
        //           && categoryMatch && recurrenceMatch && monthMatch;
        //}

        public bool Matches(TransactionItemsViewModel vm)
        {
            bool startDateMatch = !FilterByStartDate || (FilterStartDate.HasValue && vm.StartDate.Date >= FilterStartDate.Value.Date);
            bool endDateMatch = !FilterByEndDate ||
                                (vm.EndDate.HasValue && FilterEndDate.HasValue && vm.EndDate.Value.Date <= FilterEndDate.Value.Date) ||
                                !vm.EndDate.HasValue;

            bool descriptionMatch = !FilterByDescription || (vm.Description?.Contains(FilterDescription ?? "", StringComparison.InvariantCultureIgnoreCase) ?? false);
            bool amountMatch = !FilterByAmount || vm.NetAmount == FilterAmount;
            bool categoryMatch = !FilterByCategory || (vm.Category?.Id == FilterCategory?.Id);
            bool recurrenceMatch = !FilterByRecurrence || vm.Recurrence == FilterRecurrence;
            bool monthMatch = !FilterByMonth || vm.Month == FilterMonth;

            return startDateMatch && endDateMatch && descriptionMatch && amountMatch
                   && categoryMatch && recurrenceMatch && monthMatch;
        }

    }
}
