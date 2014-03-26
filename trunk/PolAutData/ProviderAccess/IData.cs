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
        DataSet GetDataSet(string query, Hashtable parameters);
        bool Execute(string query, Hashtable parameters);
    }
}
