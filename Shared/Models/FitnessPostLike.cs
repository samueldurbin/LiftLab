using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    // this class represents a table in the database that helps monitor likes of a post
    // at the minute its only representing fitness posts but will also be for nutrition
    public class FitnessPostLike
    {
        [Key]
        public int FitnessPostLikeId { get; set; } // the id of the like (primary key)

        public int FitnessPostId { get; set; } // the fitness post that has been liked by a user

        public int UserId { get; set; } // the userid who liked the post

        [ForeignKey("FitnessPostId")] // the foreign key of the fitnesspostid
        public FitnessPost FitnessPost { get; set; }
    }
}
