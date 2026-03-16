using FluentValidation;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Mvc_LifeSure_DbFirst.Data.Context;

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
using Mvc_LifeSure_DbFirst.Services.FaqServices;
using Mvc_LifeSure_DbFirst.Services.FeatureServices;
using Mvc_LifeSure_DbFirst.Services.ServiceServices;
using Mvc_LifeSure_DbFirst.Services.SliderServices;
using Mvc_LifeSure_DbFirst.Services.TeamServices;
using Mvc_LifeSure_DbFirst.Services.TestimonialServices;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;
using System;
using System.Linq;
using System.Reflection;
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
            kernel.Bind<AppDbContext>()
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

            kernel.Bind<IServicesRepository>()
                  .To<ServicesRepository>();

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

            kernel.Bind<IFaqService>()
                .To<FaqService>();

            kernel.Bind<IFeatureService>()
                .To<FeatureService>();

            kernel.Bind<IServicesService>()
               .To<ServicesService>();

            kernel.Bind<ISliderService>()
               .To<SliderService>();

            kernel.Bind<ITeamService>()
               .To<TeamService>();

            kernel.Bind<ITestimonialService>()
              .To<TestimonialService>();


            var validators = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => !t.IsAbstract && !t.IsInterface)
    .SelectMany(t => t.GetInterfaces(),
        (t, i) => new { Validator = t, Interface = i })
    .Where(x =>
        x.Interface.IsGenericType &&
        x.Interface.GetGenericTypeDefinition() == typeof(IValidator<>));

            foreach (var validator in validators)
            {
                kernel.Bind(validator.Interface)
                      .To(validator.Validator)
                      .InTransientScope();
            }
        }
    }
}