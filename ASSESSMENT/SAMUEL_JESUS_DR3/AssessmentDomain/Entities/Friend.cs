using System;
using System.Collections.Generic;

namespace AssessmentDomain.Entities
{
    public class Friend
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }

        
        //Relationships
        public virtual State State { get; set; }
        public virtual ICollection<Friend> Friends { get; set; } = new HashSet<Friend>();
    }
}
