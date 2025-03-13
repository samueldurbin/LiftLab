using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class NutritionPostComments
    {
        [Key] // primary key
        public int NutritionPostCommentId { get; set; } // unique id fot each comment

        public int NutritionPostId { get; set; } // forein key id for relavent nutritionpost

        public string? Username { get; set; } // user who created the comment. at the minute its set to can be nullable for postman tests

        public string Comment { get; set; } // the comment itself
    }
}
