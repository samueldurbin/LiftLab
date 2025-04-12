using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    [QueryProperty(nameof(Meal), "Meal")] // binds the meal parameter 
    public class MealViewModel : BaseViewModel
    {
        private Meals _meal;

        public Meals Meal
        {
            get => _meal; // returns the selected meal 
            set
            {
                _meal = value; // sets the meal values to the property changes
                OnPropertyChanged();
                OnPropertyChanged(nameof(MealName));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(Calories));
                OnPropertyChanged(nameof(Recipe));
            }
        }

        public string MealName => Meal?.MealName; // read only property 
        public string Type => Meal?.Type;
        public int? Calories => Meal?.Calories;
        public string Recipe => Meal?.Recipe;
    }
}
