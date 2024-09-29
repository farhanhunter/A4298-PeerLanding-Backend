using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqUpdateBalanceDto
    {
        public string LenderId { get; set; }
        public decimal Amount { get; set; }
        public string Operation { get; set; }
    }
}
