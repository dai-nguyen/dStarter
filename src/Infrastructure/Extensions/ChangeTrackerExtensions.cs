using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Infrastructure.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void SetShadowProperties(
            this ChangeTracker changeTracker,
            IUserSession userSession)
        {
            changeTracker.DetectChanges();
            var dbContext = (AppDbContext)changeTracker.Context;

            string userId = "?";
            DateTime timestamp = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(userSession.UserId))
                userId = userSession.UserId;

            foreach (EntityEntry entry in changeTracker.Entries())
            {
                if (entry.Entity is IEntity)
                {
                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        entry.Property("DateCreated").CurrentValue = timestamp;
                        entry.Property("CreatedBy").CurrentValue = userId;

                        entry.Property("DateUpdated").CurrentValue = timestamp;
                        entry.Property("UpdatedBy").CurrentValue = userId;
                    }

                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    {
                        entry.Property("DateUpdated").CurrentValue = timestamp;
                        entry.Property("UpdatedBy").CurrentValue = userId;
                    }
                }
            }
        }
    }
}
