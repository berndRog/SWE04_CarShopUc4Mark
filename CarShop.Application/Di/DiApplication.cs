using CarShop.Application.UseCaseCore;
using CarShop.Application.UseCasesUser;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Di;
public static class DiApplication {
   public static IServiceCollection AddApplication(this IServiceCollection services) {
      services.AddSingleton<IAppCore, CAppCore>();
      services.AddSingleton<IAppUser, CAppUserFacade>();
      services.AddSingleton<IAppCar, CAppCar>();
      services.AddSingleton<CUserRegister>();
      services.AddSingleton<CUserUpdate>();
      services.AddSingleton<CUserRemove>();
      return services;
   }
}