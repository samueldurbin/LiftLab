using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController] // apicontroller
    [Route("api/[controller]")] // url for api which uses the controller name as the url
    public class NutritionPostsController : ControllerBase
    {
        private readonly INutritionPostService _nutritionPostService; // dependency injection

        public NutritionPostsController(INutritionPostService nutritionPostService) // creates an instance of the IService
        {
            _nutritionPostService = nutritionPostService;
        }

    }
}
