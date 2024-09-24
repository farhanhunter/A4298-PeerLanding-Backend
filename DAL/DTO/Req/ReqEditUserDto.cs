using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Req
{
    public class ReqEditUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a positive number")]
        public decimal? Balance { get; set; } 
    }
}
