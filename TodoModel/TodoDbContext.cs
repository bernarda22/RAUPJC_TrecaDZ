using System.Data.Entity;

namespace TodoModel
{
    public class TodoDbContext : DbContext
    {
        public IDbSet<TodoItem> TodoItems { get; set; }

        // Hardcoded connection string is a terrible practice
        // You should use config files, but for demo simplicity this is okay 
        public TodoDbContext(string connectionString) : base(connectionString)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasKey(c => c.Id);
            modelBuilder.Entity<TodoItem>().Property(c => c.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.DateCompleted).IsOptional();
            modelBuilder.Entity<TodoItem>().Property(c => c.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(c => c.UserId).IsRequired();
        }
    }
}
