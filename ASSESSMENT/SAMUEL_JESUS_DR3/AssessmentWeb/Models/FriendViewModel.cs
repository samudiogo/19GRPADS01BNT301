using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssessmentWeb.Models
{
    [Obsolete("tira isso" ,true)]
    public class FriendViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhotoUrl { get; set; }

        [Required]
        public Guid StateId { get; set; }

        public Guid CountryId { get; set; }
        public string StateName { get; set; }
        public string StateFlagUrl { get; set; }
        public string CountryName { get; set; }
        public string CountryFlagUrl { get; set; }

        
        public IEnumerable<FriendViewModel> Friends { get; set; } = new HashSet<FriendViewModel>();

        public string FullName => $"{Name} {LastName}";
    }


}