/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Utils {
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement> {

        readonly List<TElement> elements;

        public Grouping(IGrouping<TKey, TElement> grouping) {
            if (grouping == null)
                throw new ArgumentNullException("grouping");
            Key = grouping.Key;
            elements = grouping.ToList();
        }
        public Grouping(TKey key, IEnumerable<TElement> items) {
            if (key == null)
                throw new ArgumentNullException("key");
            if (items == null)
                throw new ArgumentNullException("items");
            Key = key;
            elements = items.ToList();
        }
        public TKey Key { get; private set; }

        public IEnumerator<TElement> GetEnumerator() {
            return this.elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public int Count => elements != null ? elements.Count : 0;
    }
}
