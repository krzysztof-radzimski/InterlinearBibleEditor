namespace ChurchServices.Extensions {
    public static class ListExtensions {
        public static void AddBeforeSelf<T>(this List<T> list, T newElement, T existingElement) {
            int index = list.IndexOf(existingElement);

            if (index != -1) {
                list.Insert(index, newElement);
            }
            else {
                throw new ArgumentException("Existing element not found in the list.");
            }
        }

        public static List<T> Between<T>(this List<T> inputList, int startIndex, int endIndex) {
            if (startIndex < 0 || endIndex >= inputList.Count || startIndex > endIndex) {
                throw new ArgumentOutOfRangeException("Invalid start or end index.");
            }

            List<T> resultList = new List<T>();

            for (int i = startIndex; i <= endIndex; i++) {
                resultList.Add(inputList[i]);
            }

            return resultList;
        }
    }
}
