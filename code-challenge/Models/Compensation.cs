using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        public Compensation()
        {
            CompensationID = new Guid().ToString();
            dateTimeUTC = DateTime.Now.ToUniversalTime();
        }

        [Key]
        public string CompensationID { get; set; }
        public float salary { get; set; }
        public DateTime dateTimeUTC { get; set; }
    }
}
