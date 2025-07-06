namespace DelitaTrade.Core.ViewModels
{
    public class DescriptionCategoryViewModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
