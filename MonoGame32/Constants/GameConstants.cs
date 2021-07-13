namespace MonoGame32.Constants
{
    public static class GameConstants
    {
        private const float Ppm = 16f;

        /*
        /// <summary>
        /// Calculates the sum of all odd numbers in an array.
        /// </summary>
        /// <param name="array">The array to use.</param>
        /// <returns>The sum of all odd numbers.</returns>
        private static int SumOfOdds(int[] array)
        {
            var counter = 0;
            for (var i = 0; i < array.Length; i++)
            {
                var element = array[i];
                if (element % 2 != 0) // If dividing element by 2 has remainder = odd. All even numbers have no remainder.
                    counter += element;
            }

            return counter;
        }

        /// <summary>
        /// Calculates the sum of all odd numbers in an array.<br/><b>Around 1.14x times faster than first function.</b>
        /// This is a faster function which skips the use of modulus operator and checks first bit instead.
        /// </summary>
        /// <param name="array">The array to use.</param>
        /// <returns>The sum of all odd numbers.</returns>
        private static int SumOfOddsNoModulus(int[] array)
        {
            var counter = 0;
            for (var i = 0; i < array.Length; i++)
            {
                var element = array[i];
                if ((element & 1) == 1) // If first bit in element is 1 = odd number. All odd numbers have first bit set to 1.
                    counter += element;
            }

            return counter;
        }

        /// <summary>
        /// Calculates the sum of all odd numbers in an array.<br/><b>Around 5.4x(!) times faster than first function.</b>
        /// This is an even faster function, which skips the use of modulus operator and skips checking the first bit.
        /// This function also eliminates the use of the if-statement.
        /// </summary>
        /// <param name="array">The array to use.</param>
        /// <returns>The sum of all odd numbers.</returns>
        private static int SumOfOddsNoModulusNoBranch(int[] array)
        {
            var counter = 0;
            for (var i = 0; i < array.Length; i++)
            {
                var element = array[i];
                var odd = element & 1; // 0: is even. 1: is odd.
                counter += (odd * element); // Increase with 0 or the odd element.
            }

            return counter;
        }
        */
    }
}