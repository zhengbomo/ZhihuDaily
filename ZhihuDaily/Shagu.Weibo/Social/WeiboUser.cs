/* ==============================================================================
 * 功能描述：WeiboUser  
 * 创 建 者：贤凯
 * 创建日期：2/8/2015 3:28:21 PM
 * ==============================================================================*/

using Newtonsoft.Json;

namespace Shagu.Weibo.Social
{
    public class WeiboUser
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idstr")]
        public string Idstr { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("profile_url")]
        public string ProfileUrl { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("weihao")]
        public string Weihao { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        [JsonProperty("friends_count")]
        public int FriendsCount { get; set; }

        [JsonProperty("statuses_count")]
        public int StatusesCount { get; set; }

        [JsonProperty("favourites_count")]
        public int FavouritesCount { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("allow_all_act_msg")]
        public bool AllowAllActMsg { get; set; }

        [JsonProperty("geo_enabled")]
        public bool GeoEnabled { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("verified_type")]
        public int VerifiedType { get; set; }

        [JsonProperty("allow_all_comment")]
        public bool AllowAllComment { get; set; }

        [JsonProperty("avatar_large")]
        public string AvatarLarge { get; set; }

        [JsonProperty("verified_reason")]
        public string VerifiedReason { get; set; }

        [JsonProperty("follow_me")]
        public bool FollowMe { get; set; }

        [JsonProperty("online_status")]
        public int OnlineStatus { get; set; }

        [JsonProperty("bi_followers_count")]
        public int BiFollowersCount { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("ulevel")]
        public int Ulevel { get; set; }

        [JsonProperty("badge")]
        public object Badge { get; set; }
    }
}
