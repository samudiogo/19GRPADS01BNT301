using AssessmentDomain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssessmentCountrFlagWebApi.Models
{
    public class CountryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<StateCreateModel> States { get; set; } = new HashSet<StateCreateModel>();
    }

    public class CountryCreateEditModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
    }
    
}