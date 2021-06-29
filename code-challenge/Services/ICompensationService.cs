using challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation GetByID(string id);

        //Compensation GetByEmployeeID(string id);

        Compensation Create(Compensation comp);
    }
}
