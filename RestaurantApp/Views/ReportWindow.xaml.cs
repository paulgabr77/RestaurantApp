using RestaurantApp.ViewModels;
using System.Windows;

namespace RestaurantApp.Views
{
    public partial class ReportWindow : Window
    {
        public ReportWindow(ReportViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
} 