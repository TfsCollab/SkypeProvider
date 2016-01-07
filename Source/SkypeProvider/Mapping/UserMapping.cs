#region

using System.Xml.Serialization;

#endregion

namespace TfsCommunity.Collaboration.Skype.Mapping
{
    /// <summary>
    ///   2012 Power Tools Mapping
    /// </summary>
    public class UserMapping
    {
        /// <summary>
        ///   Gets or sets the name of the T fs.
        /// </summary>
        /// <value> The name of the T fs. </value>
        [XmlAttribute]
        public string TfsContactName { get; set; }

        /// <summary>
        ///   Gets or sets the name of the skype.
        /// </summary>
        /// <value> The name of the skype. </value>
        [XmlAttribute]
        public string IMClientName { get; set; }

        [XmlAttribute]
        public string ProtocolName { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is ignored.
        /// </summary>
        /// <value> <c>true</c> if this instance is ignored; otherwise, <c>false</c> . </value>
        [XmlAttribute]
        public bool IsIgnored { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is unassigned.
        /// </summary>
        /// <value> <c>true</c> if this instance is unassigned; otherwise, <c>false</c> . </value>
        [XmlAttribute]
        public bool IsUnassigned { get; set; }
    }
}