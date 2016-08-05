using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using a7JsonViewer.ViewModel;
using Newtonsoft.Json.Linq;

namespace a7JsonViewer.Controls
{
    /// <summary>
    /// Interaction logic for JsonTree.xaml
    /// </summary>
    public partial class JsonTree : UserControl
    {
        public DocumentVM Document
        {
            get { return (DocumentVM)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(DocumentVM), typeof(JsonTree), new PropertyMetadata(null));



        public JToken JToken
        {
            get { return (JToken)GetValue(JTokenProperty); }
            set { SetValue(JTokenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JTokenProperty =
            DependencyProperty.Register("JToken", typeof(JToken), typeof(JsonTree), new PropertyMetadata(null));



        public JsonTree()
        {
            InitializeComponent();
        }

        private void ExpandAll(ItemsControl items)
        {
            foreach (object obj in items.Items)
            {
                ItemsControl childControl = items.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;
                if (childControl != null)
                {
                    ExpandAll(childControl);
                }
                TreeViewItem item = childControl as TreeViewItem;
                if (item != null)
                    item.IsExpanded = !item.IsExpanded;
            }
        }


        public void ExpandCollapseAll()
        {
            foreach (object item in this.treeView1.Items)
            {
                TreeViewItem treeItem = this.treeView1.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (treeItem != null)
                    ExpandAll(treeItem);
                // besides root level!
                // treeItem.IsExpanded = !treeItem.IsExpanded; 
            }
        }


        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if(e.Property == JTokenProperty)
            {
                if(e.NewValue is JToken)
                {
                    this.treeView1.ItemsSource = null;
                    this.treeView1.Items.Clear();
                    this.treeView1.ItemsSource = new List<JToken> {( e.NewValue as JToken)};
                }
            }
        }
    }
}
