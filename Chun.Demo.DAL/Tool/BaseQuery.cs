using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Chun.Demo.ICommon;
using Chun.Demo.Model.Entity;

namespace Chun.Demo.DAL
{
    public class BaseDataQuery<T> : IBaseDataQuery<T> where T : class, new()
    {
        phrasehtmlEntities db;
        public BaseDataQuery ( )
        {
            this.db = new phrasehtmlEntities( );
        }

        public phrasehtmlEntities Context => db;

        #region  IBaseDataQuery
        public T Add ( T model )
        {
            db.Set<T>( ).Add(model);

            db.SaveChanges( );

            return model;
        }
        public T Update ( T model )
        {
            if (db.Entry(model).State == EntityState.Modified)
            {
                db.SaveChanges();
            }
            else if (db.Entry(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    //T old = Find(model._ID);
                    //db.Entry(old).CurrentValues.SetValues(model);
                }
                db.SaveChanges();
            }
            return model;
        }

        public void Delete ( T model )
        {
            db.Set<T>( ).Remove(model);
            db.SaveChanges( );
        }
        public void Delete ( params object[] keyValues )
        {
            T model = Find(keyValues);
            if (model != null)
            {
                db.Set<T>( ).Remove(model);
                db.SaveChanges( );
            }
        }
        public T Find ( params object[] keyValues )
        {
            return db.Set<T>( ).Find(keyValues);
        }
        public List<T> FindAll ( )
        {
            return db.Set<T>( ).ToList( );
        }

        /// <summary>
        /// LINQ执行查询返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public List<T> QueryByLinq ( Expression<Func<T, bool>> fun )
        {
            #region 
            //sql查询
            // string sql = "select distinct file_path from filepath where file_type_id= " + type.ToString( ) + " and file_status_id in ( " + (file_status == 0 ? "0,2" : (file_status == 1 ? "1" : "0,1,2")) + ")";
            ////返回实体
            //db.filepath.SqlQuery(sql)
            ////返回其他类型
            //db.database.sqlquery(sql)
            #endregion

            return db.Set<T>( )
                  .Where(fun).Distinct( )
                  .ToList( );
        }
        #endregion

        #region LINQ表的增删改查


        public T AddEntity( T entity )
        {
            //EF4.0的写法  
            //添加实体
            //db.CreateObjectSet<T>().AddObject(entity);
            //EF5.0的写法
            db.Entry(entity).State = EntityState.Added;
            //下面的写法统一
            db.SaveChanges( );
            return entity;
        }
        public bool UpdateEntity( T entity )
        {
            //EF4.0的写法
            //db.CreateObjectSet<T>().Addach(entity);
            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
            //EF5.0的写法 
            db.Set<T>( ).Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            return db.SaveChanges( ) > 0;
        }
        public bool DeleteEntity( T entity )
        {
            //EF4.0的写法 
            //db.CreateObjectSet<T>().Addach(entity);
            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Deleted);
            //EF5.0的写法
            db.Set<T>( ).Attach(entity);
            db.Entry(entity).State = EntityState.Deleted;
            return db.SaveChanges( ) > 0;
        }
        #endregion


        /// <summary>
        /// 增删改查，用sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        //EF5.0的写法
        public void ExecuteSql ( string sql,object[] pars )
        {
            try
            {
                //解决可能的超时
                 db.Database.CommandTimeout = 600;
                 db.Database.ExecuteSqlCommand(sql, pars);
            }
            catch (Exception ex)
            {
                Console.WriteLine("更新出错了！+ 错误信息 {0} 错误详情： {1}", ex.Message,ex.Data);
            }
         
        }



        //简单查询
        public IQueryable<T> LoadEntities( Expression<Func<T, bool>> whereLambda )
        {
            return db.Set<T>( ).Where(whereLambda).AsQueryable( );
        }
        //分页查询 ***
        public IQueryable<T> LoadPageEntities<TS> ( int pageSize, int pageIndex, out int totalCount, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, TS>> orderBy )
        {
            IQueryable<T> result = db.Set<T>( ).Where(whereLambda).AsQueryable( );
            totalCount = result.Count( );  //返回总记录条数
            if (isAsc)
            {
                result = result.OrderBy(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .AsQueryable( );
            }
            else
            {
                result = result.OrderByDescending(orderBy)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .AsQueryable( );
            }
            return result;
        }
    }
}