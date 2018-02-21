using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitterAccess;

namespace TwitterReader
{
    public class UserModel : NodeBaseModel
    {
        public UserModel(UserEntity user) : base(user.ProfileImageUrl)
        {
            UserId = user.Id;
            UserName = user.Name;
            ScreenName = user.ScreenName;
            ProfileImageUrl = user.ProfileImageUrl;
            TweetRetweetModelList = new List<TweetModel>();
            TweetModelListWrapper = new TweetModelListWrapper();
            RetweetModelListWrapper = new TweetModelListWrapper();
            MoveGroupUserCommand = new DelegateCommand<string>(ExecuteMoveGroupUserCommand);
        }

        public long UserId { get; }
        public string UserName { get; }
        public string ScreenName { get; }
        public bool UserNotEmpty => !string.IsNullOrWhiteSpace(UserName);
        public string ProfileImageUrl { get; }
        public GroupModel GroupModel { get; set; }
        public string CurrentGroupName => GroupModel?.GroupName;
        public string MoveToGroupMessage => App.MainViewModel.MoveToGroupMessage;
        public List<ContextMenuItem> AvailableMoveToGroupList { get; set; }
        public List<TweetModel> TweetRetweetModelList { get; set; }
        public TweetModelListWrapper TweetModelListWrapper { get; set; }
        public string TweetCountMessage => $"{TweetModelListWrapper?.TweetModelList?.Count ?? 0} recent tweets";
        public TweetModelListWrapper RetweetModelListWrapper { get; set; }
        public string RetweetCountMessage => $"{RetweetModelListWrapper?.TweetModelList?.Count ?? 0} recent retweets";
        public ICommand MoveGroupUserCommand { get; }
        public string GroupImagePath => GroupModel?.NodeImagePath;
       
        private void ExecuteMoveGroupUserCommand(string groupName)
        {
            App.MainViewModel.ExecuteMoveUserToGroup(ScreenName, groupName);
        }
    }
}