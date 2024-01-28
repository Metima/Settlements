using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Settlements.Models;

namespace Settlements.Data
{
    public class SettlementsContext : DbContext
    {
        public SettlementsContext (DbContextOptions<SettlementsContext> options)
            : base(options)
        {
        }

        public DbSet<Settlements.Models.SettlementModel> SettlementModel { get; set; } = default!;
    }
}
