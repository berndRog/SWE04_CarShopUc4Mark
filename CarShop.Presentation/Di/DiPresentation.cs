using CarShop.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace CarShop.Di; 
 public static class DiPresentation {
     public static IServiceCollection AddPresentation(this IServiceCollection services) {
         services.AddSingleton<IDialog, CDialog>();
         return services;
     } 
 }
