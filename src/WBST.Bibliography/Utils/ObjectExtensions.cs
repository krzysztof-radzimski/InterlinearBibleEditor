namespace System {
    internal static class ObjectExtensions {
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
