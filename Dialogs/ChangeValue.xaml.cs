using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace a7JsonViewer.Dialogs
{
    /// <summary>
    /// Interaction logic for ChangeValue.xaml
    /// </summary>
    public partial class ChangeValue : Window
    {
        private Type _type;
        public object Value { get; private set; }

        public ChangeValue(object value)
        {
            InitializeComponent();
            this._type = value.GetType();
            this.tbValue.Text = value.ToString();
            if (value is string && (value as string).Contains("\n"))
                setMultiLine();
        }

        private void bCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void bOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            var converter = TypeDescriptor.GetConverter(_type);
            try
            {
                this.Value = converter.ConvertFromInvariantString(this.tbValue.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Couldn't convert value - {Environment.NewLine}{ex.Message}");
            }
            
        }

        private void setMultiLine()
        {
            this.Height = 250;
            tbValue.Height = 135;
            tbValue.AcceptsReturn = true;
            tbValue.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            tbValue.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
    }
}
