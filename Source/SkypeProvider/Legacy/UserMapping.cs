#region

using System.Xml.Serialization;

#endregion

namespace TfsCommunity.Collaboration.Skype.Legacy
{
    /// <summary>
    /// Mapping definition from the Skype provider for the TFS powertools 2008, 2010
    /// To make the mapping as easy as possible for the users the provider considers also old mappings
    /// </summary>
    public class UserMapping
    {
        /// <summary>
        ///   Gets or sets TFS Username
        /// </summary>
        /// <value>(legacy) TFS Username - Active Directory E-Mailadress</value>
        [XmlAttribute]
        public string TfsName { get; set; }

        /// <summary>
        ///   Gets or sets the Skype username 
        /// </summary>
        /// <value>Skype username (handle)</value>
        [XmlAttribute]
        public string SkypeName { get; set; }

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