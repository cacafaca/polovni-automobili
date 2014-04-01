using System.Collections;
using System.Data;

namespace PolAutData
{
    interface IData
    {
        bool Open();
        bool Close();
        bool BeginTran();
        bool CommitTran();
        bool RollbackTran();
        bool InTransaction();
        bool GetDataSet(string query, Hashtable parameters, DataSet queryResult);
        bool Execute(string query, Hashtable parameters);
    }
}
