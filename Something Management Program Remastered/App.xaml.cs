using Something_Management_Program_Remastered.View;
using Something_Management_Program_Remastered.ViewModel;
using System.Windows;

namespace Something_Management_Program_Remastered
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainPage window = new MainPage();
            ObjectiveValueViewModel VM = new ObjectiveValueViewModel();
            window.DataContext = VM;
            window.Show();
            window.Closing += VM.OnWindowClosing;
        }
    }
}
