using System.Collections.Generic;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using Newtonsoft.Json;
using ZhihuDaily.Domain.Models;

namespace ZhihuDaily.ViewModels
{
    public class TestViewModel : Screen
    {
        public ObservableCollection<CommentInfo> Comments { get; set; }
        public TestViewModel()
        {
            Comments = new BindableCollection<CommentInfo>();
            var json =
                @"[{""own"":false,""author"":""vitamin1008"",""content"":""知乎你怎么了"",""avatar"":""http:\/\/pic3.zhimg.com\/dae35dac8f537a7ecf1f30be66a5992a_im.jpg"",""time"":1445523695,""voted"":false,""id"":21527684,""likes"":0},{""own"":false,""author"":""Helen灰灰灰灰灰"",""content"":""耳边响起琵琶曲\n我是看了电影再看书\n感同身受"",""avatar"":""http:\/\/pic4.zhimg.com\/d29b3626f580d436fbea3faaf5b4e6ab_im.jpg"",""time"":1445522925,""voted"":false,""id"":21527353,""likes"":0},{""own"":false,""author"":""舒啊舒啊"",""content"":""最喜欢的中篇"",""avatar"":""http:\/\/pic1.zhimg.com\/6de7b177e9ca47b323eb46c3dfb24c1c_im.jpg"",""time"":1445522330,""voted"":false,""id"":21527163,""likes"":0},{""own"":false,""author"":""冯一鸣"",""content"":""我爱你，与你无关，与爱有关。"",""avatar"":""http:\/\/pic4.zhimg.com\/9d356254352bbc2380870ca4cff9ea93_im.jpg"",""time"":1445522263,""voted"":false,""id"":21527153,""likes"":1},{""own"":false,""author"":""老杨219"",""content"":""知乎怎么成这个调调了？"",""avatar"":""http:\/\/pic1.zhimg.com\/7f990ff66440f2bc1652fd00f25e2898_im.jpg"",""time"":1445521803,""voted"":false,""id"":21527025,""likes"":2},{""own"":false,""author"":""巴拉巴拉"",""content"":""悲剧。。。"",""avatar"":""http:\/\/pic4.zhimg.com\/fde6d992e54b0be9ee8c0c1f98edc82b_im.jpg"",""time"":1445521478,""voted"":false,""id"":21526980,""likes"":0},{""own"":false,""author"":""Catnip-Catnip"",""content"":""台词听上去确实就是偏执狂。最近犯罪心理相关的内容看太多。"",""avatar"":""http:\/\/pic3.zhimg.com\/1c0196476fa8dcde5542c5587ba216b2_im.jpg"",""time"":1445520884,""voted"":false,""id"":21526912,""likes"":2},{""own"":false,""author"":""树上男爵"",""content"":""啊。。胸闷。。"",""avatar"":""http:\/\/pic1.zhimg.com\/b376befe29b97d7d0e7dc0cee52cfb6c_im.jpg"",""time"":1445520316,""voted"":false,""id"":21526871,""likes"":0},{""own"":false,""author"":""苏烁苏烁"",""content"":""孟京辉导演独角戏 一个陌生女人的独白"",""avatar"":""http:\/\/pic1.zhimg.com\/6e93373c4_im.jpg"",""time"":1445519765,""reply_to"":{""content"":""这是啥(●—●)"",""status"":0,""id"":21526747,""author"":""文学青年""},""voted"":false,""id"":21526806,""likes"":0},{""own"":false,""author"":""李落李落"",""content"":""不是 M 先生，是 W 先生。"",""avatar"":""http:\/\/pic4.zhimg.com\/e8218ae5f_im.jpg"",""time"":1445519597,""voted"":false,""id"":21526760,""likes"":3},{""own"":false,""author"":""文学青年"",""content"":""这是啥(●—●)"",""avatar"":""http:\/\/pic3.zhimg.com\/7327205d6_im.jpg"",""time"":1445519493,""voted"":false,""id"":21526747,""likes"":1},{""own"":false,""author"":""落花流水他姐"",""content"":""沙发"",""avatar"":""http:\/\/pic4.zhimg.com\/0529c60cf009263e34f2604cd58f39b3_im.jpg"",""time"":1445519474,""voted"":false,""id"":21526738,""likes"":0}]";

            var comments = JsonConvert.DeserializeObject<List<CommentInfo>>(json);
            foreach (var commentInfo in comments)
            {
                Comments.Add(commentInfo);
            }
        }
    }
}
