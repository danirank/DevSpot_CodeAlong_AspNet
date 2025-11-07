using DevSpot_CodeAlong.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSpot_CodeAlong.Controllers
{
    public class JobPostingsController : Controller
    {
        //Dependency Injection 
        private readonly IRepository<JobPostingRepository> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(IRepository<JobPostingRepository> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
