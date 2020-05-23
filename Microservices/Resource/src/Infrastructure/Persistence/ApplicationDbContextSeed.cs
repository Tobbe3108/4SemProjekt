using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Resource.Domain.Entities;
using ToolBox.Contracts.Resource;

namespace Resource.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            var random = new Random();
            // Seed, if necessary
            if (!context.Resources.Any())
            {
                await context.Resources.AddRangeAsync(new List<Domain.Entities.Resource>
                {
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Oslo",
                        Description = "Meeting room Oslo",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Helsinki",
                        Description = "Meeting room Helsinki",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tokyo",
                        Description = "Meeting room Tokyo",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Berlin",
                        Description = "Meeting room Berlin",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Moscow",
                        Description = "Meeting room Moscow",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Nairobi",
                        Description = "Meeting room Nairobi",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Rio",
                        Description = "Meeting room Rio",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Denver",
                        Description = "Meeting room Denver",
                        Available = GetTimeSlots()
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Stockholm",
                        Description = "Meeting room Stockholm",
                        Available = GetTimeSlots()
                    },
                });

                await context.SaveChangesAsync();
            }
        }

        private static List<DayAndTime> GetTimeSlots()
        {
            var random = new Random();
            var list = new List<DayAndTime>();
            for (var i = 0; i < 7; i++)
            {
                var time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 08, 00, 00);
                while (time <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 00, 00))
                {
                    var oldTime = time;
                    time = time.AddMinutes(random.Next(15, 481));
                    
                    if (time >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 16, 00, 00)) break;
                    list.Add(new DayAndTime
                    {
                        Id = Guid.NewGuid(),
                        DayOfWeek = (DayOfWeek)i,
                        From = oldTime,
                        To = time
                    });
                    
                    time = time.AddMinutes(random.Next(15, 240));
                }
            }

            return list;
        }
    }
}