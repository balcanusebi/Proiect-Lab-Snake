using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game
{
    public partial class Form1 : Form
    {
        //Snake defaults
        PictureBox[] snakeParts;
        int snakeSize = 5;
        Point location = new Point(120, 120);
        string direction = "Right";
        bool changingDirection = false;

        //Food defaults
        PictureBox food = new PictureBox();
        Point foodLocation = new Point(0, 0);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //in case of user want to play again when game is over
            gamePanel.Controls.Clear();
            snakeParts = null;
            scoreLabel.Text = "0";
            snakeSize = 5;
            direction = "Right";
            location = new Point(120, 120);

            //Start game
            drawSnake();
            drawFood();

            timer1.Start();

            //Disable some controls
            trackBar1.Enabled = false;
            startButton.Enabled = false;
            nameBox.Enabled = false;

            //Enable Stop button
            stopButton.Enabled = true;

        }

        private void drawSnake()
        {
            snakeParts = new PictureBox[snakeSize];

            //Loop for drawing each snake part one after another
            for (int i=0; i < snakeSize; i++)
            {
                snakeParts[i] = new PictureBox();
                snakeParts[i].Size = new Size(15, 15);
                snakeParts[i].BackColor = Color.Red;
                snakeParts[i].BorderStyle = BorderStyle.FixedSingle;
                snakeParts[i].Location = new Point(location.X - (15 * i), location.Y);
                gamePanel.Controls.Add(snakeParts[i]);
            }
        }

        private void drawFood()
        {
            Random rnd = new Random();
            int Xrand = rnd.Next(38) * 15;
            int Yrand = rnd.Next(30) * 15;

            bool isOnSnake = true;

            //check if food is on snake body
            while (isOnSnake)
            {
                for (int i = 0; i < snakeSize; i++)
                {
                    if (snakeParts[i].Location == new Point (Xrand, Yrand))
                    {
                        Xrand = rnd.Next(38) * 15;
                        Yrand = rnd.Next(30) * 15;
                    }
                    else
                    {
                        isOnSnake = false;
                    }
                }
            }
            //Now draw food
            if (isOnSnake == false)
            {
                foodLocation = new Point(Xrand, Yrand);
                food.Size = new Size(15, 15);
                food.BackColor = Color.Yellow;
                food.BorderStyle = BorderStyle.FixedSingle;
                food.Location = foodLocation;
                gamePanel.Controls.Add(food);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            move();
        }

        private void move()
        {
            Point point = new Point(0, 0);

            //Loop for moving each part of snake according to direction
            for (int i = 0; i < snakeSize; i++)
            {
                if (i == 0)
                {
                    point = snakeParts[i].Location;
                    if (direction == "Left")
                    {
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X - 15, snakeParts[i].Location.Y);
                    }
                    if (direction == "Right")
                    {
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X + 15, snakeParts[i].Location.Y);
                    }
                    if (direction == "Up")
                    {
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X, snakeParts[i].Location.Y - 15);
                    }
                    if (direction == "Down")
                    {
                        snakeParts[i].Location = new Point(snakeParts[i].Location.X, snakeParts[i].Location.Y + 15);
                    }
                }
                else
                {
                    Point newPoint = snakeParts[i].Location;
                    snakeParts[i].Location = point;
                    point = newPoint;
                }
            }

            //if snake hits food
            if (snakeParts[0].Location == foodLocation)
            {
                eatFood();
                drawFood();
            }

            //if snake hits any wall
            if (snakeParts[0].Location.X < 0 || snakeParts[0].Location.X >= 570 || snakeParts[0].Location.Y < 0 || snakeParts[0].Location.Y >=450)
            {
                stopGame();
            }

            //if snake hits itself
            for (int i = 3; i < snakeSize; i++)
            {
                if (snakeParts[0].Location == snakeParts[i].Location)
                {
                    stopGame();
                }
            }




            changingDirection = false;
        }

        //Now handle user input to control snake
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == (Keys.Up) && direction != "Down" && changingDirection != true)
            {
                direction = "Up";
                changingDirection = true;
            }
            if (keyData == (Keys.Down) && direction != "Up" && changingDirection != true)
            {
                direction = "Down";
                changingDirection = true;
            }
            if (keyData == (Keys.Left) && direction != "Right" && changingDirection != true)
            {
                direction = "Left";
                changingDirection = true;
            }
            if (keyData == (Keys.Right) && direction != "Left" && changingDirection != true)
            {
                direction = "Right";
                changingDirection = true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //Change interval of timer with the value of speed trackbar
            timer1.Interval = 501 - (5 * trackBar1.Value);
        }

        private void eatFood()
        {
            snakeSize++;

            //save old snake and use it to create new bigger snake with incremented rectangle
            PictureBox[] oldSnake = snakeParts;
            gamePanel.Controls.Clear();
            snakeParts = new PictureBox[snakeSize];

            for (int i = 0; i < snakeSize; i++)
            {
                snakeParts[i] = new PictureBox();
                snakeParts[i].Size = new Size(15, 15);
                snakeParts[i].BackColor = Color.Red;
                snakeParts[i].BorderStyle = BorderStyle.FixedSingle;

                if (i==0)
                {
                    snakeParts[i].Location = foodLocation;
                }
                else
                {
                    snakeParts[i].Location = oldSnake[i - 1].Location;
                }
                gamePanel.Controls.Add(snakeParts[i]);
            }

            //Update scores
            int currentScores = Int32.Parse(scoreLabel.Text);
            int newScore = currentScores + 10;
            scoreLabel.Text = newScore + "";
        }

        private void stopGame()
        {
            timer1.Stop();
            startButton.Enabled = true;
            trackBar1.Enabled = true;
            stopButton.Enabled = false;
            nameBox.Enabled = true;

            //Game Over Label
            Label over = new Label();
            over.Text = "Game/nOver";
            over.ForeColor = Color.White;
            over.Font = new Font("Arial", 100, FontStyle.Bold);
            over.Size = over.PreferredSize;
            over.TextAlign = ContentAlignment.MiddleCenter;
            over.BringToFront();

            //Fint center point to place label
            int X = gamePanel.Width / 2 - over.Width / 2;
            int Y = gamePanel.Width / 2 - over.Height / 2;
            over.Location = new Point(X, Y);

            gamePanel.Controls.Add(over);

        }
    }
}
