using System;
using Ertis.WebService.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Ertis.WebService.Services
{
    public class ServiceProvider
    {
        private static ServiceProvider self;
        public static ServiceProvider Current
        {
            get
            {
                if (self == null)
                    self = new ServiceProvider();

                return self;
            }
        }

        private static Microsoft.Extensions.DependencyInjection.ServiceProvider Factory { get; set; }

        private IAuthenticationService authenticationService;
        private IOrganizationService organizationService;
        private ISessionService sessionService;
        private IUserService userService;

        public IAuthenticationService AuthenticationService 
        { 
            get
            {
                if (this.authenticationService == null)
                    this.authenticationService = Factory.GetService<IAuthenticationService>();

                return this.authenticationService;
            }
        }

        public IOrganizationService OrganizationService
        {
            get
            {
                if (this.organizationService == null)
                    this.organizationService = Factory.GetService<IOrganizationService>();

                return this.organizationService;
            }
        }

        public ISessionService SessionService
        {
            get
            {
                if (this.sessionService == null)
                    this.sessionService = Factory.GetService<ISessionService>();

                return this.sessionService;
            }
        }

        public IUserService UserService
        {
            get
            {
                if (this.userService == null)
                    this.userService = Factory.GetService<IUserService>();

                return this.userService;
            }
        }

        private ServiceProvider()
        {
            
        }

        public static ServiceProvider Create(Microsoft.Extensions.DependencyInjection.ServiceProvider factory)
        {
            Factory = factory;
            return Current;
        }
    }
}
