using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace AssessmentDomain.Entities
{
    public class Photo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ContainerName { get; set; }
        public string FileName { get; set; }
        [NotMapped]
        public Stream BinaryContent { get; set; } //Imagem (não armazena no BD)
        public string Url { get; set; }
        public string ContentType { get; set; }

    }
}
