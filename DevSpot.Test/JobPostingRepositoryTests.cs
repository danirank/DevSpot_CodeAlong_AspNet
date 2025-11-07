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

        [Fact]
        public async Task GetIdAsync_ShouldReturnJobPosting()
        {
            //Create Db context
            var db = CreateDbContext();

            // Job posting repo
            var repo = new JobPostingRepository(db);

            // job posting to add 
            JobPosting post = new JobPosting()
            {

                Date = DateTime.Now,
                Company = "TestCompany",
                Title = "TestTitle",
                Description = "TestDescription",
                Location = "TestLocation",
                UserId = "TestUser"

            };

           await db.JobPostings.AddAsync(post);

           await db.SaveChangesAsync();

            var result = await repo.GetByIdAsync(post.Id);

            Assert.NotNull(result);
            Assert.Equal("TestTitle", result.Title);
        }

        [Fact]
        public async Task GetIdAsync_ShouldThrowKeyNotFoundExeption()
        {
            var db = CreateDbContext();

            var repo = new JobPostingRepository(db);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => repo.GetByIdAsync(999)
                );

        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllJobPostings()
        {
            var db = CreateDbContext();
            var repo = new JobPostingRepository(db);

            List<JobPosting> values = new List<JobPosting>()
            {
                new JobPosting()
                {
                    Company ="TestCompany0",
                    Description= "TesDescription0",
                    Title= "TestTitle0",
                    Location = "TesLocation0",
                    UserId = "TestUserId0"
                },
                new JobPosting()
                {
                    Company ="TestCompany1",
                    Description= "TesDescription1",
                    Title= "TestTitle1",
                    Location = "TesLocation1",
                    UserId = "TestUserId1"
                },
                new JobPosting()
                {
                    Company ="TestCompany2",
                    Description= "TesDescription2",
                    Title= "TestTitle2",
                    Location = "TesLocation2",
                    UserId = "TestUserId2"
                }

            };


            await db.JobPostings.AddRangeAsync(values);
            await db.SaveChangesAsync();

            var result = await repo.GetAllAsync();

            
            Assert.NotNull(result);
            Assert.True(result.Count()>=3);

        }


        [Fact]
        public async Task UpdateAsync_ShouldUpdatePostById()
        {
            var db = CreateDbContext();
            var repo = new JobPostingRepository(db); 


            JobPosting initialPost = new JobPosting()
            {

                Date = DateTime.Now,
                Company = "TestCompany",
                Title = "TestTitle",
                Description = "TestDescription",
                Location = "TestLocation",
                UserId = "TestUser"

            };

            await db.JobPostings.AddAsync(initialPost);
            await db.SaveChangesAsync();

            string updateTitle = "NewTestTitle";

            initialPost.Title = updateTitle;

            await repo.UpdateAsync(initialPost);
            
            var result = db.JobPostings.Find(initialPost.Id);

            Assert.NotNull(result);
            Assert.Equal($"{updateTitle}", result.Title);


        }

        [Fact]
        public async  Task DeleteAsync_ShouldDeleteById()
        {
            var db = CreateDbContext();
            var repo = new JobPostingRepository(db);

            JobPosting post = new JobPosting()
            {

                Date = DateTime.Now,
                Company = "TestCompany",
                Title = "TestTitle",
                Description = "TestDescription",
                Location = "TestLocation",
                UserId = "TestUser"

            };

            await db.JobPostings.AddAsync(post);
            await db.SaveChangesAsync();

            await repo.DeleteAsync(post.Id);

            var result = db.JobPostings.Find(post.Id);

            Assert.Null(result);


        }
    }
}
