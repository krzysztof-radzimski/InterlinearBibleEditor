/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.ComponentModel;

namespace ChurchServices.Extensions {
    public static class FormExtensions {
		public static TResult SafeInvoke<T, TResult>(this T isi, Func<T, TResult> call) where T : ISynchronizeInvoke {
			if (isi.InvokeRequired) {
				IAsyncResult result = isi.BeginInvoke(call, new object[] { isi });
				object endResult = isi.EndInvoke(result); return (TResult)endResult;
			}
			else
				return call(isi);
		}

		public static void SafeInvoke<T>(this T isi, Action<T> call) where T : ISynchronizeInvoke {
			if (isi.InvokeRequired) isi.BeginInvoke(call, new object[] { isi });
			else
				call(isi);
		}
	}
}
