namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportDayIdViewModel
    {
        public int Id { get; set; }

        public required string Day { get; set; }

        public override string ToString()
        {
            return Day;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is DayReportDayIdViewModel model)
            {
                return model.Id == Id;
            }
            return false;
        }
    }
}
