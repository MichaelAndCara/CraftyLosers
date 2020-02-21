using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CraftyLosers.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CraftyLosers.Repositories
{
    public class CraftyContext : DbContext
    {
        public CraftyContext()
            //: base(@"Data Source=.\sqlexpress;Initial Catalog=BigIsLoser;Persist Security Info=True;MultipleActiveResultSets=True;User ID=macdbuser;Password=[password]")
            //: base(@"Data Source=localhost;Initial Catalog=BigIsLoser;Persist Security Info=True;MultipleActiveResultSets=True;User ID=macdbuser;Password=[password]")
            : base(@"Server=tcp:l087l6ozx5.database.windows.net;Database=BigIsLoser;User ID=michaelandcara@l087l6ozx5;Password=[password];Trusted_Connection=False;Encrypt=True;Connection Timeout=30;")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WeightCheckIn> WeightCheckIns { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<WorkoutRef> WorkoutRefs { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
    }
}
