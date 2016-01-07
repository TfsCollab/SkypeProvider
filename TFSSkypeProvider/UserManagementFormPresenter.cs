using System.Collections.Generic;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// Presenter class for the Add User Dialog
    /// </summary>
    public class UserManagementFormPresenter
    {
        private readonly IList<User> _skypeUsers;
        private readonly IList<string> _tfsUsers;


        public UserManagementFormPresenter(IList<User> skypeUsers, IList<string> tfsUsers)
        {
            _skypeUsers = skypeUsers;
            _tfsUsers = tfsUsers;
        }

        public IList<User> SkypeUsersInternal
        {
            get { return _skypeUsers; }
        }

        public IList<UserPresenter> SkypeUsers
        {
            get
            {
                var users = new List<UserPresenter>();
                foreach (User user in SkypeUsersInternal)
                {
                    users.Add(new UserPresenter(user));
                }
                return users;
            }
        }

        public IList<string> TfsUsers
        {
            get { return _tfsUsers; }
        }

        public string SelecetdTfsUser { get; set; }
        public string SelecetdSkypeUser { get; set; }
        public UserMappingCollection Mappings { get; set; }
    }
}