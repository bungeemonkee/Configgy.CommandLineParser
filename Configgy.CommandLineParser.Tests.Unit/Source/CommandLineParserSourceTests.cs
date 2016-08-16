using System.Diagnostics.CodeAnalysis;
using Configgy.CommandLineParser.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configgy.CommandLineParser.Tests.Unit.Source
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CommandLineParserSourceTests
    {
        [TestMethod]
        public void Constructor_With_Arguments_Populates_Options()
        {
            var arguments = new string[0];
            var source = new CommandLineParserSource<object>(arguments);

            Assert.IsNotNull(source.Options);
        }

        [TestMethod]
        public void Constructor_With_Options_Populates_Options()
        {
            var options = new object();
            var source = new CommandLineParserSource<object>(options);

            Assert.IsNotNull(source.Options);
            Assert.AreSame(options, source.Options);
        }

        [TestMethod]
        public void Get_Returns_False_And_Null_When_Property_Doesnt_Exist()
        {
            const string name = "property";

            var options = new object();
            var source = new CommandLineParserSource<object>(options);

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsFalse(result);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void Get_Returns_True_And_Value_When_Property_Exists()
        {
            const string name = nameof(TestOptions.StringProperty);
            const string expected = "expected";

            var options = new TestOptions
            {
                StringProperty = expected
            };
            var source = new CommandLineParserSource<TestOptions>(options);

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, value);
        }

        [TestMethod]
        public void Get_Returns_True_And_Null_When_Property_Exists_But_Is_Null()
        {
            const string name = nameof(TestOptions.StringProperty);

            var options = new TestOptions();
            var source = new CommandLineParserSource<TestOptions>(options);

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsTrue(result);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void Get_Converts_Non_String_Value_To_String()
        {
            const string name = nameof(TestOptions.NonStringProperty);
            const int expectedInt = 10;
            const string expectedString = "10";

            var options = new TestOptions
            {
                NonStringProperty = expectedInt
            };
            var source = new CommandLineParserSource<TestOptions>(options);

            string value;
            var result = source.Get(name, null, out value);

            Assert.IsTrue(result);
            Assert.AreEqual(expectedString, value);
        }
    }
}
