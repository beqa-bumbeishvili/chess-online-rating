namespace Online_Chess_Server.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ChessModel : DbContext
    {
        public ChessModel()
            : base("name=ChessModel1")
        {
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Games)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.FirstPlayerID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Games1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.SecondPlayerID);
        }
    }
}
