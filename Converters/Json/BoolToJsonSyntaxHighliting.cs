using System;
using System.Globalization;
using System.Windows.Data;
using a7ExtensionMethods;

namespace a7JsonViewer.Converters.Json
{
    class BoolToJsonSyntaxHighliting : IValueConverter
    {
        ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition readOnlyHighLighting;
        ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition editableHighLighting;

        public BoolToJsonSyntaxHighliting()
        {
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("a7JsonViewer.Resources.avalonEditJson.xshd"))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    editableHighLighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("a7JsonViewer.Resources.avalonEditJsonReadOnly.xshd"))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    readOnlyHighLighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToBool())
                return editableHighLighting;
            else
                return readOnlyHighLighting;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
