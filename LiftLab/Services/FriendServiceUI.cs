using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Shared.Models;

namespace LiftLab.Services
{
    public class FriendServiceUI
    {
        private readonly HttpClient _httpClient;

        public FriendServiceUI()
        {
            _httpClient = new HttpClient  // Creates an instance of HTTP Client
            {
                BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/COMP3000/SDurbin/api/")  // URL for api requests

            };
        }

        public async Task<bool> AddFriend(int userId, int friendUserId)
        {
            var response = await _httpClient.PostAsJsonAsync("Friends/addfriend", new  // sends post request to add a new friend
            {
                UserId = userId,  // userid (will be for logged in user)
                FriendUserId = friendUserId // user to be added as a friend
            });

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CommunityPost>> GetFriendsPosts(int userId)
        {
            var response = await _httpClient.GetAsync($"Friends/friends/posts/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<CommunityPost>>();
            }

            throw new Exception("Failed to retrieve friends' posts.");
        }
    }
}
