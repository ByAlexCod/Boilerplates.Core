using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Boilerplates.DAL.Entities
{
    public class Placeholder
    {
        public int PlaceholderId { get; set; }
        [StringLength(255)]
        public string Origin { get; set; }
        [StringLength(255)]
        public string PublicName { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }

    }
}
