using Microsoft.EntityFrameworkCore;
using Feedback.Domain.Entities;


namespace Feedback.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<FeedbackNps> Feedbacks { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var feedbackEntity = modelBuilder.Entity<FeedbackNps>();

            feedbackEntity.HasKey(f => f.Id);            

            modelBuilder.Entity<FeedbackNps>().ToTable("Feedbacks");

        }

    }
}
