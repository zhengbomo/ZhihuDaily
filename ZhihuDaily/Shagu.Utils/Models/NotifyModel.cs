using System.ComponentModel;
using System.Runtime.CompilerServices;
using Shagu.Utils.Annotations;

namespace Shagu.Utils.Models
{
    public class NotifyModel<T>:INotifyPropertyChanged
    {
        private T _value;

        public T Value
        {
            get { return _value; }
            set
            {
                if ((_value != null && !_value.Equals(value)) || (value != null && !value.Equals(_value)))
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
