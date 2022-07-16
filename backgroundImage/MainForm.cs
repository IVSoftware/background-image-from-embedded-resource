using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace backgroundImage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            // Get the names of the embedded resources in the Images folder
            var images =
                this.GetType().Assembly
                .GetManifestResourceNames()
                .Where(name => name.Contains(".Images."))
                .ToArray();

            // Get the random index
            var index = Rando.Next(images.Length);
            BeginInvoke((MethodInvoker)delegate 
            {
                var raw = images[index];
                // Parse what you want to go in the Textbox name
                textBox1.Text = Path.GetFileNameWithoutExtension(raw).Split('.').Last();

                // Load the resource into an image and set it as background
                using(var stream = this.GetType().Assembly.GetManifestResourceStream(raw))
                {
                    this.BackgroundImage = Image.FromStream(stream);
                }
            });
        }
        Random Rando = new Random();

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Try to get single matching embedded resource in the Images folder
            var images =
                this.GetType().Assembly
                .GetManifestResourceNames()
                .Where(name => name.Contains(".Images."))
                .Where(name => name.ToLower().Contains(textBox1.Text.ToLower()))
                .ToArray();

            if(images.Length == 1)
            {
                // Load the resource into an image and set it as background
                using (var stream = this.GetType().Assembly.GetManifestResourceStream(images.First()))
                {
                    this.BackgroundImage = Image.FromStream(stream);
                }
            }
        }
    }
}
