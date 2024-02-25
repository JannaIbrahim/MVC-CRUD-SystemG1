using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {

        private protected readonly AppDbContext _dbContext;// NULL
        public GenericRepository(AppDbContext dbContext)
        {
            //_dbContext = new AppDbContext();
            _dbContext = dbContext;
        }
        public void Add(T entity)
            => _dbContext.Add(entity);

        public void Update(T entity)
            => _dbContext.Update(entity);


        public void Delete(T entity)
           => _dbContext.Remove(entity);


        public T Get(int id)
        {
            ///return _dbContext.Departments.Where(D=>D.Id == id).FirstOrDefault();
            ///var department = _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
            ///if (department is null)
            ///    department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
            ///return department;

            //return _dbContext.Departments.Find(id);
            return _dbContext.Find<T>(id); // EFCore 3.1 Feature

        }

        public IEnumerable<T> GetAll()
        {
            if (typeof(T) == typeof(Employee))
                return (IEnumerable<T>)_dbContext.Employees.Include(E => E.Department).AsNoTracking().ToList();
            else
                return _dbContext.Set<T>().AsNoTracking().ToList();
        }

    }
}
