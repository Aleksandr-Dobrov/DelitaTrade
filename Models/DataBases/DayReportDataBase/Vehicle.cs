using DelitaTrade.Models.Interfaces.DataBase;

namespace DelitaTrade.Models.DataBases.DayReportDataBase
{
    public class Vehicle : IDBData
    {
        private const int _numberOfReferences = 0;
        private string _licensePlate;
        private string _model;

        public Vehicle(string licensePlate, string model)
        {
            _licensePlate = licensePlate.ToUpper();
            _model = model;
        }

        public string LicensePlate => _licensePlate;
        public string Model => _model;

        public string Parameters => "vehicle_license_plate-=-vehicle_model";
        public string Data => $"{LicensePlate}-=-{((Model == null || Model == string.Empty) ? "няма запис" : $"{Model}")}";
        public string Procedure => "add_vehicle";
        public int NumberOfAdditionalParameters => _numberOfReferences;

        public override string ToString()
        {
            return LicensePlate;
        }

        public override int GetHashCode()
        {
            return LicensePlate.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var vehicle = obj as Vehicle;
            return vehicle?.LicensePlate == LicensePlate;
        }
    }
}
