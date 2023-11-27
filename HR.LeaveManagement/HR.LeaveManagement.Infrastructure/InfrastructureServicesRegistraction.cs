using HR.LeaveManagement.Application.Contracts.Infrastructure;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Infrastructure.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Infrastructure
{
	public static class InfrastructureServicesRegistraction
	{
		public static IServiceCollection ConfigureInfrastructuresService(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.EmailSetting));
			services.AddTransient<IEmailSender, EmailSender>();

			return services;
		}
	}
}
