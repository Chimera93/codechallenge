using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Data;
using challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation GetByID(string id)
        {
            return _compensationContext.Compensations.SingleOrDefault(c => c.identifier == id);
        }

        public Compensation GetByEmployeeID(string id)
        {
            return _compensationContext.Compensations.SingleOrDefault(c => c.entityID == id);
        }

        public Compensation Create(Compensation comp)
        {
            comp.identifier = Guid.NewGuid().ToString();
            _compensationContext.Compensations.Add(comp);

            _compensationContext.Entry(comp).State = EntityState.Added;
            _compensationContext.SaveChanges();

            return comp;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
