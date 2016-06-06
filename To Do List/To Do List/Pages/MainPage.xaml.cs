using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using To_Do_List.Classes;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace To_Do_List
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    ///
    public sealed partial class MainPage : Page
    {

       
        ObservableCollection<ToDo> defaultViewModel = new ObservableCollection<ToDo>();
        TextBlock t = new TextBlock();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (TDList.SelectionMode == ListViewSelectionMode.Multiple)
                TDList.SelectionMode = ListViewSelectionMode.Single;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            DBHelper db = new DBHelper();
            defaultViewModel = db.ReadItems();

            
            defaultViewModel = new ObservableCollection<ToDo>(defaultViewModel.OrderByDescending(i => i.priority));
            TDList.ItemsSource = defaultViewModel;

            if(defaultViewModel.Count == 0)
            {
                TDList.Visibility = Visibility.Collapsed;
                t.Text = "nothing to do. get a life.";
                t.FontSize = 20;
                Grid.SetRow(t, 2);
                LayoutRoot.Children.Add(t);
            }
            else
            {
                TDList.Visibility = Visibility.Visible;
                t.Visibility = Visibility.Collapsed;
            }
        }

      

        private void Add_Page(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Add));
        }

        private void Change_Selection_Mode(object sender, HoldingRoutedEventArgs e)
        {
            TDList.SelectionMode = ListViewSelectionMode.Multiple;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Enable(object sender, SelectionChangedEventArgs e)
        {
            if(TDList.SelectionMode == ListViewSelectionMode.Multiple && TDList.SelectedItems.Count !=0)
            {
                Del_Btn.IsEnabled = true;
            }
            else if(TDList.SelectionMode == ListViewSelectionMode.Multiple && TDList.SelectedItems.Count == 0)
            {
                Del_Btn.IsEnabled = false;
            }

            if(TDList.SelectionMode == ListViewSelectionMode.Single)
            {
                try
                {
                    DBHelper db = new DBHelper();
                    var item = TDList.SelectedItem as ToDo;
                    item.done = true;
                    db.Update(item);
                    defaultViewModel = db.ReadItems();
                    defaultViewModel = new ObservableCollection<ToDo>(defaultViewModel.OrderByDescending(i => i.priority));
                    TDList.ItemsSource = defaultViewModel;

                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                
            }
        }

        private void Delete_Item(object sender, RoutedEventArgs e)
        {
            DBHelper db = new DBHelper();
            foreach(ToDo t in TDList.SelectedItems)
            {
                db.Delete(t.Id);
            }
            TDList.SelectionMode = ListViewSelectionMode.Single;
            defaultViewModel = db.ReadItems();

            defaultViewModel = new ObservableCollection<ToDo>(defaultViewModel.OrderByDescending(i => i.priority));
            TDList.ItemsSource = defaultViewModel;

            if (defaultViewModel.Count == 0)
            {
                TDList.Visibility = Visibility.Collapsed;
                
                t.Text = "nothing to do. get a life.";
                t.FontSize = 20;
                Grid.SetRow(t, 2);
                LayoutRoot.Children.Add(t);
            }
            else
            {
                TDList.Visibility = Visibility.Visible;
                t.Visibility = Visibility.Collapsed;
            }
        }
    }
}
