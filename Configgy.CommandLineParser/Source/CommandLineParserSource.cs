using System;
using System.Reflection;
using CommandLine;
using Configgy.Source;

namespace Configgy.CommandLineParser.Source
{
    /// <summary>
    /// An <see cref="IValueSource"/> that gets values from a command line parsed using <see cref="Parser"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandLineParserSource<T> : IValueSource
        where T : new()
    {
        /// <summary>
        /// The command line options instance associated with this source.
        /// </summary>
        public T Options { get; }

        /// <summary>
        /// Create a CommandLineParserSource that parses the given arguments using the default <see cref="Parser"/> instance.
        /// </summary>
        /// <param name="arguments">The command line arguments to parse.</param>
        public CommandLineParserSource(string[] arguments)
            : this(arguments, Parser.Default)
        { }

        /// <summary>
        /// Create a CommandLineParserSource that parses the given arguments using the given <see cref="Parser"/>.
        /// </summary>
        /// <param name="arguments">The command line arguments to parse.</param>
        /// <param name="parser">The parser to use.</param>
        public CommandLineParserSource(string[] arguments, Parser parser)
        {
            Options = new T();
            if (parser.ParseArguments(arguments, Options)) return;
            throw new ArgumentException("Unrecognized command line option.");
        }

        /// <summary>
        /// Create a CommandLineParserSource using an existing options instance.
        /// </summary>
        /// <param name="options">The options to use as the source. These options must already have ben populated using <see cref="Parser"/>.</param>
        public CommandLineParserSource(T options)
        {
            Options = options;
        }

        /// <summary>
        /// See <see cref="IValueSource.Get"/>.
        /// </summary>
        public bool Get(string valueName, PropertyInfo property, out string value)
        {
            var optionProperty = typeof(T)
                .GetProperty(valueName, BindingFlags.Instance | BindingFlags.Public);

            // If the property doesn't exist...
            if (optionProperty == null)
            {
                // Return failure
                value = null;
                return false;
            }

            // Get the value from the property and return success
            var result = optionProperty.GetValue(Options);
            value = result as string ?? result?.ToString();
            return true;
        }
    }
}
