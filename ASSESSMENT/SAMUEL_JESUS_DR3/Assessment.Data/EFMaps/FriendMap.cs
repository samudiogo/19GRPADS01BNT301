using AssessmentDomain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace Assessment.Data.EFMaps
{
    public class FriendMap : EntityTypeConfiguration<Friend>
    {
        public FriendMap()
        {
            HasKey(c => c.Id);
            Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();

            Property(c => c.LastName)
                .HasMaxLength(255)
                .IsRequired();

            Property(c => c.PhotoUrl)
                .IsMaxLength()
                .IsRequired();

            Property(c => c.Email)
                .HasMaxLength(255)
                .IsRequired();

            Property(c => c.PhoneNumber)
                .HasMaxLength(255)
                .IsRequired();

            Property(c => c.BirthDate)
                .IsRequired();

            HasRequired(p => p.State);

            HasMany(p => p.Friends)
                .WithMany()
                .Map(f =>
                {
                    f.MapLeftKey("MainFriendId");
                    f.MapRightKey("ChildFriendId");
                    f.ToTable("Friendship");
                });

            MapToStoredProcedures();


        }
    }
}