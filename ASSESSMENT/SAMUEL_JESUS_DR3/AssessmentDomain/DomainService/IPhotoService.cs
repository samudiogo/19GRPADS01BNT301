using AssessmentDomain.Entities;
using System.Threading.Tasks;

namespace AssessmentDomain.DomainService
{
    public interface IPhotoService
    {
        string Create(Photo photo);
        Task<string> CreateAsync(Photo photo);
    }
}
