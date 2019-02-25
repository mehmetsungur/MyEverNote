using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class Repository<T> : RepositoryBase, IDataAccess<T> where T : class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>();
        }

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public List<T> List()
        {
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);

            if (obj is MyEntitiyBase)
            {
                MyEntitiyBase o = obj as MyEntitiyBase;
                DateTime now = DateTime.Now;

                o.CreateOn = now;
                o.ModifiedOn = now;
                o.ModifedUserName = App.Common.GetCurrentUserName();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntitiyBase)
            {
                MyEntitiyBase o = obj as MyEntitiyBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifedUserName = "system";
            }

            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);

            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}