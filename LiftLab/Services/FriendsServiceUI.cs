using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Services
{
    public class FriendsServiceUI
    {
        private readonly HttpClient _httpClient;

        public FriendsServiceUI()
        {
            _httpClient = new HttpClient  // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests

            };
        }

        public async Task<bool> AddFriend(int userId, int friendUserId) // the logged in userid makes friends with the frienduserid
        {
            var addFriend = await _httpClient.PostAsJsonAsync("Friends/addfriend", new  // sends post request to add a new friend
            {
                // send a json body containing the ids:
                UserId = userId,  // userid (will be for logged in user)
                FriendUserId = friendUserId // user to be added as a friend
            });

            return addFriend.IsSuccessStatusCode; // the failure is declared elsewhere, returns true if success
        }

        // retrieves a list of posts made by the logged in user's friendlist

        public async Task<List<CommunityPost>> GetFriendsPosts(int userId)
        {
            var response = await _httpClient.GetAsync($"Friends/friends/posts/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityPost>>();
            }

            throw new Exception("Failed to fetch friends' posts.");
        }
    }
}
