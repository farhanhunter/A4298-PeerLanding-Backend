using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqUpdateLoanDto
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public decimal? Amount { get; set; }
        public decimal? InterestRate { get; set; }
        public int? Duration { get; set; }
    }
}
