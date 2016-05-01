using Caliburn.Micro;
using Newtonsoft.Json;

namespace ZhihuDaily.Domain.Models
{
    public class CounterList : PropertyChangedBase
    {
        [JsonProperty("vote_status")]
        public int VoteStatus { get; set; }

        [JsonIgnore]
        public bool IsVoted
        {
            get { return VoteStatus > 0; }
            set
            {
                var isVoted = value ? 1 : 0;
                if (VoteStatus != isVoted)
                {
                    VoteStatus = isVoted;
                    NotifyOfPropertyChange(nameof(IsVoted));
                }
            }
        }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        private bool _favorite;

        [JsonProperty("favorite")]
        public bool Favorite
        {
            get { return _favorite; }
            set
            {
                if (_favorite != value)
                {
                    _favorite = value;
                    NotifyOfPropertyChange(nameof(Favorite));
                }
            }
        }

        [JsonProperty("long_comments")]
        public int LongComments { get; set; }

        [JsonProperty("comments")]
        public int Comments { get; set; }


        [JsonProperty("short_comments")]
        public int ShortComments { get; set; }
    }
}