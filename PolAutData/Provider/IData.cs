using System.Collections;
using System.Data;

namespace PolAutData
{
    interface IData
    {
        bool Open();
        bool Close();
        /// <summary>
        /// Begins transaction.
        /// </summary>
        /// <returns>true if transaction is open.</returns>
        bool BeginTran();
        bool CommitTran();
        bool RollbackTran();
        bool InTransaction();
        bool GetDataSet(string query, Hashtable parameters, out DataSet queryResult);
        bool GetDataSet(string query, out DataSet queryResult);
        bool Execute(string query, Hashtable parameters);
    }
}
