using glTFLoader;
using NUnit.Framework;


namespace glTFLoaderUnitTests
{
    [TestFixture]
    public class DataUrlTests
    {
        [Test]
        public void ParseDataURL_EmptyString()
        {
            var result = DataURL.FromUri("", out DataURL actual);
            Assert.IsFalse(result);
            Assert.IsNull(actual);            
        }

        [Test]
        public void ParseDataURL_NullString()
        {
            var result = DataURL.FromUri(null, out DataURL actual);
            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void ParseDataURL_ABriefNote()
        {
            // FROM https://tools.ietf.org/html/rfc2397
            var result = DataURL.FromUri("data:,A%20brief%20note", out DataURL actual);
            Assert.IsTrue(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual("text/plain", actual.MediaType);
            Assert.AreEqual("US-ASCII", actual.CharSet);
            Assert.IsNull(actual.Data);
            Assert.AreEqual("A brief note", actual.DecodedString);
        }

        [Test]
        public void ParseDataURL_GreekLetters_0()
        {
            // NOT SURE IF 0x value of fg is valid, so changed to f6
            var result = DataURL.FromUri("data:text/plain;charset=iso-8859-7,%be%f6%be", out DataURL actual);
            Assert.IsTrue(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual("text/plain", actual.MediaType);
            Assert.AreEqual("iso-8859-7", actual.CharSet);
            Assert.IsNull(actual.Data);
            Assert.AreEqual("ΎφΎ", actual.DecodedString);
        }
        
        [Test]
        public void ParseDataURL_GreekLetters_1()
        {
            // FROM https://tools.ietf.org/html/rfc2397
            var result = DataURL.FromUri("data:text/plain;charset=iso-8859-7,%be%fg%be", out DataURL actual);
            Assert.IsTrue(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual("text/plain", actual.MediaType);
            Assert.AreEqual("iso-8859-7", actual.CharSet);
            Assert.IsNull(actual.Data);
            Assert.AreEqual("Ύ%fgΎ", actual.DecodedString);
        }

        [Test]
        public void ParseDataURL_UserSpecificAppLaunch()
        {
            // FROM https://tools.ietf.org/html/rfc2397
            var result = DataURL.FromUri("data:application/vnd-xxx-query, select_vcount, fcol_from_fieldtable / local", out DataURL actual);
            Assert.IsTrue(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual("application/vnd-xxx-query", actual.MediaType);
            Assert.AreEqual("US-ASCII", actual.CharSet);
            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(" select_vcount, fcol_from_fieldtable / local", actual.DecodedString);
        }

        [Test]
        public void ParseDataURL_Base64Sample()
        {
            // FROM https://tools.ietf.org/html/rfc2397
            var result = DataURL.FromUri(
                @"data:image/gif;base64,R0lGODdhMAAwAPAAAAAAAP///ywAAAAAMAAw
                AAAC8IyPqcvt3wCcDkiLc7C0qwyGHhSWpjQu5yqmCYsapyuvUUlvONmOZtfzgFz
                ByTB10QgxOR0TqBQejhRNzOfkVJ + 5YiUqrXF5Y5lKh / DeuNcP5yLWGsEbtLiOSp
                a / TPg7JpJHxyendzWTBfX0cxOnKPjgBzi4diinWGdkF8kjdfnycQZXZeYGejmJl
                ZeGl9i2icVqaNVailT6F5iJ90m6mvuTS4OK05M0vDk0Q4XUtwvKOzrcd3iq9uis
                F81M1OIcR7lEewwcLp7tuNNkM3uNna3F2JQFo97Vriy / Xl4 / f1cf5VWzXyym7PH
                hhx4dbgYKAAA7", out DataURL actual);
            Assert.IsTrue(result);
            Assert.IsNotNull(actual);
            Assert.AreEqual("image/gif", actual.MediaType);
            Assert.AreEqual("US-ASCII", actual.CharSet);
            Assert.IsNotNull(actual.Data);
        }
    }
}
