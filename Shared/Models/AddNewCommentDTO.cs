using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    // this is a Data Transfer Object for Comments in order to decrease the request body size in backend testing
    public class AddNewCommentDTO
    {
        //public int FitnessPostCommentId { get; set; }
        public int FitnessPostId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
    }
}
