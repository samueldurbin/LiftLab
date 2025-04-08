using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class WorkoutPlans
    {
        [Key]
        public int WorkoutPlanId { get; set; }
        public string WorkoutPlanName { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<WorkoutPlanData> WorkoutPlansData { get; set; } = new List<WorkoutPlanData>();

    }
}
