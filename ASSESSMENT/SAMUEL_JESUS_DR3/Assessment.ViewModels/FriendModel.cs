using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assessment.ViewModels
{
    public class FriendModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Guid StateId { get; set; }
        public string StateName { get; set; }
        public string StateFlagPhotoUrl { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryFlagPhotoUrl { get; set; }
        public int TotalFriends { get; set; }
        public ICollection<FriendModel> Friends { get; set; }

        public string FullName => $"{Name} {LastName}";
    }
    public class FriendCreateEditModel
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
        public string PhotoUrl { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public Guid StateId { get; set; }

        public ICollection<Guid> FriendsGuidList { get; set; } = new HashSet<Guid>();
    }
}