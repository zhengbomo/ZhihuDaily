# 知乎日报UWP第三方客户端

有些时间没有维护UWP应用了，一直忙于iOS和别的事，所以把以前写的知乎日报开源出来，有兴趣搞UWP的朋友，代码写得一般，欢迎交流，有空把大概架构思路整理整理写一下

## 用到的第三方框架
  1. Caliburn.Micro：第三方MVVM框架，有点重
  2. AnimationManager：XAML动画库，支持很多动画效果，让动画更简单方便
  3. HtmlAgilityPack：Html解析工具，用于解析知乎日报文章内容
  4. Win2D：图形图像处理工具，用于闪屏图片模糊效果
  
## Feature
* 支持闪屏
* 支持日夜间模式切换
* 支持收藏

## 类库
业务无关库/通用组件  
1. Shagu.Util：基础组件库，包含一些基础工具  
2. Shagu.Controls：控件库  
3. Shagu.UI：辅助UI类，扩展MVVM，而与业务无关，本项目存在主要是因为之前有很多个项目需要维护，所以把一些项目公共的又与业务无关的类抽离到本项目中  
4. Shagu.Weibo：微博相关类  

业务相关库
1. ZhihuDaily.Domain：数据业务层：接口请求与数据持久化
2. ZhihuDaily：主项目：UI与交互

## 相关说明
1. 使用MobileFrameManager和DesktopFrameManager分别适配Desktop版和Mobile版的页面导航
2. RequestUtil：很多页面可能都需要对列表数据进行操作，列表操作无外乎就是刷新，加载更多，缓存三个操作，该类封装了这些操作
3. NavigationManager：用于重用某些通用方法，还有一些零散的方法
4. DeviceAdaptiveTrigger：结合MainShellView进行Desktop版页面适配
5. 。。。
