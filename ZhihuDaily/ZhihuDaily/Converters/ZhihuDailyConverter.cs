using System;
using Windows.ApplicationModel.Store;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Caliburn.Micro;
using ZhihuDaily.Domain.Core;
using ZhihuDaily.Domain.Models;
using ZhihuDaily.Util;

namespace ZhihuDaily.Converters
{
    public class ZhihuDailyConverter : IValueConverter
    {
        private readonly SolidColorBrush _favoriteBrush;
        private readonly SolidColorBrush _unFavoriteBrush;
        public ZhihuDailyConverter()
        {
            _favoriteBrush = new SolidColorBrush(Colors.Yellow);
            _unFavoriteBrush = new SolidColorBrush(Colors.White);
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var param = (string) parameter;
            if (param != null)
            {
                switch (param)
                {
                    case "favorite_text":
                        var favorite = (bool) value;
                        return favorite ? "取消收藏" : "收藏";

                    case "like_text":
                        var like = (bool) value;
                        return like ? "取消赞" : "赞";

                    case "favorite_icon_brush":
                        favorite = (bool) value;
                        return favorite ? _favoriteBrush : _unFavoriteBrush;

                    case "nightMode_text":
                        var nightMode = (bool) value;
                        return nightMode ? "日间模式" : "夜间模式";

                    case "comment_count_text":
                        return string.Format("{0} 评论", value);


                    case "comment_reply_visible":
                        var commentInfo = (CommentInfo) value;
                        return commentInfo.Own ? Visibility.Collapsed : Visibility.Visible;

                    case "comment_delete_visible":
                        commentInfo = (CommentInfo) value;
                        return commentInfo.Own ? Visibility.Visible : Visibility.Collapsed;
                    case "comment_report_visible":
                        commentInfo = (CommentInfo) value;
                        return commentInfo.Own ? Visibility.Collapsed : Visibility.Visible;

                    case "is_ad_visible":
                        //是否已付费去广告
                        return IoC.Get<GlobalInfoManager>().HasPayForAd ? Visibility.Collapsed : Visibility.Visible;
                }
            }



            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}