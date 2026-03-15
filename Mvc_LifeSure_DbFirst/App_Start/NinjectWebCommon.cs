using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.AboutRepositories;
using Mvc_LifeSure_DbFirst.Repositories.BlogRepositories;
using Mvc_LifeSure_DbFirst.Repositories.FaqRepositories;
using Mvc_LifeSure_DbFirst.Repositories.FeatureRepositories;
using Mvc_LifeSure_DbFirst.Repositories.ServiceRepositories;
using Mvc_LifeSure_DbFirst.Repositories.SliderRepositories;
using Mvc_LifeSure_DbFirst.Repositories.TeamRepositories;
using Mvc_LifeSure_DbFirst.Repositories.TestimonialRepositories;
using Mvc_LifeSure_DbFirst.Services.AboutServices;
using Mvc_LifeSure_DbFirst.Services.BlogServices;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Mvc_LifeSure_DbFirst.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Mvc_LifeSure_DbFirst.App_Start.NinjectWebCommon), "Stop")]

namespace Mvc_LifeSure_DbFirst.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            RegisterServices(kernel);

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            // DbContext
            kernel.Bind<MvcLifeSureDbEntities>()
                  .ToSelf()
                  .InRequestScope();


            // Repository
            kernel.Bind<IAboutRepository>()
                  .To<AboutRepository>();

            kernel.Bind<IBlogRepository>()
                  .To<BlogRepository>();

            kernel.Bind<IFaqRepository>()
                  .To<FaqRepository>();

            kernel.Bind<IFeatureRepository>()
                  .To<FeatureRepository>();

            kernel.Bind<IServiceRepository>()
                  .To<ServiceRepository>();

            kernel.Bind<ISliderRepository>()
                  .To<SliderRepository>();

            kernel.Bind<ITeamRepository>()
                  .To<TeamRepository>();

            kernel.Bind<ITestimonialRepository>()
                  .To<TestimonialRepository>();

            //Services
            kernel.Bind<IAboutService>()
                 .To<AboutService>();

            kernel.Bind<IBlogService>()
                .To<BlogService>();
        }
    }
}