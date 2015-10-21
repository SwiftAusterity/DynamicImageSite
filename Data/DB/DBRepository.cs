using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

using Infuz.API.SQL;
using Infuz.Utilities;

using Ninject;

namespace Site.Data.DB
{
    public abstract class DBRepository
    {
        [Inject]
        public ISqlServiceProvider SqlProvider { get; set; }

        public const String UserAlias = "usr";
        public const String UserSelectColumns = " usr.[ID], usr.[Name], usr.[Permissions] ";
        public const String UserInsertColumns = " [ID], [Name], [Permissions] ";

        public const String ContactAlias = "con";
        public const String ContactSelectColumns = " con.[Email], con.[Body], con.[Name], con.[Subscribed], con.[Created]";
        public const String ContactInsertColumns = " [Email], [Body], [Name], [Subscribed] ";
        
        public IList<TElement> UntilDovesCry<TElement>(string sql
            , bool initialLoad
            , Action<IDataReader, IList<TElement>> appendAction
            , params SqlParameter[] parameters)
        {
            var maxChances = initialLoad ? 10 : 3;
            IList<TElement> listFirst = null;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    listFirst = new List<TElement>();
                    SqlProvider.ExecuteDataReader(CommandType.Text
                                                  , sql
                                                  , reader => appendAction(reader, listFirst)
                                                  , parameters);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.IsWorthRetry())
                        Thread.Sleep(10);
                    else
                        throw;
                }
            }

            return listFirst;
        }

        public long UntilDovesCry(string sql, SqlParameter[] parameters)
        {
            string lastException = String.Empty;
            var maxChances = 3;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    return SqlProvider.ExecuteScalar(CommandType.Text
                                                        , sql
                                                        , default(int)
                                                        , parameters);
                }
                catch (Exception ex)
                {
                    if (!ex.IsWorthRetry())
                        throw;

                    lastException = ex.Message;
                    Thread.Sleep(10);
                }
            }

            throw new InvalidOperationException("Unable to call " + sql + " " + lastException);
        }

        public int UntilDovesCryScalar(string sql, params SqlParameter[] parameters)
        {
            return UntilDovesCryScalar<int>(sql, parameters);
        }

        public T UntilDovesCryScalar<T>(string sql, params SqlParameter[] parameters)
        {
            string lastException = String.Empty;
            var maxChances = 3;

            for (var chance = 0; chance < maxChances; chance++)
            {
                try
                {
                    return SqlProvider.ExecuteScalar<T>(CommandType.Text
                                                      , sql
                                                      , default(T)
                                                      , parameters);
                }
                catch (Exception ex)
                {
                    if (!ex.IsWorthRetry())
                        throw;

                    lastException = ex.Message;
                    Thread.Sleep(10);
                }
            }

            throw new InvalidOperationException("Unable to call " + sql + " " + lastException);
        }

    }
}
