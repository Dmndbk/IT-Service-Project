using System;
using System.Windows.Controls;

namespace ITService.UI.Views.MainViews
{
    /// <summary>
    /// Логика взаимодействия для StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {

        public StatisticsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintVisual(printGrid, "Grid Printing.");
        }
    }
}
