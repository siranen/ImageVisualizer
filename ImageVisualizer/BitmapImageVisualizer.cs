using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Diagnostics;

[assembly: DebuggerVisualizer(typeof(ImageVisualizer.BitmapImageVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(Bitmap),
Description = "ImageVisualizer")]

namespace ImageVisualizer
{
    public class BitmapImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var bitmap = (Bitmap)objectProvider.GetObject();

            if (bitmap == null)
                throw new NullReferenceException("Make sure your Bitmap is not null.");

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

        //public static void Debug(Bitmap objectToVisualize)
        //{   
        //    new VisualizerDevelopmentHost(objectToVisualize, typeof(BitmapImageVisualizer)).ShowVisualizer();
        //}
    }
}
