using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.Core.DataAccess
{
    public interface IDataAccess<T>
    {
        List<T> List(Expression<Func<T, bool>> where);

        List<T> List();

        IQueryable<T> ListQueryable();

        T Find(Expression<Func<T, bool>> where);

        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Save();
    }
}