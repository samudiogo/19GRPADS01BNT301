using System;
using System.ComponentModel.DataAnnotations;

namespace AssessmentCountrFlagWebApi.Models
{
    public class StateCreateModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FlagPhotoUrl { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        public string CountryName { get; set; }
    }

    public class StateEditModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FlagPhotoUrl { get; set; }
    }
}