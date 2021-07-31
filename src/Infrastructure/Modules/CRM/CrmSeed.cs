using Infrastructure.Data;
using Infrastructure.Modules.CRM.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Modules.CRM
{
    public class CrmSeed
    {
        public static async Task SeedAsync(
            AppDbContext dbContext,
            ILoggerFactory loggerFac,
            int? retry = 0)
        {
            int retryForAvail = retry.Value;

            try
            {
                if (!dbContext.Customers.Any())
                {
                    foreach (var customer in GetPreconfiguredCustomer())
                    {                        
                        customer.Id = Guid.NewGuid().ToString();
                        await dbContext.Customers.AddAsync(customer);

                        foreach (var contact in GetPreconfiguredContact(customer.Id))
                        {
                            contact.Id = Guid.NewGuid().ToString();
                            await dbContext.Contacts.AddAsync(contact);

                            foreach (var ticket in GetPrecongiuredTicket(contact.Id))
                            {
                                ticket.Id = Guid.NewGuid().ToString();
                                await dbContext.Tickets.AddAsync(ticket);

                                foreach (var labor in GetPreconfiguredLabor(ticket.Id))
                                {
                                    labor.Id = Guid.NewGuid().ToString();
                                    await dbContext.LaborHours.AddAsync(labor);
                                }
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvail < 3)
                {
                    retryForAvail++;
                    var log = loggerFac.CreateLogger<AppDbContext>();
                    log.LogError(ex.Message);
                    await SeedAsync(
                        dbContext,                        
                        loggerFac,
                        retryForAvail);
                }
                throw;
            }
        }

        static IEnumerable<Customer> GetPreconfiguredCustomer()
        {
            var data = new List<Customer>();

            for (int i = 0; i < 100; i++)
            {
                data.Add(new Customer()
                {
                    Name = $"CustomerName #{i}",
                    Address1 = $"Address1 #{i}",
                    Address2 = $"Address2 #{i}",
                    City = $"City #{i}",
                    State = $"AA",
                    Zip = $"{i}",
                    Country = "US",
                    Phone = $"{i}"
                });
            }

            return data;
        }

        static IEnumerable<Contact> GetPreconfiguredContact(string customerId)
        {
            var data = new List<Contact>();

            for (int i = 0; i < 10; i++)
            {
                data.Add(new Contact()
                {
                    Title = $"Title #{i}",
                    FirstName = $"First #{i}",
                    LastName = $"Last #{i}",
                    Email = $"email{i}@company{i}.com",
                    Phone = $"123-123-12{i}",
                    CustomerId = customerId
                });
            }

            return data;
        }

        static IEnumerable<Ticket> GetPrecongiuredTicket(string contactId)
        {
            var data = new List<Ticket>();

            for (int i = 0; i < 20; i++)
            {
                data.Add(new Ticket()
                {
                    Title = $"Title #{i}",
                    Description = $"Description #{i}",
                    Status = (i % 2) == 0 ? Constants.TicketStatus.Open : Constants.TicketStatus.Completed,
                    IsBilled = (i % 2) == 0 ? false : true,
                    IsPaid = (i % 2) == 0 ? false : true,
                    ContactId = contactId
                });
            }

            return data;
        }

        static IEnumerable<Labor> GetPreconfiguredLabor(string ticketId)
        {
            var data = new List<Labor>();

            for (int i = 0; i < 5; i++)
            {
                data.Add(new Labor()
                {
                    Hour = i+1,
                    Minute = i+2,
                    Description = $"Description #{i}",
                    TicketId = ticketId
                });
            }

            return data;
        }
    }
}
