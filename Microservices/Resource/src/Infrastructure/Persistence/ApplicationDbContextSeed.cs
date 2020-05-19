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
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            },
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(2),
                                To = DateTime.Now.AddHours(3)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Helsinki",
                        Description = "Meeting room Helsinki",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Tokyo",
                        Description = "Meeting room Tokyo",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Berlin",
                        Description = "Meeting room Berlin",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Moscow",
                        Description = "Meeting room Moscow",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Nairobi",
                        Description = "Meeting room Nairobi",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Rio",
                        Description = "Meeting room Rio",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Denver",
                        Description = "Meeting room Denver",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                    new Domain.Entities.Resource
                    {
                        Id = Guid.NewGuid(),
                        Name = "Stockholm",
                        Description = "Meeting room Stockholm",
                        Available = new List<DayAndTime>
                        {
                            new DayAndTime
                            {
                                Id = Guid.NewGuid(),
                                DayOfWeek = DateTime.Now.DayOfWeek,
                                From = DateTime.Now.AddHours(-1),
                                To = DateTime.Now.AddHours(1)
                            }
                        }
                    },
                });

                await context.SaveChangesAsync();
            }
        }
    }
}