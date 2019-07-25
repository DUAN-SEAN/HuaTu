namespace HuaTu.Controls.Internal.Dockable.Base
{
    /// <summary>
    /// Utility for enums
    /// </summary>
    internal class EnumUtility
    {
        /// <summary>
        /// Check if current value contains the checked value
        /// </summary>
        /// <param name="currentValue">current value</param>
        /// <param name="checkedValue">checked value</param>
        /// <returns>true if the current value contains checked value</returns>
        public static bool Contains(object currentValue, object checkedValue)
        {
            return ((int)currentValue & (int)checkedValue) == (int)checkedValue;
        }

        /// <summary>
        /// Exclude given value from the current value
        /// </summary>
        /// <typeparam name="T">type of the enum</typeparam>
        /// <param name="excludeValue">value to exclude</param>
        /// <param name="currentValue">current value to be changed</param>
        public static void Exclude<T>(T excludeValue, ref T currentValue)
        {
            if (Contains(currentValue, excludeValue))
            {
                currentValue = (T)(object)((int)(object)currentValue ^ (int)(object)excludeValue);
            }
        }
    }

}