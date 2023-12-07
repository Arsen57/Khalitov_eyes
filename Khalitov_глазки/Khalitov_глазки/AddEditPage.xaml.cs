using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        //новое поле, которое хранит экземпляр сервиса
        private Agent currentAgent = new Agent();
        bool CheckAgent = false;
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
            {
                currentAgent = SelectedAgent;
                CheckAgent = true;
            }
            DataContext = currentAgent;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ComboType.SelectedIndex == 1)
            {
                ComboTypeId.SelectedIndex = 1;
            }
            if (ComboType.SelectedIndex == 2)
            {
                ComboTypeId.SelectedIndex = 2;
            }
            if (ComboType.SelectedIndex == 3)
            {
                ComboTypeId.SelectedIndex = 3;
            }
            if (ComboType.SelectedIndex == 4)
            {
                ComboTypeId.SelectedIndex = 4;
            }
            if (ComboType.SelectedIndex == 5)
            {
                ComboTypeId.SelectedIndex = 5;
            }
            if (ComboType.SelectedIndex == 6)
            {
                ComboTypeId.SelectedIndex = 6;
            }
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "")/*.Replace(")", "").Replace("", "")*/;
                if (((ph[1]=='9' || ph[1]=='4' || ph[1]=='8') && ph.Length!=11)
                    || (ph[1]=='3' && ph.Length!=12) )
                    errors.AppendLine("Укажите правильно телефон агента");
            }
            if (string.IsNullOrWhiteSpace(currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var allAgent = Khalitov_глазкиEntities.GetContext().Agent.ToList();
            allAgent = allAgent.Where(p => p.Title == currentAgent.Title).ToList();
            if (allAgent.Count == 0 || (CheckAgent == true && allAgent.Count <= 1))
            {
                //добавить в контекст текущие значения новой услуги
                if (currentAgent.ID == 0)
                    Khalitov_глазкиEntities.GetContext().Agent.Add(currentAgent);
                //сохранить изменения, если никаких ошибок не выявлено
                try
                {
                    Khalitov_глазкиEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Такой агент уже существует");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;

            var currentProductSale = Khalitov_глазкиEntities.GetContext().ProductSale.ToList();
            currentProductSale = currentProductSale.Where(p => p.AgentID == currentAgent.ID).ToList();

            if (currentProductSale.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление агента");
            }
            else
            {
                if (MessageBox.Show("Вы хотите удалить агента?","Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Khalitov_глазкиEntities.GetContext().Agent.Remove(currentAgent);
                        Khalitov_глазкиEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
            
        }
    }
}
