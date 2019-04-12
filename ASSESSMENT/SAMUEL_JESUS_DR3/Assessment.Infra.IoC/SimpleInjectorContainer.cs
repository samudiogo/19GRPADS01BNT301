using Assessment.Data;
using Assessment.Data.Repositories;
using AssessmentDomain.DomainService;
using SimpleInjector;

namespace Assessment.Infra.IoC
{
    public static class SimpleInjectorContainer
    {
        public static void Registra(Container container)
        {
            container.Options.UseFullyQualifiedTypeNames = true;

            container.Register<AssessmentDbContext>(Lifestyle.Scoped);

            //servicos:
            container.Register<IPhotoService, PhotoAzureBlobRepository>(Lifestyle.Scoped);
        }
    }
}
