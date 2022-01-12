namespace System {
    internal static class ObjectExtensions {
        public static int ToInt(this object o) {
            if (o != null) {
                var i = 0;
                if (int.TryParse(o.ToString(), out i)) {
                    return i;
                }
            }
            return 0;
        }
        public static bool IsNotNullOrEmpty(this object o) {
            return o != null && !String.IsNullOrEmpty(o.ToString());
        }
        public static bool IsNotNullOrMissing(this object o) {
            return !o.IsNullOrMissing();
        }
        public static bool IsNullOrMissing(this object o) {
            if (o == null) { return true; }
            object missing = Type.Missing;
            if (o == missing) { return true; }
            return false;
        }
    }
}
