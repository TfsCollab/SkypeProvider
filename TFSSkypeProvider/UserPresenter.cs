using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    public class UserPresenter
    {
        private readonly User _user;

        public UserPresenter(User user)
        {
            _user = user;
        }

        public User User
        {
            get { return _user; }
        }

        public string DisplayName
        {
            get
            {
                if (_user == null)
                    return "No Skype User attached";

                return string.Format("{0} ({1})",
                                     string.IsNullOrEmpty(_user.FullName) ? "<no full name>" : _user.FullName,
                                     _user.Handle);
            }
        }
    }
}