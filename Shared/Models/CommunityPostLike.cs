using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CommunityPostLike
    {
        [Key]
        public int CommunityPostLikeId { get; set; } // the id of the like (primary key)

        public int CommunityPostId { get; set; } // the fitness post that has been liked by a user

        public int UserId { get; set; } // the userid who liked the post

        [ForeignKey("CommunityPostId")] // the foreign key of the fitnesspostid
        public CommunityPost CommunityPost { get; set; }
    }
}
