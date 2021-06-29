using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, ICompensationService compensationService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet(Name = "reportscount")]
        [Route("reportscount/{id}")]
        public IActionResult GetNumberofReports(string id)
        {
            _logger.LogDebug($"Retreiving reports for '{id}'");

            int numberOfReports = _employeeService.GetNumberOfReports(id);

            if (numberOfReports < 0) 
            {
                //Depending on the use case for this endpoint (user-facing or not) I'd want to change the logging to be more informative
                _logger.LogDebug($"Error retreiving reports for '{id}'. Consult logs for further information.");
                return NotFound();
            }

            return Ok(numberOfReports);
        }

        [HttpGet(Name = "reports")]
        [Route("reports/{id}")]
        public IActionResult GetEmployeeReports(string id)
        {
            _logger.LogDebug($"Retreiving reports for '{id}'");

            var reports = _employeeService.GetFullReports(id);

            if (reports == null)
            {
                //Depending on the use case for this endpoint (user-facing or not) I'd want to change the logging to be more informative
                _logger.LogDebug($"Error retreiving reports for '{id}'. Consult logs for further information.");
                return NotFound();
            }

            return Ok(reports);
        }

        [HttpGet]
        [Route("compensation/{id}")]
        public IActionResult GetEmployeeCompensation(string id)
        {
            _logger.LogDebug($"Retreiving compensation for '{id}'");

            var comp = _employeeService.GetEmployeeCompensation(id);

            if (comp == null)
            {
                //Depending on the use case for this endpoint (user-facing or not) I'd want to change the logging to be more informative
                _logger.LogDebug($"Error retreiving compensation for '{id}'. Consult logs for further information.");
                return NotFound();
            }

            return Ok(comp);
        }

        [HttpPut]
        [Route("compensation/{id}")]
        public IActionResult CreateEmployeeCompensation(string id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Creating compensation for '{id}'");

            var comp = _employeeService.CreateEmployeeCompensation(compensation);

            if (comp == null)
            {
                //Depending on the use case for this endpoint (user-facing or not) I'd want to change the logging to be more informative
                _logger.LogDebug($"Error creating compensation for '{id}'. Consult logs for further information.");
                return NotFound();
            }
            else
            {
                _employeeService.SetEmployeeCompensation(id, comp.CompensationID);
            }

            return Ok(comp);
        }

        [HttpPost]
        [Route("compensation/{eid}")]
        public IActionResult UpdateEmployeeCompensation(string eid, [FromBody] string cid)
        {
            _logger.LogDebug($"Updating compensation for '{eid}'");

            var comp = _compensationService.GetByID(cid);

            if (comp == null)
            {
                //Depending on the use case for this endpoint (user-facing or not) I'd want to change the logging to be more informative
                _logger.LogDebug($"Error retrieving compensation '{cid}'. Consult logs for further information.");
                return NotFound();
            }
            else
            {
                _employeeService.SetEmployeeCompensation(eid, comp.CompensationID);
            }

            return Ok(_employeeService.GetById(eid));
        }
    }
}
