using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Boilerplates.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        [StringLength(255)]
        public string UserName { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(500)]
        public string Password { get; set; }
        public ICollection<Boilerplate> Boilerplates { get; set; }
    }
}
