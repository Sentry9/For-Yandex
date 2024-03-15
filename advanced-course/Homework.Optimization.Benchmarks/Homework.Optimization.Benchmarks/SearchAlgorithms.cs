namespace Homework.Optimization.Core;

public static class SearchAlgorithms
{
    /// <summary>
    /// Найти элемент в отсортированном массиве целых чисел.
    /// Сложность по памяти O(1), сложность по времени О(n)
    /// </summary>
    /// <remarks>
    /// Алгоритм не выполняет проверку, что массив отсортирован.
    /// </remarks>
    /// <param name="array">Отсортированный массив целых чисел.</param>
    /// <param name="value">Искомое число.</param>
    /// <returns>Искомое число. Если его нет в массиве, то -1.</returns>
    public static int FindElementInSortedArray(int[] array, int value)
    {
        for (var index = 0; index < array.Length; index++)
        {
            var item = array[index];
            if (item == value)
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// Найти элемент в отсортированном массиве целых чисел. 
    /// </summary>
    /// <remarks>
    /// Алгоритм не выполняет проверку, что массив отсортирован.
    /// </remarks>
    /// <param name="array">Отсортированный массив целых чисел.</param>
    /// <param name="value">Искомое число.</param>
    /// <returns>Искомое число. Если его нет в массиве, то -1.</returns>
    public static int FindElementInSortedArrayOptimized(int[] array, int value)
    {
        int left = 0;
        int right = array.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (array[mid] == value)
            {
                return mid;
            }
             if (array[mid] < value)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return -1;
    }
}