using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CommunityPost
    {
        [Key]
        public int CommunityPostId { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string? Caption { get; set; }

        public int CommentCount { get; set; }

        public int LikeCount { get; set; }

        public int? WorkoutPlanId { get; set; } // optional workoutplan additon

        public int? MealId { get; set; } // optional meal

        public int? MealPlanId { get; set; } // optional mealplan addition

        public DateTime CreatedDate { get; set; }

        public List<CommunityPostComments> Comments { get; set; } = new();
    }
}
