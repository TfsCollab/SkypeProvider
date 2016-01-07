using Microsoft.VisualStudio.TestTools.UnitTesting;
using TfsCommunity.Collaboration.Skype;

namespace TFSSkypeProviderTest
{
    /// <summary>
    ///This is a test class for UserMappingCollectionTest and is intended
    ///to contain all UserMappingCollectionTest Unit Tests
    ///</summary>
    [TestClass]
    public class UserMappingCollectionTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod]
        public void LoadSaveTest()
        {
            var target = new UserMappingCollection();
            target.Clear();
            target.Add(new UserMapping {SkypeName = "A", TfsName = "A", IsIgnored = false, IsUnassigned = false});
            target.Add(new UserMapping {SkypeName = "B", TfsName = "B", IsIgnored = false, IsUnassigned = false});
            target.Add(new UserMapping {SkypeName = "C", TfsName = "C", IsIgnored = false, IsUnassigned = false});
            target.Add(new UserMapping {SkypeName = "D", TfsName = "D", IsIgnored = false, IsUnassigned = false});
            target.Save();
            target = new UserMappingCollection();
            target.Load();
            Assert.IsNotNull(target);
            Assert.AreEqual(4, target.Count);
        }
    }
}