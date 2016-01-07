using System.Xml.Serialization;

namespace TfsCommunity.Collaboration.Skype
{
    public class UserMapping
    {
        /// <summary>
        /// Gets or sets the name of the T fs.
        /// </summary>
        /// <value>The name of the T fs.</value>
        [XmlAttribute]
        public string TfsName { get; set; }

        /// <summary>
        /// Gets or sets the name of the skype.
        /// </summary>
        /// <value>The name of the skype.</value>
        [XmlAttribute]
        public string SkypeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ignored.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is ignored; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute]
        public bool IsIgnored { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is unassigned.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is unassigned; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute]
        public bool IsUnassigned { get; set; }
    }
}