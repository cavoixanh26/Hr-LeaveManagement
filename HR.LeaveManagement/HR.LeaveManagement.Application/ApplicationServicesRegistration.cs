using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HR.LeaveManagement.Application
{
	public static class ApplicationServicesRegistration
	{
		public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
		{
			// register automapper
			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			// register mediatR
			services.AddMediatR(Assembly.GetExecutingAssembly());



			return services;
		}
	}
}
