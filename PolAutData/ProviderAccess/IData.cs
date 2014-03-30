using System.Collections;
using System.Data;

namespace PolAutData
{
    interface IData
    {
        Data GetDataInstance();
        bool Open();
        bool Close();
        bool BeginTran();
        bool CommitTran();
        bool RollbackTran();
        bool GetDataSet(string query, Hashtable parameters, DataSet queryResult);
        bool Execute(string query, Hashtable parameters);
    }
}
