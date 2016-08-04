using System;
using System.Collections.Generic;
using System.IO;
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
using a7JsonViewer.Utils;
using a7JsonViewer.ViewModel;
using ICSharpCode.AvalonEdit.Folding;

namespace a7JsonViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FoldingManager foldingManager;
        BraceFoldingStrategy foldingStrategy;

        public MainWindow()
        {
            this.Loaded += OnLoaded;
            this.DragEnter += (sender, args) => args.Effects = DragDropEffects.Move;
            this.Drop += OnDrop;
            InitializeComponent();
            this.DataContext = new DocumentVM(
                @"{ 'message' : 'try to drop a json file!'}");
            //MessageBox.Show(Environment.GetCommandLineArgs().Length.ToString());
            //MessageBox.Show(string.Join(",", Environment.GetCommandLineArgs()));
        }

        private void OnDrop(object sender, DragEventArgs dragEventArgs)
        {
            if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop);
                if (files.Any() && File.Exists(files[0]))
                {
                    var content = File.ReadAllText(files[0]);
                    this.DataContext = new DocumentVM(content);
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.jsonEditor.TextChanged += Te_TextChanged;
            foldingManager = FoldingManager.Install(this.jsonEditor.TextArea);
            foldingStrategy = new BraceFoldingStrategy();
            foldingStrategy.UpdateFoldings(foldingManager, jsonEditor.Document);
        }

        private void Te_TextChanged(object sender, EventArgs e)
        {
            foldingStrategy.UpdateFoldings(foldingManager, jsonEditor.Document);
        }
    }
}
