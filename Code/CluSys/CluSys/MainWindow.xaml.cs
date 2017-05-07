using System;
using System.Collections;
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

namespace CluSys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Search_OnKeyDown(Object sender, KeyEventArgs e)
        {
            ListBox lb;
            var treeVisitor = sender;

            while(true)
            {

                try
                {
                    var children = treeVisitor.GetType().GetProperty("Children").GetValue(treeVisitor);
                    foreach(var item in (IEnumerable)children)
                        if((string)item.GetType().GetProperty("Name").GetValue(item) == "SearchableAthletesList")
                        {
                            lb = (ListBox)item;
                            goto Done;
                        }
                }
                catch (NullReferenceException) { }

                try
                {
                    if((treeVisitor = treeVisitor.GetType().GetProperty("Parent").GetValue(treeVisitor)) == null)
                        return;
                }
                catch (NullReferenceException)
                {
                    return;
                }
            }
            Done:
            List<String> itemList = new List<string>();
            foreach(var item in lb.Items)
            {
                itemList.Add(((TextBlock)item).Text);
            }
            if (itemList.Count > 0)
            {
                //clear the items from the list
                lb.Items.Clear();

                //filter the items and add them to the list
                foreach(var item in itemList)
                {
                    if (item.Contains(((TextBox)sender).Text))
                        lb.Items.Add(item);
                }
            }
        }
    }
}
