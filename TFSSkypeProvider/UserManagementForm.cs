using System;
using System.Windows.Forms;

namespace TfsCommunity.Collaboration.Skype
{
    /// <summary>
    /// The UI for managing skype user assignments
    /// </summary>
    public partial class UserManagementForm : Form
    {
        private readonly UserManagementFormPresenter _presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementForm"/> class.
        /// </summary>
        public UserManagementForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementForm"/> class.
        /// </summary>
        /// <param name="presenter">The presenter.</param>
        public UserManagementForm(UserManagementFormPresenter presenter) : this()
        {
            _presenter = presenter;
            Text = string.Format(Properties.Resources.WindowTitle, presenter.SelecetdTfsUser);
            tfsUserName.Text = presenter.SelecetdTfsUser;
            userPresenterBindingSource.DataSource = presenter.SkypeUsers;
        }
        

        private void skypeUsersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnsureButtons();
        }

        private void EnsureButtons()
        {
            assignUserButton.Enabled = (skypeUsersListBox.SelectedItem != null);
        }

        private void ignoreUserButton_Click(object sender, EventArgs e)
        {
            var tfsUser = _presenter.SelecetdTfsUser;
            var skypeUser = skypeUsersListBox.SelectedItem as UserPresenter;
            if ((!string.IsNullOrEmpty(tfsUser)) && (skypeUser != null))
            {
                if (_presenter.Mappings.Contains(tfsUser))
                {
                    _presenter.Mappings[tfsUser].SkypeName = string.Empty;
                    _presenter.Mappings[tfsUser].IsIgnored = true;
                }
                else
                {
                    _presenter.Mappings.Add(new UserMapping
                                                {TfsName = tfsUser, SkypeName = skypeUser.User.Handle, IsIgnored = true});
                }
            }
            this.Close();
        }

        private void assignUserButton_Click(object sender, EventArgs e)
        {
            var tfsUser = _presenter.SelecetdTfsUser;
            var skypeUser = skypeUsersListBox.SelectedItem as UserPresenter;
            if ((!string.IsNullOrEmpty(tfsUser)) && (skypeUser != null))
            {
                if (_presenter.Mappings.Contains(tfsUser))
                {
                    _presenter.Mappings[tfsUser].SkypeName = skypeUser.User.Handle;
                }
                else
                {
                    _presenter.Mappings.Add(new UserMapping {TfsName = tfsUser, SkypeName = skypeUser.User.Handle});
                }
            }
            this.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}