using System;
using System.Threading;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using Shagu.UI.Utils;
using Shagu.Utils.Controls;
using Shagu.Utils.Utils;
using ZhihuDaily.Controls;
using ZhihuDaily.Domain.Service;
using ZhihuDaily.Util;

namespace ZhihuDaily.ViewModels
{
    public class UserInfoViewModel : Screen
    {
        private readonly GlobalInfoManager _globalInfoManager;
        private readonly CancellationToken _cancelToken;
        private readonly IFrameManager _frameManager;
        
        public string UserName { get; set; }
        public string Icon => _globalInfoManager.LoginResult.Avatar;
        public UserInfoViewModel(GlobalInfoManager globalInfoManager, IFrameManager frameManager)
        {
            _frameManager = frameManager;
            _globalInfoManager = globalInfoManager;
            _cancelToken = new CancellationToken();

            UserName = _globalInfoManager.LoginResult.Name;
        }

        public async void SaveName()
        {
            //保存图片


            //保存昵称
            if (!_globalInfoManager.LoginResult.Name.Equals("未登录") ||
                !_globalInfoManager.LoginResult.Name.Equals(UserName))
            {
                //不同
                var loadingView = LoadingView.ShowLoading("正在保存");

                var operationResult = await
                    DataService.SetName(_globalInfoManager.AccessToken, UserName, _cancelToken);

                RequestUtil.DoResult(operationResult, operation =>
                {
                    _globalInfoManager.UserName = UserName;
                    ToastHelper.ToastInfo("保存成功", _globalInfoManager.IsNightMode);
                });

                loadingView.Hide();
            }
            else
            {
                ToastHelper.ToastInfo("资料未修改", _globalInfoManager.IsNightMode);
            }
        }

        public async void ChangeAvatar()
        {
            var imagePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".png", ".bmp" }
            };
            var file = await imagePicker.PickSingleFileAsync();
            if (file != null)
            {
                var newImage = await CropImageView.ShowDialog(file);

                if (newImage != null)
                {
                    //不同
                    var loadingView = LoadingView.ShowLoading("正在上传头像");

                    using (var randomAccessStream = new InMemoryRandomAccessStream())
                    {
                        await newImage.ToStreamAsJpeg(randomAccessStream);
                        randomAccessStream.Seek(0);

                        var avatarResult = await
                      DataService.SetAvatar(_globalInfoManager.AccessToken, randomAccessStream, _cancelToken);

                        RequestUtil.DoResult(avatarResult, avatar =>
                        {
                            _globalInfoManager.Avatar = avatar.Avatar;
                            NotifyOfPropertyChange(nameof(Icon));
                            ToastHelper.ToastInfo("保存成功", _globalInfoManager.IsNightMode);
                        });

                    }




                    loadingView.Hide();
                }

            }
        }



        public async void Logout()
        {
            if (ContentDialogResult.Primary == await MessageBox.ShowAsync("确定要退出登录吗？", "提示"))
            {
                _globalInfoManager.LoginResult = null;
                ToastHelper.ToastInfo("退出成功", _globalInfoManager.IsNightMode);
                _frameManager.CenterFrame.GoBack();
            }
        }

    }
}
