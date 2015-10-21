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
    public class TextCommandRepository : DBRepository, ITextCommandRepositoryBackingStore
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public ITextCommandRepository Me { get; set; }

        #region ItextcommandRepository Members

        public bool Save(String command, String path, bool handled)
        {
            string sql = "INSERT INTO dbo.[TextCommandHistory]([Command], [Path], [Handled]) VALUES(@command, @path, @handled)";

            var id = UntilDovesCryScalar(sql
                                        , Utility.Parameter("@command", command, true, 2000)
                                        , Utility.Parameter("@path", path, true, 2000)
                                        , Utility.Parameter("@handled", handled));
            return id > -1;
        }

        #endregion
    }
}
