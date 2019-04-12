using System.Threading.Tasks;
using AssessmentBlobService;
using AssessmentDomain.DomainService;
using AssessmentDomain.Entities;

namespace Assessment.Data.Repositories
{
    public class PhotoAzureBlobRepository: IPhotoService
    {
        public string Create(Photo photo)
        {
            var blobService = new BlobService();

            return blobService.UploadFile(photo.ContainerName,
                photo.FileName, photo.BinaryContent,
                photo.ContentType);
        }

        public async Task<string> CreateAsync(Photo photo)
        {
            var blobService = new BlobService();

            return await blobService.UploadFileAsync(photo.ContainerName,
                photo.FileName, photo.BinaryContent,
                photo.ContentType);
        }
    }
}