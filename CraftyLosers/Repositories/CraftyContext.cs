using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CraftyLosers.Models;

namespace CraftyLosers.Repositories
{
    public class CraftyContext : DbContext
    {
        public CraftyContext()
            : base(@"Data Source=.\sqlexpress;Initial Catalog=BigIsLoser;Persist Security Info=True;MultipleActiveResultSets=True;User ID=macdbuser;Password=cjogre77")
            //: base(@"metadata=res://*/Repositories.BISL_EntityDataModel.csdl|res://*/Repositories.BISL_EntityDataModel.ssdl|res://*/Repositories.BISL_EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string='Data Source=.\sqlexpress;Initial Catalog=BigIsLoser;Persist Security Info=True;MultipleActiveResultSets=True;User ID=macdbuser;Password=cjogre77'")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WeightCheckIn> WeightCheckIns { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}