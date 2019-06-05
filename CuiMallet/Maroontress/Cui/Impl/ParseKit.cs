namespace Maroontress.Cui.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The factory of <see cref="OptionParsingException"/> objects.
    /// </summary>
    public sealed class ParseKit
    {
        private readonly OptionSchema schema;
        private readonly Func<string> option;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseKit"/> class.
        /// </summary>
        /// <param name="schema">
        /// The option schema.
        /// </param>
        /// <param name="option">
        /// The long name of the option. It can be followed by <c>=value</c>.
        /// </param>
        public ParseKit(OptionSchema schema, string option)
        {
            this.schema = schema;
            this.option = () => option;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseKit"/> class.
        /// </summary>
        /// <param name="schema">
        /// The option schema.
        /// </param>
        /// <param name="option">
        /// The short name of the option.
        /// </param>
        public ParseKit(OptionSchema schema, char option)
        {
            this.schema = schema;
            this.option = () => $"-{option}";
        }

        /// <summary>
        /// Gets the string representing the specified instance.
        /// </summary>
        /// <param name="kit">
        /// The instance.
        /// </param>
        public static implicit operator string(ParseKit kit)
            => kit.ToString();

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option which requires an argument has no argument.
        /// </summary>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception NewMissingArgument()
        {
            var m = string.Format(Resources.MissingArgument, option());
            return new OptionParsingException(schema, m);
        }

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option which requires no argument has an extra argument.
        /// </summary>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception NewUnableToGetArgument()
        {
            var m = string.Format(Resources.UnableToGetArgument, option());
            return new OptionParsingException(schema, m);
        }

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option is not found in the schema.
        /// </summary>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception NewUnknownOption()
        {
            var m = string.Format(Resources.UnknownOption, option());
            return new OptionParsingException(schema, m);
        }

        /// <summary>
        /// Gets a new <see cref="OptionParsingException"/> representing
        /// that the option name is ambiguous since the abbreviations are
        /// not unique.
        /// </summary>
        /// <param name="candidates">
        /// The names of the candidates.
        /// </param>
        /// <returns>
        /// The new exception.
        /// </returns>
        public Exception NewAmbiguousOption(IEnumerable<string> candidates)
        {
            var all = string.Join(", ", candidates.Select(n => $"--{n}"));
            var m = string.Format(Resources.AmbiguousOption, option(), all);
            return new OptionParsingException(schema, m);
        }

        /// <inheritdoc/>
        public sealed override string ToString() => option();
    }
}
