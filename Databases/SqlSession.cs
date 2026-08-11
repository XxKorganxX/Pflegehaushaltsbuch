using Pflegehaushaltsbuch.Data;
using System;
using System.Linq;

namespace Pflegehaushaltsbuch.Databases
{
    public class SqlSession : IDisposable
    {
        public SQLBase SQL { get; private set; }
        public bool IsConnected => SQL != null;
        public User User => SQL?.User;

        public void Replace(SQLBase sql)
        {
            if (ReferenceEquals(SQL, sql))
                return;

            SQLBase previousSql = SQL;
            SQL = sql;
            previousSql?.Dispose();
        }

        public void Disconnect()
        {
            Replace(null);
        }

        public SQLBase Detach()
        {
            SQLBase detachedSql = SQL;
            SQL = null;
            return detachedSql;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
