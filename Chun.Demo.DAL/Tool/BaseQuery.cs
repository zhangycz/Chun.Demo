using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.DAL
{
    public class BaseDataQuery<T> : IBaseDataQuery<T> where T : class, new()
    {
        public BaseDataQuery()
        {
            Context = new phrasehtmlEntities();
        }

        public phrasehtmlEntities Context { get; }


        /// <summary>
        ///     增删改查，用sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        //EF5.0的写法
        public void ExecuteSql(string sql, object[] pars)
        {
            try
            {
                //解决可能的超时
                Context.Database.CommandTimeout = 18000;
                Context.Database.ExecuteSqlCommand(sql, pars);
            }
            catch (Exception ex)
            {
                Console.WriteLine("更新出错了！+ 错误信息 {0} 错误详情： {1}", ex.Message, ex.Data);
            }
        }


        //简单查询
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Context.Set<T>().Where(whereLambda).AsQueryable();
        }

        //分页查询 ***
        public IQueryable<T> LoadPageEntities<TS>(int pageSize, int pageIndex, out int totalCount,
            Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TS>> orderBy)
        {
            var result = Context.Set<T>().Where(whereLambda).AsQueryable();
            totalCount = result.Count(); //返回总记录条数
            if (isAsc)
            {
                result = result.OrderBy(orderBy)
                    .Skip(pageSize*(pageIndex - 1))
                    .Take(pageSize)
                    .AsQueryable();
            }
            else
            {
                result = result.OrderByDescending(orderBy)
                    .Skip(pageSize*(pageIndex - 1))
                    .Take(pageSize)
                    .AsQueryable();
            }
            return result;
        }

        #region  IBaseDataQuery

        public T Add(T model)
        {
            Context.Set<T>().Add(model);

            Context.SaveChanges();

            return model;
        }

        public T Update(T model)
        {
            if (Context.Entry(model).State == EntityState.Modified)
            {
                Context.SaveChanges();
            }
            else if (Context.Entry(model).State == EntityState.Detached)
            {
                try
                {
                    Context.Set<T>().Attach(model);
                    Context.Entry(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    //T old = Find(model._ID);
                    //db.Entry(old).CurrentValues.SetValues(model);
                }
                Context.SaveChanges();
            }
            return model;
        }

        public void Delete(T model)
        {
            Context.Set<T>().Remove(model);
            Context.SaveChanges();
        }

        public void Delete(params object[] keyValues)
        {
            var model = Find(keyValues);
            if (model != null)
            {
                Context.Set<T>().Remove(model);
                Context.SaveChanges();
            }
        }

        public T Find(params object[] keyValues)
        {
            return Context.Set<T>().Find(keyValues);
        }

        public List<T> FindAll()
        {
            return Context.Set<T>().ToList();
        }

        /// <summary>
        ///     LINQ执行查询返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public IQueryable<T> QueryByLinq(Expression<Func<T, bool>> fun)
        {
            #region

            //sql查询
            // string sql = "select distinct file_path from filepath where file_type_id= " + type.ToString( ) + " and file_status_id in ( " + (file_status == 0 ? "0,2" : (file_status == 1 ? "1" : "0,1,2")) + ")";
            ////返回实体
            //db.filepath.SqlQuery(sql)
            ////返回其他类型
            //db.database.sqlquery(sql)

            #endregion

            return Context.Set<T>()
                .Where(fun)
                .Distinct();
            //    .ToList();
        }
        /// <summary>
        ///     LINQ执行查询返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public IQueryable<T> Query(Expression<Func<T, bool>> fun)
        {
            return Context.Set<T>()
                     .Where(fun).Distinct();
        }

        public List<U> QueryByStoredProcedure<U>(string procedureStr, object[] sqlparms) {
            var articles =
                Context.Database.SqlQuery<U>(procedureStr, sqlparms).ToList();
            return articles;
        }
        #endregion

        #region LINQ表的增删改查

        public T AddEntity(T entity)
        {
            //EF4.0的写法  
            //添加实体
            //db.CreateObjectSet<T>().AddObject(entity);
            //EF5.0的写法
            Context.Entry(entity).State = EntityState.Added;
            //下面的写法统一
            Context.SaveChanges();
            return entity;
        }

        public bool UpdateEntity(T entity)
        {
            //EF4.0的写法
            //db.CreateObjectSet<T>().Addach(entity);
            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
            //EF5.0的写法 
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChanges() > 0;
        }

        public bool DeleteEntity(T entity)
        {
            //EF4.0的写法 
            //db.CreateObjectSet<T>().Addach(entity);
            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Deleted);
            //EF5.0的写法
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Deleted;
            return Context.SaveChanges() > 0;
        }

        #endregion
    }
}