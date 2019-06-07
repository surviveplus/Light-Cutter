using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

namespace OcrSample
{
    public static class OcrWrapper
    {

        public static async Task<OcrResult> RecognizeAsync(string path) {
            OcrResult result;
            using (var bitmap = await LoadImage(path).ConfigureAwait(false))
            {
                result = await RecognizeAsync(bitmap).ConfigureAwait(false); 
            }
            return result;
        }

        public static async Task<OcrResult> RecognizeAsync(SoftwareBitmap bitmap)
        {
            OcrEngine ocrEngine = null;
            var en = new Windows.Globalization.Language("en");
            if (OcrEngine.IsLanguageSupported(en))
            {
                ocrEngine = OcrEngine.TryCreateFromLanguage(en);
            }
            if(ocrEngine == null)
            {
                ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            }
            Debug.WriteLine(ocrEngine.RecognizerLanguage.DisplayName);

            var ocrResult = await ocrEngine.RecognizeAsync(bitmap);
            return ocrResult;
        } // end function


        private static async Task<SoftwareBitmap> LoadImage(string path)
        {
            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);
            using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                var bitmap = await decoder.GetSoftwareBitmapAsync();
                return bitmap;
            }
        }

    } // end class


    public static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this IAsyncOperation<T> operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            var source = new TaskCompletionSource<T>();
            operation.Completed = delegate 
            {
                switch (operation.Status)  
                {
                    case AsyncStatus.Completed: source.SetResult(operation.GetResults()); break;
                    case AsyncStatus.Error: source.SetException(operation.ErrorCode); break;
                    case AsyncStatus.Canceled: source.SetCanceled(); break;
                }
            };
            return source.Task;
        }
        public static TaskAwaiter<T> GetAwaiter<T>(this IAsyncOperation<T> operation)
        {
            return operation.AsTask().GetAwaiter();
        }
    }
} // end namespace
