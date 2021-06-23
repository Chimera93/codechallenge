using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation GetByID(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetByID(id);
            }

            return null;
        }

        public Compensation GetByEmployeeID(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetByEmployeeID(id);
            }

            return null;
        }

        public Compensation Create(Compensation comp)
        {
            if (comp != null)
            {
                _compensationRepository.Create(comp);
                _compensationRepository.SaveAsync().Wait();
            }

            return comp;
        }
    }
}
