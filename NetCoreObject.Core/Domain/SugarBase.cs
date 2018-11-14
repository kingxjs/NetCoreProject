using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Data;

namespace NetCoreObject.Core
{
    public  class SugarBase
    {
        public static SqlSugarClient _db;
        public static string ConnectionString;
        public SugarBase()
        {
            //_db = InitDB(config,30, DBType.MySql, true);
        }
        public static void SetConnectionString(IConfiguration config) {
            ConnectionString = config.GetConnectionString("DefaultConnection");
        }
        /// <summary>
        /// 服务命令
        /// </summary>
        /// <typeparam name="TOutsourcing">外包对象</typeparam>
        /// <param name="func"></param>
        public void Command<TOutsourcing>(Action<SqlSugarClient, TOutsourcing> func) where TOutsourcing : class, new()
        {
            try
            {
                _db = InitDB(30, DBType.MySql, true);
                var o = new TOutsourcing();
                func(_db, o);
                o = null;//及时释放对象 
                //_db 会在http请求结束前执行 dispose 
            }
            catch (Exception ex)
            {
                //在这里可以处理所有controller的异常
                //获错误写入日志
                
                throw;
            }
        }

        /// <summary>
        /// 获得SqlSugarClient(使用该方法, 默认请手动释放资源, 如using(var db = SugarBase.GetIntance()){你的代码}, 如果把isAutoCloseConnection参数设置为true, 则无需手动释放, 会每次操作数据库释放一次, 可能会影响性能, 请自行判断使用)
        /// </summary>
        /// <param name="commandTimeOut">等待超时时间, 默认为30秒 (单位: 秒)</param>
        /// <param name="dbType">数据库类型, 默认为SQL Server</param>
        /// <param name="isAutoCloseConnection">是否自动关闭数据库连接, 默认不是, 如果设置为true, 则会在每次操作完数据库后, 即时关闭, 如果一个方法里面多次操作了数据库, 建议保持为false, 否则可能会引发性能问题</param>
        /// <returns></returns>
        /// <author>旷丽文</author>
        public static SqlSugarClient GetIntance(int commandTimeOut = 30, DBType dbType = DBType.MySql, bool isAutoCloseConnection = false)
        {
            return _db;
        }
        //public static SqlSugarClient GetIntance(IConfiguration config)
        //{
        //    _db = InitDB(config, 30, DBType.MySql, true);
        //    return _db;
        //}
        public static SqlSugarClient GetIntance()
        {
            _db = InitDB(30, DBType.MySql, true);
            return _db;
        }
        /// <summary>
        /// 初始化ORM连接对象, 一般无需调用, 除非要自己写很复杂的数据库逻辑
        /// </summary>
        /// <param name="commandTimeOut">等待超时时间, 默认为30秒 (单位: 秒)</param>
        /// <param name="dbType">数据库类型, 默认为SQL Server</param>
        /// <param name="isAutoCloseConnection">是否自动关闭数据库连接, 默认不是, 如果设置为true, 则会在每次操作完数据库后, 即时关闭, 如果一个方法里面多次操作了数据库, 建议保持为false, 否则可能会引发性能问题</param>
        /// <author>旷丽文</author>
        private static SqlSugarClient InitDB(int commandTimeOut = 30, DBType dbType = DBType.MySql, bool isAutoCloseConnection = false)
        {

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConnectionString,
                DbType = dbType == DBType.SqlServer ? SqlSugar.DbType.SqlServer : SqlSugar.DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = isAutoCloseConnection
            });
            db.Ado.CommandTimeOut = commandTimeOut;
            return db;
        }

        /// <summary>
        /// 执行数据库操作
        /// </summary>
        /// <typeparam name="Result">返回值类型</typeparam>
        /// <param name="func">方法体</param>
        /// <returns></returns>
        /// <author>旷丽文</author>
        public static Result Exec<Result>(Func<SqlSugarClient, Result> func, int commandTimeOut = 30, DBType dbType = DBType.MySql)
        {
            if (func == null) throw new Exception("委托为null, 事务处理无意义");
            using (var db = _db)
            {
                try
                {
                    return func(db);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// 带事务处理的执行数据库操作
        /// </summary>
        /// <typeparam name="Result">返回值类型</typeparam>
        /// <param name="func">方法体</param>
        /// <returns></returns>
        /// <author>旷丽文</author>
        public static Result ExecTran<Result>(Func<SqlSugarClient, Result> func, int commandTimeOut = 30, DBType dbType = DBType.MySql)
        {
            if (func == null) throw new Exception("委托为null, 事务处理无意义");
            using (var db = _db)
            {
                try
                {
                    db.Ado.BeginTran(IsolationLevel.Unspecified);
                    var result = func(db);
                    db.Ado.CommitTran();
                    return result;
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw ex;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DBType
    {
        SqlServer = 1,

        MySql = 2
    }
}