using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;

namespace FlashCards_Project
{
    public class FlashcardsDbContext : DbContext
    {
        //Constructor calling the DbContext class constructor
        public FlashcardsDbContext(string? connectionString) : base(connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            // Database.SetInitializer(new CreateDatabaseIfNotExists<FlashcardsDbContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FlashcardsDbContext, Migrations.Configuration>());
        }
        public FlashcardsDbContext()
        {
            // Database.SetInitializer(new CreateDatabaseIfNotExists<FlashcardsDbContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FlashcardsDbContext, Migrations.Configuration>());
        }

        //Adding Domain Classes as DbSet
        public DbSet<Stack> Stacks { get; set; } = null!;
        public DbSet<Flashcard> Flashcards { get; set; } = null!;

        public DbSet<Sessions> Sessions { get; set; } = null!;
    }

    public class FlashcardSeedInitializer : DropCreateDatabaseAlways<FlashcardsDbContext>
    {
        protected override void Seed(FlashcardsDbContext context)
        {
            string query = File.ReadAllText("flashcards.text");
            context.Flashcards.SqlQuery(query);
            base.Seed(context);
        }
    }
}