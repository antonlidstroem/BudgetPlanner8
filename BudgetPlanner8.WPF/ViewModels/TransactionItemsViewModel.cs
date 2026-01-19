using System;
using System.Collections.Generic;
using System.Text;
using BudgetPlanner8.DAL.Models;
using BudgetPlanner8.WPF.ViewModels.Base;

namespace BudgetPlanner8.WPF.ViewModels
{
    public class TransactionItemsViewModel : ViewModelBase
    {
        private readonly Transaction model;
        public Transaction Model => model;

        public TransactionItemsViewModel(Transaction model)
        {
            this.model = model;
        }

        public DateTime StartDate
        {
            get { return model.StartDate; }
            set { model.StartDate = value; RaisePropertyChanged(nameof(StartDate));
            }
        }
        public DateTime? EndDate
        {
            get { return model.EndDate; }
            set { model.EndDate = value; RaisePropertyChanged(nameof(EndDate));
            }
        }

        public decimal NetAmount
        {
            get { return model.NetAmount; }
            set { model.NetAmount = value; RaisePropertyChanged(nameof(NetAmount));
            }
        }

        public decimal? GrossAmount
        {
            get => model.GrossAmount;
            set { model.GrossAmount = value; RaisePropertyChanged(nameof(GrossAmount));
            }
        }

        public string? Description
        {
            get { return model.Description; }
            set { model.Description = value; RaisePropertyChanged(nameof(Description));
            }
        }

        public int CategoryId
        {
            get { return model.CategoryId; }
            set { model.CategoryId = value; RaisePropertyChanged(nameof(CategoryId)); }
        }

        public Month? Month
        {
            get { return model.Month; }
            set { model.Month = value; RaisePropertyChanged(nameof(Month));}
        }

        public decimal? Rate
        {
            get { return model.Rate; }
            set { model.Rate = value; RaisePropertyChanged(nameof(Rate));
            }
        }

        public Category? Category
        {
            get { return model.Category; }
            set { model.Category = value; model.CategoryId = value?.Id ?? 0;
                RaisePropertyChanged(nameof(Category));
            }
        }

        public Recurrence Recurrence
        {
            get { return model.Recurrence; }
            set { model.Recurrence = value; RaisePropertyChanged(nameof(Recurrence));
            }
        }

        public bool IsActive
        {
            get { return model.IsActive; }
            set { model.IsActive = value; RaisePropertyChanged(nameof(IsActive));
            }
        }

        public TransactionType Type
        {
            get { return model.Type; }
            set
            {
                if (model.Type != value)
                {
                    model.Type = value;
                    RaisePropertyChanged(nameof(Type));
                }
            }
        }
        

        public void RefreshFromModel()
        {
            RaisePropertyChanged(nameof(StartDate));
            RaisePropertyChanged(nameof(EndDate));
            RaisePropertyChanged(nameof(NetAmount));
            RaisePropertyChanged(nameof(GrossAmount));
            RaisePropertyChanged(nameof(Category));
            RaisePropertyChanged(nameof(Recurrence));
            RaisePropertyChanged(nameof(Description));
            RaisePropertyChanged(nameof(Month));
            RaisePropertyChanged(nameof(Type));
            RaisePropertyChanged(nameof(Rate));
            RaisePropertyChanged(nameof(IsActive));
        }
    }
}
