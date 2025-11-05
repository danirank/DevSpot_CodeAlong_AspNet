using DevSpot_CodeAlong.Data;
using DevSpot_CodeAlong.Models;
using DevSpot_CodeAlong.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSpot.Test
{
    public class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPostingDb")
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => 
            new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            //Create Db context
            var db = CreateDbContext();

            // Job posting repo
            var repo = new JobPostingRepository(db);

            // job posting to add 
            JobPosting post = new JobPosting()
            {

                Date = DateTime.Now,
                Company ="TestCompany",
                Title = "TestTitle",
                Description = "TestDescription",
                Location = "TestLocation",
                UserId = "TestUser"

            };

            // execute
            await repo.AddAsync(post);
            
            // check result? 
            var result = db.JobPostings.FirstOrDefault(p => p.Title == "TestTitle");

            //assert (output Ok?)
            Assert.NotNull(result);
            Assert.Equal("TestCompany", result.Company);
            

        }
    }
}
