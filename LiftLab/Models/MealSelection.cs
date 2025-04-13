using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Models
{
    public class MealSelection
    {
        public Meals Meal { get; set; }
        public bool IsSelected { get; set; }
    }
}
