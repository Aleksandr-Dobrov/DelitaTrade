namespace DelitaTrade.WpfViewModels
{
    public class WpfDayReportIdViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is WpfDayReportIdViewModel model)
            {
                return model.Id == Id;
            }
            return false;
        }
    }
}
