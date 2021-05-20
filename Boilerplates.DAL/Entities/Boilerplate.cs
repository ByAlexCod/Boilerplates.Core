using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Boilerplates.DAL.Entities
{
    public class Boilerplate
    {
        public int BoilerplateId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(10000)]

        public string MdDescription { get; set; }
        public User Author { get; set; }
    }
}
