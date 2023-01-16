using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Snake Speed Game 
//January 2023
//Jasmine Josan
namespace SnakeSpeed
{
    public partial class Form1 : Form
    {
        Rectangle player = new Rectangle(230, 170, 15, 15);
        Rectangle obstacle1 = new Rectangle(130, 80, 200, 25);
        Rectangle obstacle2 = new Rectangle(240, 320, 200, 25);
        Rectangle ball = new Rectangle(70, 195, 95, 95);
        //Rectangle point = new Rectangle(200, 195, 10, 10);

        List<Rectangle> points = new List<Rectangle>();
        List<String> pointColours = new List<string>();
        List<int> pointSpeeds = new List<int>();

        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush limeBrush = new SolidBrush(Color.LimeGreen);
        SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
        SolidBrush cyanBrush = new SolidBrush(Color.Cyan);


        bool upArrowDown = false;
        bool downArrowDown = false;
        bool leftDown = false;
        bool rightDown = false;

        int playerScore = 0;
        int playerSpeed = 3;

        int pointSize = 10;

        string gameState = "waiting";

        Random randGen = new Random();
        int randValue = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        //GameSetup();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        this.Close();
                    }
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw player
            e.Graphics.FillRectangle(cyanBrush, player);

            //draw obstacles
            e.Graphics.FillRectangle(redBrush, obstacle1);
            e.Graphics.FillRectangle(redBrush, obstacle2);
            e.Graphics.FillEllipse(limeBrush, ball);

            //draw points
            for (int i = 0; i < points.Count(); i++)
            {
                e.Graphics.FillEllipse(yellowBrush, points[i]);

            }
        }


        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player
            if (upArrowDown == true && player.Y > 0)
            {
                player.Y -= playerSpeed;
            }

            if (downArrowDown == true && player.Y < 400)
            {
                player.Y += playerSpeed;
            }
            if (rightDown == true && player.X < 395)
            {
                player.X += playerSpeed;
            }
            if (leftDown == true && player.X > 2)
            {
                player.X -= playerSpeed;
            }

            //generate a random value
            randValue = randGen.Next(1, 101);

            //generate new point if it is time
            if (randValue < 3)
            {
                points.Add(new Rectangle(randGen.Next(0, this.Width - pointSize), randGen.Next(0, this.Height - pointSize), pointSize, pointSize));
                pointSpeeds.Add(10);
            }

            //check for collision between player and obstacles
            for (int i = 0; i < points.Count; i++)
            {
                if (player.IntersectsWith(points[i]))
                {
                    playerScore++;
                    playerScoreLabel.Text = $"Score: {playerScore}";

                    points.RemoveAt(i);
                    pointSpeeds.RemoveAt(i);
                }
            }

            //remove points if it they go off screen
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y >= this.Height)
                {
                    points.RemoveAt(i);
                    pointSpeeds.RemoveAt(i);
                }
            }

            Refresh();

        }
    }
}

