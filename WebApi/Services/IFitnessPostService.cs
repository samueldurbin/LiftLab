using Shared.Models;

namespace WebApi.Services
{
    public interface IFitnessPostService
    {
        Task<FitnessPost> CreatePost(FitnessPost post);  // creates a new user

    }
}
