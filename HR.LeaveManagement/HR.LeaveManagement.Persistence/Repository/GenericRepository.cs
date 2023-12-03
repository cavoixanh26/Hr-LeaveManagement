using HR.LeaveManagement.Application.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly LeaveManagementDbContext context;

		public GenericRepository(LeaveManagementDbContext context)
		{
			this.context = context;
		}

		public async Task<T> AddAsync(T entity)
		{
			await context.AddAsync(entity);
			return entity;
		}

		public async Task DeleteAsync(T entity)
		{
			context.Set<T>().Remove(entity);
		}

		public async Task<bool> Exists(int id)
		{
			var entity = await GetAsync(id);
			return entity != null;
		}

		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await context.Set<T>().ToListAsync();
		}

		public async Task<T> GetAsync(int id)
		{
			return await context.Set<T>().FindAsync(id);
		}

		public async Task UpdateAsync(T entity)
		{
			context.Entry(entity).State = EntityState.Modified;
		}
	}
}
