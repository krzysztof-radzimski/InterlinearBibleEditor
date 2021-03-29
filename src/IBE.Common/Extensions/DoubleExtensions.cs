/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System;

namespace IBE.Common.Extensions {
    public static  class DoubleExtensions {
        const double INCHES_PER_MM = (1.0 / 25.4);
        const double INCHES_PER_CM = (1.0 / 2.54);
        const double POINTS_PER_INCH = 72.0;

        public static Decimal ToDecimal(this double value) {
            return value.ToDecimal(false);
        }
        public static Decimal ToDecimal(this double value, bool nonnegative) {
            try {
                var r = Convert.ToDecimal(value);
                if (nonnegative && r < 0) {
                    r = 0;
                }

                return r;
            }
            catch {
                return 0;
            }
        }
        public static Decimal ToDecimalUnsigned(this double value) {
            try {
                var r = Convert.ToDecimal(value);
                if (r < 0) {
                    r = 0;
                }

                return r;
            }
            catch {
                return 0;
            }
        }
        public static Decimal ToDecimal(this float value) {
            try {
                return Convert.ToDecimal(value);
            }
            catch {
                return 0;
            }
        }
        public static float ToFloat(this Decimal value) {
            try {
                return (float)value;
            }
            catch {
                return 0;
            }
        }

        public static float ToFloat(this double value) {
            try {
                return (float)value;
            }
            catch { }
            return 0;
        }

        public static int ToInt(this double value) {
            try {
                return Convert.ToInt32(value);
            }
            catch {
                return 0;
            }
        }

        public static float MilimetersToInches(this float milimeters) {
            return milimeters * (float)INCHES_PER_MM;
        }
        public static double MilimetersToInches(this double milimeters) {
            return milimeters * INCHES_PER_MM;
        }
        public static double InchesToPoints(this double inches) {
            return inches * POINTS_PER_INCH;
        }
        public static double MilimetersToPoints(this double milimeters) {
            return milimeters.MilimetersToInches() * POINTS_PER_INCH;
        }
        public static float MilimetersToPoints(this float milimeters) {
            return milimeters.MilimetersToInches() * (float)POINTS_PER_INCH;
        }
        public static float PointsToMilimeters(this float points) {
            return points / ((float)INCHES_PER_MM * (float)POINTS_PER_INCH);
        }
        public static double PointsToMilimeters(this double points) {
            return points / (INCHES_PER_MM * POINTS_PER_INCH);
        }
        public static double PointsToCentimeters(this double points) {
            return PointsToMilimeters(points) / 10.0;
        }
        public static double CentimetersToInches(this double centimeters) {
            return centimeters * INCHES_PER_CM;
        }
        public static float CentimetersToInches(this float centimeters) {
            return centimeters * (float)INCHES_PER_CM;
        }
        public static double CentimetersToPoints(this double centimeters) {
            try {
                return centimeters.CentimetersToInches() * POINTS_PER_INCH;
            }
            catch { }

            return 0;
        }
        public static float CentimetersToPoints(this float centimeters) {
            float PPI = 72.0F;
            float IPC = (1.0F / 2.54F);
            var inches = centimeters * IPC;
            var points = inches * PPI;
            return points;
        }

        public static float CentimetersToPixels(this float centimeters, float dpi) {
            return (centimeters / 2.54F) * dpi;
        }
    }
}
