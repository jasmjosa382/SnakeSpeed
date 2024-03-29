﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

//Snake Speed Game 
//January 2023
//Jasmine Josan
namespace SnakeSpeed
{
    public partial class Form1 : Form
    {
        Rectangle player = new Rectangle(230, 170, 21, 21);
        Rectangle obstacle1 = new Rectangle(130, 80, 200, 25);
        Rectangle obstacle2 = new Rectangle(285, 320, 200, 25);
        Rectangle ball = new Rectangle(80, 195, 95, 95);

        List<Rectangle> points = new List<Rectangle>();
        List<String> pointColours = new List<string>();

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

        int obstacleXSpeed = 3;

        int pointSize = 10;

        string gameState = "waiting";

        Random randGen = new Random();
        int randValue = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void GameSetup()
        {
            gameState = "running";

            titleLabel.Visible = false;
            playerScoreLabel.Visible = true;
            titleLabel.Text = "";
            subtitleLabel.Text = "";
            subtitle2Label.Text = "";

            gameTimer.Enabled = true;
            playerScore = 0;
            playerSpeed = 3;
            player.X = 230;
            player.Y = 170;

            points.Clear();
            pointColours.Clear();
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
                        GameSetup();
                        SoundPlayer startGame = new SoundPlayer(Properties.Resources.startGame);
                        startGame.Play();
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

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move obstacle 1
            obstacle1.X += obstacleXSpeed;

            //move player
            if (upArrowDown == true && player.Y > 0)
            {
                player.Y -= playerSpeed;

                if (player.Y < 0)
                {
                    player.Y = 0;
                }
            }

            if (downArrowDown == true && player.Y < 410)
            {
                player.Y += playerSpeed;
              
                if (player.Y > 410)
                {
                    player.Y = 410;
                }
            }
            if (rightDown == true && player.X < 461)
            {
                player.X += playerSpeed;

                if (player.X > 461)
                {
                    player.X = 461;
                }
            }
            if (leftDown == true && player.X > 2)
            {
                player.X -= playerSpeed;

                if (player.X < 2)
                {
                    player.X = 2;
                }
            }

            //generate a random value
            randValue = randGen.Next(1, 101);

            //generate new point if it is time
            if (randValue < 4)
            {
                Rectangle newPoint = new Rectangle(randGen.Next(0, this.Width - pointSize), randGen.Next(0, this.Height - pointSize), pointSize, pointSize);

                //points interact with obstacles

                while (newPoint.IntersectsWith(obstacle2) || newPoint.IntersectsWith(ball))
                {
                    newPoint = new Rectangle(randGen.Next(0, this.Width - pointSize), randGen.Next(0, this.Height - pointSize), pointSize, pointSize);

                }

                points.Add(newPoint);
            }


            //check for collision between player and points
            for (int i = 0; i < points.Count; i++)
            {
                if (player.IntersectsWith(points[i]))
                {
                    SoundPlayer pointUp = new SoundPlayer(Properties.Resources.pointUp);
                    pointUp.Play();

                    playerSpeed++;

                    playerScore++;
                    playerScoreLabel.Text = $"Score: {playerScore}";

                    points.RemoveAt(i);
                }
            }


            //remove points if it they go off screen
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y >= 410 || points[i].X >= 455)
                {
                    points.RemoveAt(i);

                }
            }


            //player intersects with obstacles
            if (player.IntersectsWith(obstacle1) || player.IntersectsWith(obstacle2) || player.IntersectsWith(ball))
            {
                SoundPlayer gameOver = new SoundPlayer(Properties.Resources.gameOver);
                gameOver.Play();

                gameTimer.Enabled = false;

                gameState = "over";

            }

            //change direction if obstacle 1 hits the wall 
            if (obstacle1.X < 0 || obstacle1.X > 280)
            {
                obstacleXSpeed *= -1;
            }




            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameState == "waiting")
            {
                titleLabel.Text = "Snake Speed";
                subtitleLabel.Text = "Avoid the obstacles and reach a new high speed!";
                subtitle2Label.Text = "Press Space to play or Esc to exit";
            }
            else if (gameState == "running")
            {
                // update labels
                playerScoreLabel.Text = $"Score: {playerScore}";

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
            else if (gameState == "over")
            {
                playerScoreLabel.Text = "";

                titleLabel.Visible = true;

                titleLabel.Text = "Game Over";
                subtitleLabel.Text = $"High Speed: {playerScore}";
                subtitle2Label.Text = "Press Space to Start or Esc to Exit";
            }
        }

    }
}

