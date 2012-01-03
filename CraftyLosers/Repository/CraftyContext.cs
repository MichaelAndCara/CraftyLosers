using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CraftyLosers.Repository
{
    public class CraftyContext : DbContext
    {
        public CraftyContext()
            : base(@"metadata=res://*/Repositories.BISL_EntityDataModel.csdl|res://*/Repositories.BISL_EntityDataModel.ssdl|res://*/Repositories.BISL_EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string='Data Source=.\sqlexpress;Initial Catalog=BigIsLoser;Persist Security Info=True;MultipleActiveResultSets=True;User ID=macdbuser;Password=cjogre77'")
        {
            
        }

        public DbSet<WeightCheckIns> WeightCheckins { get; set; }
    }
}