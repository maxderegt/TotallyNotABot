using System.Transactions;

namespace TotallyNotABot.DiscordFormat
{
    class DiscordString
    {
        public string Message { get; set; }
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
        public bool IsUnderline { get; set; } = false;

        private DiscordString(string message)
        {
            this.Message = message;
        }

        public override string ToString()
        {
            string message = this.Message;
            if (this.IsBold)
            {
                message = $"**{message}**";
            }
            if (this.IsItalic)
            {
                message = $"_{message}_";
            }
            if (this.IsUnderline)
            {
                message = $"__{message}__";
            }

            return message;
        }

        /// <summary>
        /// Create a DiscordString instance that is bold.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A bold discord string</returns>
        public static DiscordString Bold(string message)
        {
            return new DiscordString(message) {IsBold = true};
        }

        /// <summary>
        /// Modify a discord string to make it bold
        /// </summary>
        /// <returns>The modified discord string</returns>
        public DiscordString Bold()
        {
            IsBold = true;
            return this;
        }

        /// <summary>
        /// Crate a DiscordString instance that is italic.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>An italic discord string</returns>
        public static DiscordString Italic(string message)
        {
            return new DiscordString(message) {IsItalic = true};
        }

        /// <summary>
        /// Modify a discord string to make it bold
        /// </summary>
        /// <returns>The modified discord string</returns>
        public DiscordString Italic()
        {
            IsItalic = true;
            return this;
        }

        /// <summary>
        /// Create a DiscordString instance that is underlined.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>An underlined discord string</returns>
        public static DiscordString Underline(string message)
        {
            return new DiscordString(message) {IsUnderline = true};
        }

        /// <summary>
        /// Modify a discord string to make it underlined.
        /// </summary>
        /// <returns>The modified discord string</returns>
        public DiscordString Underline()
        {
            IsUnderline = true;
            return this;
        }
    }
}
