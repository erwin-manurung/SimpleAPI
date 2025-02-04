using System.Data;
#if DEBUG
    using System.Text;
#endif
namespace Allegro.BuildTasks.Test
{
    public class MainWS
    {
#if DEBUG
        private string _GetAnotherVersion()
        {
            return "24.0.1";
        }
#endif

        private string _GetVersion()
        {
            return "24.0.1";
        }
        private string _GetVersionWithParam(string param = "ok")
        {
            return "24.0.1-"+param;
        }
        private DataSet _RetrieveData(DataSet dataSet)
        {
            DataSet ret = new DataSet();
            DataTable dataTable = new DataTable("simpletable");
            dataTable.Columns.Add(new DataColumn("simpleid"));
            dataTable.Columns.Add(new DataColumn("simplename"));
            dataTable.AcceptChanges();
            ret.Tables.Add(dataTable);
            return ret;
        }

        private DataSet _RetrieveData(DataSet dsRetrieve, string[] tableNames)
        {
            DataSet ret = new DataSet();
            DataTable dataTable = new DataTable("dummy");
            dataTable.Columns.Add("dummy");
            dataTable.Rows.Add("ok");
            dataTable.AcceptChanges();
            ret.Tables.Add(dataTable);
            return ret;
        }
        private DataSet _AnotherRetrieveData(DataSet dsRetrieve, string[] tableNames)
        {
            DataSet ret = new DataSet();
            DataTable dataTable = new DataTable("dummy");
            dataTable.Columns.Add("dummy");
            dataTable.Rows.Add("ok");
            dataTable.AcceptChanges();
            ret.Tables.Add(dataTable);
            return ret;
        }
    }
}
