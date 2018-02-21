using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterReader
{
    public class GroupModel : NodeBaseModel
    {
        public GroupModel(string groupName, string nodeImagePath)
                    : base(nodeImagePath)
        {
            GroupName = groupName;
            UserModelList = new List<UserModel>();
            GroupScreenNamelList = new List<string>();
        }

        public string GroupName { get; }
        public List<UserModel> UserModelList { get; }
        /// <summary>
        /// From group setting json file.
        /// </summary>
        public List<string> GroupScreenNamelList { get; set; }
    }
}
