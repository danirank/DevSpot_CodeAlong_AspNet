using DevSpot_CodeAlong.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DevSpot_CodeAlong.Models;
using DevSpot_CodeAlong.Data;
using DevSpot_CodeAlong.ViewModel;
using Microsoft.AspNetCore.Authorization;
using DevSpot_CodeAlong.Constants;

namespace DevSpot_CodeAlong.Controllers
{
    [Authorize] //Tvingar inloggning för att använda metoder
    public class JobPostingsController : Controller
    {
        //Dependency Injection 
        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(IRepository<JobPosting> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [AllowAnonymous] //Tillåter att inte vara inloggag när vi använt authorize
        public async Task<IActionResult> Index()
        {
            var jobPosting = await _repository.GetAllAsync();

            //Logik för att employer bara ska se sina egna jobbannonser
            if (User.IsInRole(Roles.Employer))
            {
                var userId = _userManager.GetUserId(User);
                jobPosting = jobPosting.Where(jp => jp.UserId == userId);
            }

            return View(jobPosting);
        }

        [Authorize(Roles = "Admin, Employer")]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin, Employer")]
        [HttpPost] 

        //Använd ViewModel för att komma Runt Godkänt ModelState där t.ex UserID är rquired men inte fylls i i formuläret
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {
            var jobPosting = new JobPosting()
            {
                Title = jobPostingVm.Title,
                Description = jobPostingVm.Description,
                Company = jobPostingVm.Company,
                Location = jobPostingVm.Location,
                UserId = _userManager.GetUserId(User)
            };

            if (ModelState.IsValid)
            {
                await _repository.AddAsync(jobPosting);
                return RedirectToAction(nameof(Index));

            }

            return View(jobPostingVm);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPost = await _repository.GetByIdAsync(id);
            if (jobPost == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            if (User.IsInRole(Roles.Admin)==false && jobPost.UserId != userId)
            {
                return Forbid();
            }
            await _repository.DeleteAsync(id);
            

            return Ok();
        }
    }
}
