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
        /// Indicates whether the current comparison is for equalization purpose.
        /// </summary>
        private readonly Boolean _isEqualization;

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
        /// Indicates whether the current comparison is for equalization purpose.
        /// </summary>
        public Boolean IsEqualization => this._isEqualization;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ignoreCase">Indicates whether the comparison must be continue.</param>
        public ComparisonState(Boolean ignoreCase)
        {
            this._ignoreCase = ignoreCase;
            this._isEqualization = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ignoreCase">Indicates whether the comparison must be continue.</param>
        /// <param name="isEqualization">Indicates whether the current comparison is for equalization purpose.</param>
        public ComparisonState(Boolean ignoreCase, Boolean isEqualization)
        {
            this._ignoreCase = ignoreCase;
            this._isEqualization = isEqualization;
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

