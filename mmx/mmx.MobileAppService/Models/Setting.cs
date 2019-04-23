using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mmx.MobileAppService.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Update { get; set; }
    }
}
