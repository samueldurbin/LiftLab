using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class FitnessPostComments
    {
        [Key] // primary key
        public int FitnessPostCommentId { get; set; } // unique id fot each comment

        public int FitnessPostId { get; set; } // forein key id for relavent fitnesspost

        public string? Username { get; set; } // user who created the comment. at the minute its set to can be nullable for postman tests

        public string Comment { get; set; } // the comment itself

        [ForeignKey("FitnessPostId")]
        public FitnessPost? FitnessPost { get; set; }
    }
}
