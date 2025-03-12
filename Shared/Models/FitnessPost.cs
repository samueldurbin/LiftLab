using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class FitnessPost // contains all of the fitnessposts
    {
        public int FitnessPostId { get; set; } // Changed to FitnessPostId to make the foreign keys easier to understand
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
        public int? WorkoutPlanId { get; set; } // set the nullable as adding a plan to a post is not required

        [ForeignKey("WorkoutPlanId")] // foreign key from WorkoutPlans table
        public WorkoutPlans? WorkoutPlan { get; set; } // also set to nullable as not required

    }
}
