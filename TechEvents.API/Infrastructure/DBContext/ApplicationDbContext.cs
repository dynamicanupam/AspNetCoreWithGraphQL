using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechEvents.API.Infrastructure.DBContext;
using TechEvents.API.Infrastructure.EntityConfig;

namespace TechEvents.API.Infrastructure.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> context) : base(context)
        {

        }
        public DbSet<TechEventInfo> TechEventInfo { get; set; }
        public DbSet<Participant> Participant { get; set; }
        public DbSet<EventParticipants> EventParticipants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TechEventEntityConfig());
            modelBuilder.ApplyConfiguration(new ParticipantEntityConfig());
            modelBuilder.ApplyConfiguration(new EventParticipantsEntityConfig());
        }
    }
}
