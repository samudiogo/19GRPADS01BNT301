using System;

namespace AssessmentDomain.Entities
{
    public class State
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FlagPhotoUrl { get; set; }

        public virtual Country Country { get; set; }
    }
}