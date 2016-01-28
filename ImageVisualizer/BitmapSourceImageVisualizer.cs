using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using ImageVisualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: DebuggerVisualizer(typeof(BitmapSourceImageVisualizer),
 typeof(BitmapSourceImageVisualizerObjectSource),
 Target = typeof(BitmapSource),
 Description = "ImageVisualizer")]

namespace ImageVisualizer
{
    public class BitmapSourceImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var bitmap = new Bitmap(objectProvider.GetData());

            var form = new Form
            {
                Text = String.Format("ImageView - W: {0}px  H: {1}px", bitmap.Width, bitmap.Height),
                ClientSize = new Size(480, 320),
                FormBorderStyle = FormBorderStyle.Sizable,
                ShowInTaskbar = false,
                ShowIcon = false
            };

            var pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = bitmap,
                Parent = form,
                Dock = DockStyle.Fill
            };

            form.ShowDialog();
        }

        //public static void Debug(BitmapSource src)
        //{
        //    new VisualizerDevelopmentHost(src, typeof(BitmapSourceImageVisualizer),
        //        typeof(BitmapSourceImageVisualizerObjectSource)).ShowVisualizer();
        //}
    }

    internal class BitmapSourceImageVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var source = (BitmapSource)target;
            if (source == null)
                throw new NullReferenceException("Make sure your BitmapSource is not null.");

            var encoder = new JpegBitmapEncoder { QualityLevel = 80 };

            try
            {
                encoder.Frames.Add(BitmapFrame.Create(source));
            }
            catch (Exception)
            {
                throw new Exception("Make sure your BitmapSource is correctly initialized.");
            }

            byte[] outgoingBytes;
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                outgoingBytes = stream.ToArray();
                stream.Close();
            }

            outgoingData.Write(outgoingBytes, 0, outgoingBytes.Length);
        }
    }
}

