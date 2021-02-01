using System;

namespace IBE.Data.Import.Extensions {
    public static  class ObjectExtensions {
        public static bool IsNull(this object o) {
            return o == null;
        }

        public static bool IsNotNull(this object o) {
            return o != null;
        }

        public static int ToInt(this object o) {
            if (o.IsNull()) { return 0; }
            if (o is string) {
                if (o.ToString().IsNullOrEmpty()) {
                    return 0;
                }
            }

            try {
                return Convert.ToInt32(o);
            }
            catch {
                return 0;
            }
        }
    }
}
