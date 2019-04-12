using System.Data.Entity.ModelConfiguration;
using AssessmentDomain.Entities;

namespace Assessment.Data.EFMaps
{
    public class StateMap: EntityTypeConfiguration<State>
    {
        public StateMap()
        {
            HasKey(c => c.Id);
            Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();

            HasRequired(p => p.Country);
            MapToStoredProcedures();
        }
    }
}