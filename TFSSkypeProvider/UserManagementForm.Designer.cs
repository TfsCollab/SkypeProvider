namespace TfsCommunity.Collaboration.Skype
{
    partial class UserManagementForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagementForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ignoreUserButton = new System.Windows.Forms.Button();
            this.assignUserButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.skypeUsersListBox = new System.Windows.Forms.ListBox();
            this.userPresenterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.tfsUserName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userPresenterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ignoreUserButton);
            this.panel1.Controls.Add(this.assignUserButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 303);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 30);
            this.panel1.TabIndex = 0;
            // 
            // ignoreUserButton
            // 
            this.ignoreUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ignoreUserButton.Location = new System.Drawing.Point(216, 4);
            this.ignoreUserButton.Name = "ignoreUserButton";
            this.ignoreUserButton.Size = new System.Drawing.Size(75, 23);
            this.ignoreUserButton.TabIndex = 2;
            this.ignoreUserButton.Text = "&Ignore";
            this.ignoreUserButton.UseVisualStyleBackColor = true;
            this.ignoreUserButton.Visible = false;
            this.ignoreUserButton.Click += new System.EventHandler(this.ignoreUserButton_Click);
            // 
            // assignUserButton
            // 
            this.assignUserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.assignUserButton.Location = new System.Drawing.Point(297, 4);
            this.assignUserButton.Name = "assignUserButton";
            this.assignUserButton.Size = new System.Drawing.Size(75, 23);
            this.assignUserButton.TabIndex = 1;
            this.assignUserButton.Text = "A&ssign";
            this.assignUserButton.UseVisualStyleBackColor = true;
            this.assignUserButton.Click += new System.EventHandler(this.assignUserButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Team Project Users";
            // 
            // skypeUsersListBox
            // 
            this.skypeUsersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.skypeUsersListBox.DataSource = this.userPresenterBindingSource;
            this.skypeUsersListBox.DisplayMember = "DisplayName";
            this.skypeUsersListBox.Location = new System.Drawing.Point(0, 50);
            this.skypeUsersListBox.Name = "skypeUsersListBox";
            this.skypeUsersListBox.Size = new System.Drawing.Size(360, 251);
            this.skypeUsersListBox.TabIndex = 1;
            this.skypeUsersListBox.ValueMember = "User";
            this.skypeUsersListBox.SelectedIndexChanged += new System.EventHandler(this.skypeUsersListBox_SelectedIndexChanged);
            // 
            // userPresenterBindingSource
            // 
            this.userPresenterBindingSource.DataSource = typeof(TfsCommunity.Collaboration.Skype.UserPresenter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Skype Users";
            // 
            // tfsUserName
            // 
            this.tfsUserName.AutoSize = true;
            this.tfsUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tfsUserName.Location = new System.Drawing.Point(119, 9);
            this.tfsUserName.Name = "tfsUserName";
            this.tfsUserName.Size = new System.Drawing.Size(79, 13);
            this.tfsUserName.TabIndex = 2;
            this.tfsUserName.Text = "tfsUserName";
            // 
            // UserManagementForm
            // 
            this.AcceptButton = this.assignUserButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 333);
            this.Controls.Add(this.tfsUserName);
            this.Controls.Add(this.skypeUsersListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserManagementForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Users";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userPresenterBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button assignUserButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox skypeUsersListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ignoreUserButton;
        private System.Windows.Forms.BindingSource userPresenterBindingSource;
        private System.Windows.Forms.Label tfsUserName;


    }
}