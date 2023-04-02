namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator<TChar>
{
    /// <summary>
    /// State of UTF-8 comparison.
    /// </summary>
    private sealed record ComparisonState
    {
        /// <summary>
        /// Indicates whether current comparison is case-insensitive.
        /// </summary>
        private readonly Boolean _ignoreCase;
        /// <summary>
        /// Count of perfomed comparison.
        /// </summary>
        private Int32 _count;
        /// <summary>
        /// Indicates whether the comparison must be continue.
        /// </summary>
        private Boolean _continue;

        /// <summary>
        /// Indicates whether current comparison is case-insensitive.
        /// </summary>
        public Boolean IgnoreCase => this._ignoreCase || this._count > 1;
        /// <summary>
        /// Indicates whether the comparison must be continue.
        /// </summary>
        public Boolean Continue => this._continue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ignoreCase">Indicates whether the comparison must be continue.</param>
        public ComparisonState(Boolean ignoreCase)
        {
            this._ignoreCase = ignoreCase;
        }

        /// <summary>
        /// Initialize a new comparison. 
        /// </summary>
        public void InitializeComparison()
        {
            this._count++;
            this._continue = false;
        }

        /// <summary>
        /// Sets the continue flag as <see langword="true"/>.
        /// </summary>
        public void SetContinue() => this._continue = true;
    }
}

