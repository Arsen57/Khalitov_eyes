using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Khalitov_глазки
{
    /// <summary>
    /// Логика взаимодействия для ProdPage.xaml
    /// </summary>
    public partial class ProdPage : Page
    {
        private Agent currentAgent = new Agent();
        public ProdPage(Agent SelectedAgent)
        {
            InitializeComponent();

            currentAgent = SelectedAgent;

            var currentProduct = Khalitov_глазкиEntities.GetContext().ProductSale.ToList();
            ProductHistoryListView.ItemsSource = currentProduct.Where(p => p.AgentID == currentAgent.ID);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var _currentProduct = (sender as Button).DataContext as ProductSale;

            var _currentProductSale = Khalitov_глазкиEntities.GetContext().ProductSale.ToList();
            _currentProductSale = _currentProductSale.Where(p => p.ProductID == _currentProduct.ID).ToList();

            try
            {
                Khalitov_глазкиEntities.GetContext().ProductSale.Remove(_currentProduct);
                Khalitov_глазкиEntities.GetContext().SaveChanges();
                UpdateProduct();
                //Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddProdSale addProdSale = new AddProdSale(currentAgent);
            addProdSale.ShowDialog();
            UpdateProduct();
        }

        private void UpdateProduct()
        {
            var currentProduct = Khalitov_глазкиEntities.GetContext().ProductSale.ToList();
            ProductHistoryListView.ItemsSource = currentProduct.Where(p => p.AgentID == currentAgent.ID);
        }
    }
}
