using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetByID(string id);

        Compensation GetByEmployeeID(string id);

        Compensation Create(Compensation comp);

        Task SaveAsync();
    }
}
