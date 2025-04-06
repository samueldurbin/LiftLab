using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class CommunityPostComments
    {
        [Key] // primary key
        public int CommunityPostCommentId { get; set; } // unique id fot each comment

        public int CommunityPostId { get; set; } // forein key id for relavent fitnesspost

        public string? Username { get; set; } // user who created the comment. at the minute its set to can be nullable for postman tests

        public string Comment { get; set; } // the comment itself

        [JsonIgnore]
        [ForeignKey("CommunityPostId")]
        public CommunityPost? CommunityPost { get; set; }
    }
}
