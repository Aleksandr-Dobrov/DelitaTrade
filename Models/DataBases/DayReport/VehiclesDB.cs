using DelitaTrade.Models.Interfaces.DataBase;

namespace DelitaTrade.Models.DataBases.DayReport
{
    public class VehiclesDB : IDBIdData
    {
        private int _id;
        private string _licensePlate;

        public int Id => _id;
        public string Parameters => throw new NotImplementedException();
        public string Data => throw new NotImplementedException();
        public string Procedure => throw new NotImplementedException();
        public int NumberOfReferences => throw new NotImplementedException();
    }
}
