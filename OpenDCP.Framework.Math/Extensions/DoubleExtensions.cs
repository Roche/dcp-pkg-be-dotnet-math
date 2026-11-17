namespace OpenDCP.Framework.Math.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Tests the relative difference between two values
        /// https://docs.microsoft.com/en-us/dotnet/api/system.double?view=netframework-4.8#Equality
        /// </summary>
        /// <param name="firstValueForCheck"></param>
        /// <param name="secondValueForCheck"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool IsApproximatelyEqual(this double? firstValueForCheck, double? secondValueForCheck, double epsilon)
        {
            if (!firstValueForCheck.HasValue || !secondValueForCheck.HasValue)
            {
                return false;
            }
            // If they are equal anyway, just return True.
            if (firstValueForCheck.Equals(secondValueForCheck))
                return true;

            // Handle NaN, Infinity.
            if (double.IsInfinity(firstValueForCheck.Value) | double.IsNaN(firstValueForCheck.Value))
                return firstValueForCheck.Equals(secondValueForCheck);
            else if (double.IsInfinity(secondValueForCheck.Value) | double.IsNaN(secondValueForCheck.Value))
                return firstValueForCheck.Equals(secondValueForCheck);

            // Handle zero to avoid division by zero
            double divisor = System.Math.Max(firstValueForCheck.Value, secondValueForCheck.Value);
            if (divisor.Equals(0))
                divisor = System.Math.Min(firstValueForCheck.Value, secondValueForCheck.Value);

            return System.Math.Abs((firstValueForCheck.Value - secondValueForCheck.Value) / divisor) <= epsilon;
        }
    }
}
