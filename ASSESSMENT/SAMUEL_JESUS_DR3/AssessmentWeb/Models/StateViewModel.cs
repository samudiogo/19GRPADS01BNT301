using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssessmentWeb.Models
{
    public class StateCreateViewModel
    {
        public Guid Id { get; internal set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        
        public string FlagPhotoUrl { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        public IEnumerable<CountryViewModel> Countries { get; set; }
    }

    public class StateEditViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FlagPhotoUrl { get; set; }

        public Guid CountryId { get; set; }

        public string CountryName { get; set; }

    }
}