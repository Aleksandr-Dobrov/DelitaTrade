using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsView
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging = false;

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new PropertyMetadata(string.Empty, PasswordPropertyChange));

        private static void PasswordPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty); 
            set => SetValue(PasswordProperty, value);
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
        }
        private void PasswordBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Focus();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }

        private void UpdatePassword()
        {
            if (_isPasswordChanging == false)
            {
                passwordBox.Password = Password;
            }
        }
    }
}
