using Shared.Models;

namespace WebApi.Services
{
    public interface IPalsService 
    {
        Task<Pals> AddPals(Pals pals); // method to add pals (contains pal to be added and the id of whos being added)
        // returns a pals object
        Task<int> GetMyPals(int userId); // returns the count of followers from the service
    }

}
