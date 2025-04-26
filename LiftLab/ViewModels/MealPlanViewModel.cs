using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.ViewModels
{
    [QueryProperty(nameof(MealPlan), "MealPlan")]
    public class MealPlanViewModel : BaseViewModel
    {
        private MealPlans _mealPlan;

        public MealPlans MealPlan
        {
            get => _mealPlan;
            set
            {
                _mealPlan = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MealPlanName));
                OnPropertyChanged(nameof(CreatedAt));
                OnPropertyChanged(nameof(Meals));
            }
        }

        public string MealPlanName => MealPlan?.MealPlanName;
        public DateTime CreatedAt => MealPlan?.CreatedAt ?? DateTime.MinValue;
        public ICollection<Meals> Meals => MealPlan?.Meals ?? new List<Meals>();
    }
}
