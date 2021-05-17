using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Inlämning_Asp.Models;

namespace Inlämning_Asp.Models
{
    public class Buyer : IdentityUser
    {
        [InverseProperty("Attendees")]
        public List<Event> Events { get; set; }
        [InverseProperty("Organizer")]
        public List<Event> OrganizedEvents { get; set; }
        public List<Buyer> Buyers  { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

     


        public List<IdentityRole> Roles { get; set; }

        public static implicit operator Buyer(Organizer v)
        {
            throw new NotImplementedException();
        }
    }
}