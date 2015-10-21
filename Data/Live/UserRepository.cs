using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Infuz.API.SQL;
using Infuz.Utilities;

using Ninject;

using Site.Data.API;
using Site.Data.API.Repository;
using Site.Data.DB;
using Site.Data.DTO;

namespace Site.Data.Live
{
    public class UserRepository : DBRepository, IUserRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IUserRepository Me { get; set; }

        public bool Save(string source, string url)
        {
            string sql = string.Format("INSERT INTO dbo.[User]({0}) "
                                    + " VALUES(@source, @url)", UserInsertColumns);

            var id = UntilDovesCryScalar(sql
                                        , Utility.Parameter("@source", source, false, 50)
                                        , Utility.Parameter("@url", url, true, 128));
            return id > -1;
        }

        public bool Delete(IUser user)
        {
            string sql = string.Format("DELETE FROM dbo.[User] WHERE [ID] = @ID");

            var id =  UntilDovesCryScalar(sql
                                        , Utility.Parameter("@ID", user.ID));
            return id > -1;
        }

        public IEnumerable<IUser> Get(bool first)
        {
            var sql = string.Format("SELECT {0} FROM dbo.[User] AS {1}"
                                            , UserSelectColumns, UserAlias);

            return UntilDovesCry<IUser>(sql
                                        , first
                                        , AppendFromDataReader
                                        , null
                                        );
        }

        public IEnumerable<IUser> Get()
        {
            return Get(true);
        }

        public IUser Get(Guid userId)
        {
            return Me.Get().FirstOrDefault(user => user.ID.Equals(userId));
        }

        private void AppendFromDataReader(IDataReader reader, IList<IUser> result)
        {
            int columnIndex = 0;
            var data = ReadFrom(reader, ref columnIndex);
            result.Add(data);
        }

        internal IUser ReadFrom(IDataReader reader, ref int columnIndex)
        {
            var id = reader.ColumnValue(columnIndex++, Guid.Empty);
            var name = reader.ColumnValue(columnIndex++, string.Empty);
            var permissions = reader.ColumnValue(columnIndex++, string.Empty);

            return Instantiate(id, name, permissions);
        }

        internal IUser ToSingle(DataRow currentRow)
        {
            var id = currentRow["id"].ColumnValue<Guid>(Guid.Empty);
            var name = currentRow["name"].ColumnValue<string>(string.Empty);
            var permissions = currentRow["permissions"].ColumnValue<string>(string.Empty);

            return Instantiate(id, name, permissions);
        }

        internal IUser Instantiate(Guid id, string name, string permissions)
        {
            var data = Kernel.Get<IUser>();

            data.ID = id;
            data.Name = name;
            data.Permissions = permissions;

            return data;
        }
    }
}
