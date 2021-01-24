using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Stores
{
    public class WikiStore : GenericStore<Wiki, WikiDto>
    {
        public WikiStore(
            ILogger<WikiStore> logger, 
            AppDbContext dbContext, 
            IMapper mapper, 
            IUserSession userSession) 
            : base(logger, dbContext, mapper, userSession)
        {
        }


    }
}
