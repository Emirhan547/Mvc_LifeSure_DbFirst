using FluentValidation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Data.Identity;
using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using Mvc_LifeSure_DbFirst.Dtos.AppUserDtos;
using Mvc_LifeSure_DbFirst.Dtos.ContactMessageDtos;
using Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos;
using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using Mvc_LifeSure_DbFirst.Repositories.AboutRepositories;
using Mvc_LifeSure_DbFirst.Repositories.AdminLogRepositories;
using Mvc_LifeSure_DbFirst.Repositories.BlogRepositories;
using Mvc_LifeSure_DbFirst.Repositories.ContactMessageRepositories;
using Mvc_LifeSure_DbFirst.Repositories.FaqRepositories;
using Mvc_LifeSure_DbFirst.Repositories.FeatureRepositories;
using Mvc_LifeSure_DbFirst.Repositories.InsurancePackageRepositories;
using Mvc_LifeSure_DbFirst.Repositories.PolicyRepositories;
using Mvc_LifeSure_DbFirst.Repositories.PolicySaleDataRepositories;
using Mvc_LifeSure_DbFirst.Repositories.ServiceRepositories;
using Mvc_LifeSure_DbFirst.Repositories.SliderRepositories;
using Mvc_LifeSure_DbFirst.Repositories.TeamRepositories;
using Mvc_LifeSure_DbFirst.Repositories.TestimonialRepositories;
using Mvc_LifeSure_DbFirst.Services.AboutServices;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AIServices;
using Mvc_LifeSure_DbFirst.Services.AppUserServices;
using Mvc_LifeSure_DbFirst.Services.BlogServices;
using Mvc_LifeSure_DbFirst.Services.ContactMessageServices;
using Mvc_LifeSure_DbFirst.Services.FaqServices;
using Mvc_LifeSure_DbFirst.Services.FeatureServices;
using Mvc_LifeSure_DbFirst.Services.InsurancePackageServices;
using Mvc_LifeSure_DbFirst.Services.MLServices;
using Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices;
using Mvc_LifeSure_DbFirst.Services.PolicyServices;
using Mvc_LifeSure_DbFirst.Services.ServiceServices;
using Mvc_LifeSure_DbFirst.Services.SliderServices;
using Mvc_LifeSure_DbFirst.Services.TeamServices;
using Mvc_LifeSure_DbFirst.Services.TestimonialServices;
using Mvc_LifeSure_DbFirst.Validators.AboutValidators;
using Mvc_LifeSure_DbFirst.Validators.AppUserValidators;
using Mvc_LifeSure_DbFirst.Validators.ContactMessageValidators;
using Mvc_LifeSure_DbFirst.Validators.InsurancePackageValidators;
using Mvc_LifeSure_DbFirst.Validators.PolicyValidators;
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
            kernel.Bind<AppDbContext>().ToSelf().InRequestScope();

            kernel.Bind<IUserStore<AppUser, string>>().ToMethod(context =>
                 new UserStore<AppUser, AppRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>(
                     context.Kernel.Get<AppDbContext>()))
                 .InRequestScope();
            kernel.Bind<IRoleStore<AppRole, string>>().To<RoleStore<AppRole>>().InRequestScope();

            // AppUserManager binding'i
            kernel.Bind<AppUserManager>().ToMethod(context =>
            {
                var userStore = context.Kernel.Get<IUserStore<AppUser, string>>();
                return new AppUserManager(userStore);
            }).InRequestScope();

            // UserManager<AppUser, string> binding'i (AppUserService için gerekli)
            kernel.Bind<UserManager<AppUser, string>>().ToMethod(context =>
            {
                var userStore = context.Kernel.Get<IUserStore<AppUser>>();
                return new UserManager<AppUser, string>(userStore);
            }).InRequestScope();

            // AppSignInManager binding'i
            kernel.Bind<AppSignInManager>().ToMethod(context =>
            {
                var userManager = context.Kernel.Get<AppUserManager>();
                var authManager = context.Kernel.Get<IAuthenticationManager>();
                return new AppSignInManager(userManager, authManager);
            }).InRequestScope();

            // AuthenticationManager
            kernel.Bind<IAuthenticationManager>().ToMethod(context =>
                HttpContext.Current.GetOwinContext().Authentication).InRequestScope();

            // Repository Bindings
            kernel.Bind<IAboutRepository>().To<AboutRepository>();
            kernel.Bind<IBlogRepository>().To<BlogRepository>();
            kernel.Bind<IFaqRepository>().To<FaqRepository>();
            kernel.Bind<IFeatureRepository>().To<FeatureRepository>();
            kernel.Bind<IServicesRepository>().To<ServicesRepository>();
            kernel.Bind<ISliderRepository>().To<SliderRepository>();
            kernel.Bind<ITeamRepository>().To<TeamRepository>();
            kernel.Bind<ITestimonialRepository>().To<TestimonialRepository>();
            kernel.Bind<IInsurancePackageRepository>().To<InsurancePackageRepository>();
            kernel.Bind<IPolicyRepository>().To<PolicyRepository>();
            kernel.Bind<IContactMessageRepository>().To<ContactMessageRepository>();
            kernel.Bind<IAdminLogRepository>().To<AdminLogRepository>();
            kernel.Bind<IPolicySaleDataRepository>().To<PolicySaleDataRepository>();

            // Service Bindings
            kernel.Bind<IAboutService>().To<AboutService>();
            kernel.Bind<IBlogService>().To<BlogService>();
            kernel.Bind<IFaqService>().To<FaqService>();
            kernel.Bind<IFeatureService>().To<FeatureService>();
            kernel.Bind<IServicesService>().To<ServicesService>();
            kernel.Bind<ISliderService>().To<SliderService>();
            kernel.Bind<ITeamService>().To<TeamService>();
            kernel.Bind<ITestimonialService>().To<TestimonialService>();
            kernel.Bind<IAppUserService>().To<AppUserService>();
            kernel.Bind<IInsurancePackageService>().To<InsurancePackageService>();
            kernel.Bind<IPolicyService>().To<PolicyService>();
            kernel.Bind<IContactMessageService>().To<ContactMessageService>();
            kernel.Bind<IAdminLogService>().To<AdminLogService>();
            kernel.Bind<IPolicySaleDataService>().To<PolicySaleDataService>();
            kernel.Bind<IForecastService>().To<ForecastService>().InRequestScope();

            // AI Servisleri
            kernel.Bind<IHuggingFaceService>().To<HuggingFaceService>().InRequestScope();
            kernel.Bind<IChatGPTService>().To<ChatGPTService>().InRequestScope();
            kernel.Bind<IMailService>().To<MailService>().InRequestScope();
            kernel.Bind<IGeminiService>().To<GeminiService>().InRequestScope();
            kernel.Bind<ITavilyService>().To<TavilyService>().InRequestScope();

            // VALIDATOR BINDINGS - MANUEL OLARAK EKLİYORUZ (Otomatik bulma çalışmazsa diye)
            kernel.Bind<IValidator<CreateAboutDto>>().To<CreateAboutValidator>().InTransientScope();
            kernel.Bind<IValidator<UpdateAboutDto>>().To<UpdateAboutValidator>().InTransientScope();

            kernel.Bind<IValidator<RegisterDto>>().To<RegisterValidator>().InTransientScope();
            kernel.Bind<IValidator<LoginDto>>().To<LoginValidator>().InTransientScope();
            kernel.Bind<IValidator<UpdateAppUserDto>>().To<UpdateAppUserValidator>().InTransientScope();

            kernel.Bind<IValidator<CreateInsurancePackageDto>>().To<CreateInsurancePackageValidator>().InTransientScope();
            kernel.Bind<IValidator<UpdateInsurancePackageDto>>().To<UpdateInsurancePackageValidator>().InTransientScope();

            kernel.Bind<IValidator<CreatePolicyDto>>().To<CreatePolicyValidator>().InTransientScope();
            kernel.Bind<IValidator<UpdatePolicyDto>>().To<UpdatePolicyValidator>().InTransientScope();

            kernel.Bind<IValidator<CreateContactMessageDto>>().To<CreateContactMessageValidator>().InTransientScope();

            // OTOMATİK VALIDATOR BINDING (Yedek olarak)
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
                // Eğer daha önce manuel eklenmemişse ekle
                if (!kernel.GetBindings(validator.Interface).Any())
                {
                    kernel.Bind(validator.Interface)
                          .To(validator.Validator)
                          .InTransientScope();
                }
            }
        }
    }
}