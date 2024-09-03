using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;  

namespace MemoryGame
{
    public partial class Form1 : Form
    {
        private List<Button> buttons;
        private Timer timer;
        private List<Image> images;
        private Button firstClicked, secondClicked;
        private int matchCount;
        private bool isChecking;

        public Form1()
        {
            InitializeComponent();

            // Initialize Timer
            timer = new Timer { Interval = 1000 }; // 1 second for hiding images
            timer.Tick += Timer_Tick;

            // Initialize Images
            images = LoadImages(); // Load your 8 sushi images
            buttons = new List<Button>();

            // Create Buttons
            CreateButtons();

            // Start Timer to show images
            ShowAllImages();
        }

        private void CreateButtons()
        {
            var shuffledImages = images.Concat(images).OrderBy(x => Guid.NewGuid()).ToList();
            int rowCount = tableLayoutPanel.RowCount;
            int columnCount = tableLayoutPanel.ColumnCount;

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    Button button = new Button
                    {
                        Dock = DockStyle.Fill,
                        Tag = shuffledImages[row * columnCount + col],
                        BackgroundImageLayout = ImageLayout.Stretch,
                        BackgroundImage = Properties.Resources.what // A question mark image
                    };
                    button.Click += Button_Click;
                    tableLayoutPanel.Controls.Add(button, col, row);
                    buttons.Add(button);
                }
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (isChecking) return;

            var button = sender as Button;

            if (firstClicked == null)
            {
                firstClicked = button;
                firstClicked.BackgroundImage = (Image)firstClicked.Tag;
            }
            else if (secondClicked == null)
            {
                secondClicked = button;
                secondClicked.BackgroundImage = (Image)secondClicked.Tag;

                if (firstClicked.Tag.Equals(secondClicked.Tag))
                {
                    matchCount++;
                    firstClicked = null;
                    secondClicked = null;

                    if (matchCount == 8)
                    {
                        MessageBox.Show("You won!");
                    }
                }
                else
                {
                    // Start timer to hide non-matching images
                    isChecking = true;
                    timer.Start();
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (firstClicked != null && secondClicked != null)
            {
                firstClicked.BackgroundImage = Properties.Resources.what;
                secondClicked.BackgroundImage = Properties.Resources.what;
            }

            firstClicked = null;
            secondClicked = null;
            isChecking = false;
        }

        private void ShowAllImages()
        {
            // Display all sushi images for a few seconds
            foreach (var button in buttons)
            {
                button.BackgroundImage = (Image)button.Tag;
            }

            // Set a timer to hide images after 3 seconds
            Timer hideTimer = new Timer { Interval = 3000 }; // 3 seconds
            hideTimer.Tick += (s, e) =>
            {
                hideTimer.Stop();
                foreach (var button in buttons)
                {
                    button.BackgroundImage = Properties.Resources.what; // Set to question mark
                }
            };
            hideTimer.Start();
        }

        private List<Image> LoadImages()
        {
            // Create a list to store the images
            var images = new List<Image>
            {
                Properties.Resources.sushi1,
                Properties.Resources.sushi2,
                Properties.Resources.sushi3,
                Properties.Resources.sushi4,
                Properties.Resources.sushi5,
                Properties.Resources.sushi6,
                Properties.Resources.sushi7,
                Properties.Resources.sushi8
            };

            // Return the list of images
            return images;
        }
    }
}
