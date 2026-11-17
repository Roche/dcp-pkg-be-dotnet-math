namespace OpenDCP.Framework.Math.Constants
{
    /// <summary>
    /// Specifies the expected data format resulting from an expression or formula evaluation.
    /// This type dictates how the system validates the output against defined notification thresholds.
    /// </summary>
    public enum OutputType : byte
    {
        /// <summary>
        /// The formula results in a numerical value (e.g., arithmetic like "@sensor1@ + @sensor2@").
        /// Validation typically involves comparing the result against a specific threshold or range.
        /// </summary>
        Numeric = 1,

        /// <summary>
        /// The formula results in a logical state (e.g., a comparison like "0 &lt; @sensor@ &lt; 6").
        /// Validation checks if the expression evaluates to True or False to trigger an alert.
        /// </summary>
        Boolean = 2,

        /// <summary>
        /// The formula results in a character sequence or category label.
        /// Validation typically involves string matching or checking for specific status keywords.
        /// </summary>
        String = 3
    }
}
