using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using a7ExtensionMethods;
using a7JsonViewer.Dialogs;
using a7JsonViewer.Utils;
using Microsoft.Win32;
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
                var dlg = new SaveFileDialog();
                if (!string.IsNullOrWhiteSpace(_sourceFilePath))
                    dlg.FileName = _sourceFilePath;
                var result = dlg.ShowDialog();
                if (result == true)
                {
                    File.WriteAllText(dlg.FileName, this.Stringified);
                }
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
                var dlg = new OpenFileDialog();
                var result = dlg.ShowDialog();
                if (result == true)
                {
                    OpenFileContent(dlg.FileName);
                }
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
                    var js = this.JToken;
                    var token = js.SelectToken(jp.Path);
                    if (token != null)
                    {
                        token.Replace(JToken.FromObject(dlg.Value));
                    }
                    else
                    {
                        throw new Exception($"'{jp.Path}' not found...");
                    }

       //             jp.Value = new JValue(dlg.Value);
                    Stringified = JToken.ToString();
                    JToken = JToken.Parse(Stringified);
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

        private string _sourceFilePath;


        public DocumentVM(string json)
        {
            _isBusy = false;
            _isEditMode = true;
            setJson(json);
        }

        public void OpenFileContent(string filePath)
        {
            if(!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                _sourceFilePath = filePath;
                var content = File.ReadAllText(filePath);
                setJson(content, false);
            }
        }

        private void setJson(string json, bool clearSourceFilePath = true)
        {
            if (clearSourceFilePath)
                _sourceFilePath = null;
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
