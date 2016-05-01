using Windows.UI.Xaml.Controls;

namespace Shagu.Utils.Models
{
    public class HeaderInfo<T1, T2>:HeaderInfo
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
    }

    public class HeaderInfo<T> : HeaderInfo
    {
        public T SubValue { get; set; }
    }

    public class HeaderInfo
    {
        public string Title { get; set; }
        public Symbol SymbolIcon { get; set; }
    }
}
