using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Windows.ApplicationModel;

namespace Shagu.Utils.Extends
{
    public class ObservableCollectionExt<T> : ObservableCollection<T>
    {
        public bool IsLoaded { get; set; }

        public bool IsShowEmpty { get; set; }
        public int MaxCount { get; set; }
        public int Page { get; set; }
        public int PageCount { get; set; }

        public ObservableCollectionExt()
        {
            PageCount = 10;

            if (DesignMode.DesignModeEnabled)
            {
                HasMore = true;
            }
        }

        #region IsEmpty, HasMore, IsLoading, IsRefreshing

        private bool hasMore;
        private bool isLoading;
        private bool isRefreshing;

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsLoading"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsShowHasMore"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEmpty"));
                }
            }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsRefreshing"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsShowHasMore"));
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEmpty"));
                }
            }
        }

        public bool HasMore
        {
            get { return hasMore && Count > 0; }
            set
            {
                hasMore = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HasMore"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsShowHasMore"));
            }
        }

        public bool IsShowHasMore => Count > 0 && !isRefreshing;

        /// <summary>
        /// 标识内容是否为空，如果正在加载，刷新，则表示不为空
        /// </summary>
        public bool IsEmpty => IsShowEmpty && !IsLoading && !IsRefreshing && Count == 0;

        #endregion

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (IsShowEmpty && !IsLoading && !IsRefreshing && Count == 0)
            {
                //通知是否为空
                OnPropertyChanged(new PropertyChangedEventArgs("IsEmpty"));
            }
        }
    }
}
