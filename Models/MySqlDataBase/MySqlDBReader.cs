using DelitaTrade.Models.Loggers;
using MySql.Data.MySqlClient;
using System.Data;

namespace DelitaTrade.Models.MySqlDataBase
{
    public enum MySqlReadCommand 
    {
        AllCompanies,
        AllObjects,
        AllVehicles,
        AllDayReports,
        AllPayDesks,
        AllInvoicesInDayReport,
        AllInvoiceInCurrentDayReport,
        IsNewInvoice,
        GetIdByInvoiceId,
        AllProducts
    }


    public class MySqlDBReader
    {
        private Dictionary<MySqlReadCommand, string> _readCommands = new()
        {
            [MySqlReadCommand.AllCompanies] = "get_companies_with_out_objects",
            [MySqlReadCommand.AllObjects] = "get_all_objects",
            [MySqlReadCommand.AllVehicles] = "get_all_vehicles",
            [MySqlReadCommand.AllDayReports] = "get_all_day_reports",
            [MySqlReadCommand.AllPayDesks] = "get_all_day_reports_pay_desk",
            [MySqlReadCommand.AllInvoicesInDayReport] = "get_all_invoices_in_day_reports",
            [MySqlReadCommand.AllInvoiceInCurrentDayReport] = "get_all_invoice_in_day_report",
            [MySqlReadCommand.IsNewInvoice] = "is_new_invoice",
            [MySqlReadCommand.GetIdByInvoiceId] = "get_all_invoices_id",
            [MySqlReadCommand.AllProducts] = "get_all_products"
        };
        private Dictionary<MySqlReadCommand, int> _readDataParameter = new()
        {
            [MySqlReadCommand.AllCompanies] = 3,
            [MySqlReadCommand.AllObjects] = 4,
            [MySqlReadCommand.AllVehicles] = 2,
            [MySqlReadCommand.AllDayReports] = 0,
            [MySqlReadCommand.AllPayDesks] = 0,
            [MySqlReadCommand.AllInvoicesInDayReport] = 0,
            [MySqlReadCommand.AllInvoiceInCurrentDayReport] = 0,
            [MySqlReadCommand.IsNewInvoice] = 0,
            [MySqlReadCommand.GetIdByInvoiceId] = 0,
            [MySqlReadCommand.AllProducts] = 0
        };
        private Dictionary<MySqlReadCommand, bool> _readCommandWithParams = new()
        {
            [MySqlReadCommand.AllCompanies] = false,
            [MySqlReadCommand.AllObjects] = false,
            [MySqlReadCommand.AllVehicles] = false,
            [MySqlReadCommand.AllDayReports] = true,
            [MySqlReadCommand.AllPayDesks] = true,
            [MySqlReadCommand.AllInvoicesInDayReport] = true,
            [MySqlReadCommand.AllInvoiceInCurrentDayReport] = true,
            [MySqlReadCommand.IsNewInvoice] = true,
            [MySqlReadCommand.GetIdByInvoiceId] = true,
            [MySqlReadCommand.AllProducts] = false
        };
        private MySqlCommand _reader;
        private MySqlReadCommand _command;

        public MySqlDBReader(MySqlReadCommand command, MySqlDBConnection connection)
        {
            _command = command;
            _reader = new MySqlCommand(_readCommands[_command], connection.MySqlConnection);
            _reader.CommandType = CommandType.StoredProcedure;
        }

        public IEnumerable<string> GetAllData()
        {
            List<string> data = new List<string>();
            try
            {                
                _reader.Connection.Open();
                MySqlDataReader reader = _reader.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(reader.GetString(_readDataParameter[_command]));
                }
            }
            catch (MySqlException ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error).Log(ex,Logger.LogLevel.Error);
            }
            finally
            {
                _reader.Connection.Close();
                _reader.Parameters.Clear();
            }
            return data;
        }

        public IEnumerable<string> GetAllData(MySqlParameter[] sqlParameters)
        {
            if (_readCommandWithParams[_command])
            { 
                _reader.Parameters.AddRange(sqlParameters);
            }
            return GetAllData();
        }
    }
}
