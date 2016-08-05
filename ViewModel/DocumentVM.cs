using System;
using System.Windows;
using System.Windows.Input;
using a7ExtensionMethods;
using a7JsonViewer.Dialogs;
using a7JsonViewer.Utils;
using Newtonsoft.Json.Linq;

namespace a7JsonViewer.ViewModel
{
    public class DocumentVM : BaseVM
    {
        private string _stringified;
        public string Stringified
        {
            get { return _stringified; }
            set { _stringified = value; OnPropertyChanged(); }
        }

        private JToken _jToken;
        public JToken JToken
        {
            get { return _jToken; }
            set { _jToken = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand => new LambdaCommand((o) =>
        {
            IsBusy = true;
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Not possible to save changes:{Environment.NewLine}{e.ToString()}");
            }
            IsBusy = false;
        });

        public ICommand OpenFileCommand => new LambdaCommand((o) =>
        {
            IsBusy = true;
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Not possible to open file:{Environment.NewLine}{e.ToString()}");
            }
            IsBusy = false;
        });

        public ICommand CopyValueFromJPropertyCommand => new LambdaCommand((o) =>
        {
            if (o.IsNotEmpty() && o is JProperty)
            {
                var jp = o as JProperty;
                Clipboard.SetText(jp.Value.ToString());
            }
        });
        public ICommand CopyNameFromJPropertyCommand => new LambdaCommand((o) =>
        {
            if (o.IsNotEmpty() && o is JProperty)
            {
                var jp = o as JProperty;
                Clipboard.SetText(jp.Name);
            }
        });
        public ICommand EditValueFromJPropertyCommand => new LambdaCommand((o) =>
        {
            if (o.IsNotEmpty() && o is JProperty)
            {
                var jp = o as JProperty;
                var dlg = new ChangeValue(jp.Value.ToObject<object>());
                if (dlg.ShowDialog() == true)
                {
                    throw new NotImplementedException();
                }
            }
        });

        public ICommand FromClipboardCommand => new LambdaCommand((o) =>
        {
            var content = System.Windows.Clipboard.GetText(TextDataFormat.Text);
            if(!string.IsNullOrWhiteSpace(content))
                setJson(content);
        });

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { _isEditMode = value; OnPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; OnPropertyChanged(); }
        }



        public DocumentVM(string json)
        {
            _isBusy = false;
            _isEditMode = true;
            setJson(json);
        }

        private void setJson(string json)
        {
            try
            {
                JToken = JToken.Parse(json);
                Stringified = JToken.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("Not valid json format.");
            }
        }
    }
}
