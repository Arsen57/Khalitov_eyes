﻿using System;
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
    /// Логика взаимодействия для ServiceEyesPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        int CounteRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPagelist = new List<Agent>();
        List<Agent> TableList;

        List<int> SelectPriority = new List<int>();
        public AgentPage()
        {
            InitializeComponent();
            //добавить строки
            //загрузить в список из БД
            var currentAgents = Khalitov_глазкиEntities.GetContext().Agent.ToList();
            //связаться с листвью
            AgentListView.ItemsSource = currentAgents;
            //добавить строки

            ComboTypeAgTy.SelectedIndex = 0;
            ComboTypeSort.SelectedIndex = 0;

            UpdateAgent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboTypeSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboTypeAgTy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void UpdateAgent()
        {
            var currentAgent = Khalitov_глазкиEntities.GetContext().Agent.ToList();


            if (ComboTypeAgTy.SelectedIndex == 0)
            {
                currentAgent = currentAgent.ToList();
            }
            if (ComboTypeAgTy.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 3)).ToList();
            }
            if (ComboTypeAgTy.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 5)).ToList();
            }
            if (ComboTypeAgTy.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 1)).ToList();
            }
            if (ComboTypeAgTy.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 2)).ToList();
            }
            if (ComboTypeAgTy.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 4)).ToList();
            }
            if (ComboTypeAgTy.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => (p.AgentTypeID == 6)).ToList();
            }

            currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Contains(TBoxSearch.Text)
            || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (ComboTypeSort.SelectedIndex == 0)
            {
                currentAgent = currentAgent.ToList();
            }
            if (ComboTypeSort.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 3)
            {
                currentAgent = currentAgent.OrderBy(p => p.SalePercent).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 4)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.SalePercent).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 5)
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            if (ComboTypeSort.SelectedIndex == 6)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }

            AgentListView.ItemsSource = currentAgent;

            TableList = currentAgent;

            ChangePage(0, 0);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPagelist.Clear();
            CounteRecords = TableList.Count;

            if (CounteRecords % 10 > 0)
            {
                CountPage = CounteRecords / 10 + 1;
            }
            else
            {
                CountPage = CounteRecords / 10;
            }

            Boolean Ifupdate = true;

            int min;

            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CounteRecords ? CurrentPage * 10 + 10 : CounteRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPagelist.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CounteRecords ? CurrentPage * 10 + 10 : CounteRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPagelist.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CounteRecords ? CurrentPage * 10 + 10 : CounteRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPagelist.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }

            if (Ifupdate)
            {
                PageListBox.Items.Clear();

                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                //вывод количества записей на странице и общего количества
                min = CurrentPage * 10 + 10 < CounteRecords ? CurrentPage * 10 + 10 : CounteRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CounteRecords.ToString();


                AgentListView.ItemsSource = CurrentPagelist;
                AgentListView.Items.Refresh();
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Khalitov_глазкиEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = Khalitov_глазкиEntities.GetContext().Agent.ToList();
            }
            UpdateAgent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
            UpdateAgent();
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 1)
            {
                PriorityButton.Visibility = Visibility.Visible;
            }
            else
            {
                PriorityButton.Visibility = Visibility.Hidden;
            }
        }

        private void PriorityButton_Click(object sender, RoutedEventArgs e)
        {
            int max_priority = 0;
            foreach (Agent agentPriority in AgentListView.SelectedItems)
            {
                if (max_priority < agentPriority.Priority)
                    max_priority = agentPriority.Priority;
            }

            PriorityEdit priorityEdit = new PriorityEdit(max_priority);
            priorityEdit.ShowDialog();

            if (string.IsNullOrEmpty(priorityEdit.PriorityBox.Text))
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(priorityEdit.PriorityBox.Text) && Convert.ToInt32(priorityEdit.PriorityBox.Text) > 0 && priorityEdit.CheckClick == true)
            {

                foreach (Agent agentPriority in AgentListView.SelectedItems)
                {
                    agentPriority.Priority = Convert.ToInt32(priorityEdit.PriorityBox.Text);
                }
                try
                {
                    Khalitov_глазкиEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            UpdateAgent();
        }

        private void ProdViewButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProdPage((sender as Button).DataContext as Agent));
            UpdateAgent();
        }

        //private void PriorityButton_Click_1(object sender, RoutedEventArgs e)
        //{
        //}

        //private void PriorityButton_Click_2(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
