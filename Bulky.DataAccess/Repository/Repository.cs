﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;

		internal DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
            _db= db;
			this.dbset=_db.Set<T>();     //dbset got all dbset data from applicationdbcontext now using dbset you can perform all the operatins
			_db.Products.Include(u => u.category);
        }
        public void Add(T entity)
		{
			dbset.Add(entity);
   		}

		public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null)
		{
			IQueryable<T> query = dbset;
			query = query.Where(filter);
			if (!string.IsNullOrEmpty(includeproperties))
			{
				foreach (var includeProp in includeproperties
					.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}
			return query.FirstOrDefault();
		}
		public IEnumerable<T> GetAll(string? includeproperties =null)
		{
			IQueryable<T> query = dbset;
			if (!string.IsNullOrEmpty(includeproperties))
			{
				foreach(var includeProp in includeproperties
					.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeProp);
				}
			}
			return query.ToList();
		}

		public void Remove(T entity)
		{
			dbset.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entity)
		{
			dbset.RemoveRange(entity);
		}
	}
}
