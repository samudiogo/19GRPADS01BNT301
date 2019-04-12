using System;
using System.Collections.Generic;

namespace AssessmentDomain.Entities
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }

        //relationships
        public virtual ICollection<State> States { get; set; }

    }
}