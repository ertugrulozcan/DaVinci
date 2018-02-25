using Ertis.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Services.Contracts
{
    public interface ISettingsService
    {
        AppSettings GetLastSettings();

        AppSettings GetUserSettings(int userId);

        void SaveUserSettings(int userId);

        void SaveUserSettings(AppSettings settings);

        AppSettings CreateInitialSettings(int userId);
    }
}
