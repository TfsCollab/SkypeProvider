using System;
using Microsoft.TeamFoundation.Collaboration;
using SKYPE4COMLib;

namespace TfsCommunity.Collaboration.Skype
{
    internal class SkypeConversation : IConversation
    {
        #region Implementation of IDisposable

        /// <summary>
        ///                     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _skypeChat = null;
            _skypeCall = null;
        }

        #endregion

        private ICall _skypeCall;

        private IChat _skypeChat;

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeConversation"/> class.
        /// </summary>
        /// <param name="skypeChat">The skype chat.</param>
        public SkypeConversation(IChat skypeChat)
        {
            _skypeChat = skypeChat;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeConversation"/> class.
        /// </summary>
        /// <param name="skypeCall">The skype call.</param>
        public SkypeConversation(ICall skypeCall)
        {
            _skypeCall = skypeCall;
        }

        #region Implementation of IConversation

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (_skypeChat != null)
            {
                _skypeChat.Leave();
            }
            if (_skypeCall != null)
            {
                _skypeCall.Finish();
            }
        }

        /// <summary>
        /// Sends the text.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendText(string message)
        {
            if (_skypeChat != null)
            {
                _skypeChat.SendMessage(message);
            }
            if (_skypeCall != null)
            {
                // Send the message to all participants of the call
                var users = new UserCollection();
                foreach (Participant participant in _skypeCall.Participants)
                {
                    users.Add(Skype.get_User(participant.Handle));
                }
                IChat chat = Skype.CreateChatMultiple(users);
                chat.SendMessage(message);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                if (_skypeCall != null)
                {
                    return _skypeCall.Subject;
                }
                if (_skypeChat != null)
                {
                    return _skypeChat.Topic;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the history.
        /// </summary>
        /// <value>The history.</value>
        public string History
        {
            get
            {
                ChatMessageCollection chatMessageCollection = _skypeChat.RecentMessages;
                String output = string.Empty;
                foreach (ChatMessage chatMessage in chatMessageCollection)
                {
                    output += string.Format("{0} {1}: {2}", chatMessage.Timestamp, chatMessage.FromDisplayName,
                                            chatMessage.Body);
                }
                return output;
            }
        }

        #endregion

        /// <summary>
        /// Gets the skype call.
        /// </summary>
        /// <value>The skype call.</value>
        public ICall SkypeCall
        {
            get { return _skypeCall; }
        }

        /// <summary>
        /// Gets the skype chat.
        /// </summary>
        /// <value>The skype chat.</value>
        public IChat SkypeChat
        {
            get { return _skypeChat; }
        }

        /// <summary>
        /// Gets or sets the skype client.
        /// </summary>
        /// <value>The skype.</value>
        public ISkype Skype { get; set; }
    }
}