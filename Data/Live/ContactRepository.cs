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
    public class ContactRepository : DBRepository, IContactRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IContactRepository Me { get; set; }

        #region IContactRepositoryBackingStore Members

        public IEnumerable<IContact> Get(bool first)
        {
            var sql = String.Format("SELECT {0} FROM dbo.[Contact] AS {1}"
                                            , ContactSelectColumns, ContactAlias);

            return UntilDovesCry<IContact>(sql
                                        , first
                                        , AppendFromDataReader
                                        , null
                                        );
        }

        #endregion

        #region IContactRepository Members

        public bool Save(string email, string body, string name, bool subscribed)
        {
            string sql = String.Format("INSERT INTO dbo.[Contact]({0}) "
                                    + " VALUES(@email, @body, @name, @subscribed)"
                                    , ContactInsertColumns);

            var id = UntilDovesCryScalar(sql
                                        , Utility.Parameter("@email", email, true, 255)
                                        , Utility.Parameter("@body", body, true, 2000)
                                        , Utility.Parameter("@name", name, true, 100)
                                        , Utility.Parameter("@subscribed", subscribed));
            return id > -1;
        }

        public bool Delete(IContact contact)
        {
            string sql = String.Format("DELETE FROM dbo.[Contact] WHERE [Email] = @Email AND [Created] = @Created");

            var id = UntilDovesCryScalar(sql
                                        , Utility.Parameter("@Email", contact.Email, true, 255)
                                        , Utility.Parameter("@Created", contact.Created));
            return id > -1;
        }

        public IEnumerable<IContact> Get()
        {
            return Get(true);
        }

        public IEnumerable<IContact> Get(String emailAddress)
        {
            var list = Me.Get();
            return list.Where(contact => contact.Email.Equals(emailAddress, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        private void AppendFromDataReader(IDataReader reader, IList<IContact> result)
        {
            int columnIndex = 0;
            var data = ReadFrom(reader, ref columnIndex);
            result.Add(data);
        }

        internal IContact ReadFrom(IDataReader reader, ref int columnIndex)
        {
            var email = reader.ColumnValue(columnIndex++, String.Empty);
            var body = reader.ColumnValue(columnIndex++, String.Empty);
            var name = reader.ColumnValue(columnIndex++, String.Empty);
            var subscribed = reader.ColumnValue(columnIndex++, false);
            var created = reader.ColumnValue(columnIndex++, DateTime.MinValue);

            return Instantiate(email, body, name, subscribed, created);
        }

        internal IContact Instantiate(String email, String body, String name, bool subscribed, DateTime created)
        {
            var data = Kernel.Get<IContact>();

            data.Email = email;
            data.Body = body;
            data.Name = name;
            data.Subscribed = subscribed;
            data.Created = created;

            return data;
        }
    }
}
