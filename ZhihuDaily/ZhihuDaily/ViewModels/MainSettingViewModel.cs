using System;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using ZhihuDaily.Util;
using Shagu.UI.Utils;
using Shagu.Utils;
using Shagu.Utils.Models;
using Shagu.Utils.Utils;

namespace ZhihuDaily.ViewModels
{
    public class MainSettingViewModel : Screen
    {
        private readonly IFrameManager _frameManager;
        private readonly GlobalInfoManager _globalInfoManager;
        public NotifyModel<bool> IsLoading { get; set; }

        private string _cacheSize;

        public string CacheSize
        {
            get { return _cacheSize; }
            set
            {
                _cacheSize = value;
                NotifyOfPropertyChange();
            }
        }

        private string _imageCacheSize;

        public string ImageCacheSize
        {
            get { return _imageCacheSize; }
            set
            {
                _imageCacheSize = value;
                NotifyOfPropertyChange();
            }
        }

        public string Text { get; set; }

        public MainSettingViewModel()
        {
            IsLoading = new NotifyModel<bool>();
            Text = @"关于中秋节的来历，大致有三种：起源于古代对月的崇拜、月下歌舞觅偶的习俗，古代秋报拜土地神的遗俗。 ""中秋""一词，最早见于《周礼》。根据我国古代历法，一年有四季，每季三个月，分别被称为孟月、仲月、季月三部分，因此秋季的第二月叫仲秋，又因农历八月十五日，在八月中旬，故称""中秋""。到唐朝初年，中秋节才成为固定的节日。《新唐书·卷十五 志第五·礼乐五》载""其中春、中秋释奠于文宣王、武成王""，及""开元十九年，始置太公尚父庙，以留侯张良配。中春、中秋上戊祭之，牲、乐之制如文""。据史籍记载，古代帝王祭月的节期为农历八月十五，时日恰逢三秋之半，故名""中秋节""; 又因为这个节日在秋季八月，故又称""秋节""、""八月节""、""八月会""、""中秋节"";又有祈求团圆的信仰和相关习俗活动，故亦称""团圆节""、""女儿节""。因中秋节的主要活动都是围绕""月""进行的，所以又俗称""月节""、""月夕""、""追月节""、""玩月节""、""拜月节""; 在唐朝，中秋节还被称为""端正月""。中秋节的盛行始于宋朝，至明清时，已与元旦齐名，成为我国的主要节日之一。";
        }

        public MainSettingViewModel(IFrameManager frameManager, GlobalInfoManager globalInfoManager) : this()
        {
            _frameManager = frameManager;
            _globalInfoManager = globalInfoManager;
            
            CacheSize = "正在计算...";
            ImageCacheSize = "正在计算...";
        }



        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            var easClientDeviceInformation = new EasClientDeviceInformation();
            var os = easClientDeviceInformation.OperatingSystem;

            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("cachedata", CreationCollisionOption.OpenIfExists);
            CacheSize = "正在计算...";
            var size = await FileManager.GetFolderSizeAsync(folder);
            CacheSize = FileManager.GetSizeString(size);


            folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("cacheImage", CreationCollisionOption.OpenIfExists);
            ImageCacheSize = "正在计算...";
            size = await FileManager.GetFolderSizeAsync(folder);
            ImageCacheSize = FileManager.GetSizeString(size);

        }

        public async void ClearCache()
        {
            if (CacheSize.Equals("正在计算..."))
            {
                ToastHelper.ToastInfo("正在计算，请稍后再试", _globalInfoManager.IsNightMode);
                return;
            }

            var dialogResult = await MessageBox.ShowAsync("清理缓存之后，您将无法在离线模式下查看，确定要清理吗", "提示");
            if (dialogResult == ContentDialogResult.Primary)
            {
                IsLoading.Value = true;

                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("cachedata", CreationCollisionOption.OpenIfExists);
                await FileManager.LockDeleteFolderAsync(folder);
                ToastHelper.ToastInfo("清理完成", _globalInfoManager.IsNightMode);

                CacheSize = "正在计算...";
                var size = await FileManager.GetFolderSizeAsync(folder);
                CacheSize = FileManager.GetSizeString(size);

                IsLoading.Value = false;

            }
        }

        public async void ClearImageCache()
        {
            if (ImageCacheSize.Equals("正在计算..."))
            {
                ToastHelper.ToastInfo("正在计算，请稍后再试", _globalInfoManager.IsNightMode);
                return;
            }

            var dialogResult = await MessageBox.ShowAsync("清理缓存之后，您将无法在离线模式下查看图片，确定要清理吗", "提示");
            if (dialogResult == ContentDialogResult.Primary)
            {
                IsLoading.Value = true;

                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("cacheImage", CreationCollisionOption.OpenIfExists);
                await FileManager.LockDeleteFolderAsync(folder);
                ToastHelper.ToastInfo("清理完成", _globalInfoManager.IsNightMode);

                ImageCacheSize = "正在计算...";
                var size = await FileManager.GetFolderSizeAsync(folder);
                ImageCacheSize = FileManager.GetSizeString(size);

                IsLoading.Value = false;
            }
        }
    }
}
