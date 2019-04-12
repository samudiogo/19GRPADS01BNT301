using System.Reflection;
using System.Web.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using static Assessment.Infra.IoC.SimpleInjectorContainer;
namespace AssessmentWeb
{
    public static class SimpleInjectorInitializer
    {
        public static void Inicializa()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InicializaContainer(container);


            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            //container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InicializaContainer(Container container)
        {
            Registra(container);

        }
    }
}