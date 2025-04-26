using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Shared.Utilities;

namespace Shared.Models
{
    public class CommunityPost : INotifyPropertyHelper
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

        public ObservableCollection<CommunityPostComments> Comments { get; set; } = new(); // this makes the comments ui get real time updates


        // this part of the model is required for displaying and adding comments to the posts
        // requires inotifypropertychanged

        [JsonIgnore]
        [NotMapped] // not mapped prevents the database from interacting with this part of the model as there were failures in regards to swaggerui without it
        private string commentText;

        [JsonIgnore] // this is used to prevent it being part of the json request
        [NotMapped]
        public string CommentText 
        {
            get => commentText;
            set => SetProperty(ref commentText, value);
        }

        [JsonIgnore]
        [NotMapped]
        private bool showCommentBox;

        [JsonIgnore] // this is used to prevent it being part of the json request
        [NotMapped]
        public bool ShowCommentBox
        {
            get => showCommentBox;
            set => SetProperty(ref showCommentBox, value);
        }

    }
}
