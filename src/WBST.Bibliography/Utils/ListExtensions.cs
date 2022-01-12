using System.Collections.Generic;

namespace WBST.Bibliography.Utils {
    internal static class ListExtensions {
        public static void MoveUp<T>(this List<T> list, T item) {
            var oldIndex = list.IndexOf(item);
            if (oldIndex > 0) {
                list.Move(item, oldIndex - 1);
            }
        }
        public static void MoveDown<T>(this List<T> list, T item) {
            var oldIndex = list.IndexOf(item);
            if (oldIndex < list.Count - 1) {
                list.Move(item, oldIndex + 1);
            }
        }
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex) {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            if (newIndex > oldIndex) newIndex--;
            // the actual index could have shifted due to the removal

            list.Insert(newIndex, item);
        }

        public static void Move<T>(this List<T> list, T item, int newIndex) {
            if (item != null) {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1) {
                    list.RemoveAt(oldIndex);

                    if (newIndex > oldIndex) newIndex--;
                    // the actual index could have shifted due to the removal

                    list.Insert(newIndex, item);
                }
            }

        }
    }
}
