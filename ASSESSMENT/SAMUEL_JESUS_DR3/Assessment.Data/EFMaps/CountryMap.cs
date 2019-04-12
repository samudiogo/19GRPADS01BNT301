using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AssessmentDomain.Entities;

namespace Assessment.Data.EFMaps
{
    public class CountryMap:EntityTypeConfiguration<Country>
    {
        public CountryMap()
        {
            HasKey(c => c.Id);
            Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();
            MapToStoredProcedures();
        }
    }
}