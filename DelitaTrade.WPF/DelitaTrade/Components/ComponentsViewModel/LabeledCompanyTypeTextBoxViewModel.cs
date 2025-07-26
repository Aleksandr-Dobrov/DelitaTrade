using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledCompanyTypeTextBoxViewModel : LabeledStringTextBoxViewModel
    {
        private string _companyType = string.Empty;
        [MinLength(0)]
        public override string TextBox
        { 
            get => _companyType;
            set
            {
                _companyType = value;
                OnPropertyChange();
            }
        }
    }
}
