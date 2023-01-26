using System.Windows;

namespace WpfEssentials.Exceptions
{
    public partial class ExceptionPrompt : Window
    {
        public ExceptionPrompt(System.Exception e)
        {
            InitializeComponent();

            var sb = new System.Text.StringBuilder();

            do
            {
                sb.AppendLine(e.ToString());
            } while ((e = e.InnerException) != null);

            ErrorTextBox.Text = sb.ToString();

            foreach (var wnd in Application.Current.Windows)
            {
                if (ReferenceEquals(wnd, this))
                    continue;

                try
                {
                    ((Window)wnd).Close();
                }
                catch (System.Exception) { }
            }
        }

        private void OnShutdownClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
