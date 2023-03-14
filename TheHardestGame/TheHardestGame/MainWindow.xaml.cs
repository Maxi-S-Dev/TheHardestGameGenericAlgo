using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TheHardestGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer DP = new DispatcherTimer();
        DispatcherTimer MovementTimer;

        int tickSpeed = 16;

        float speedY, speedX;
        float speed = 2;
        bool up = false, down = false, left = false, right = false;


        float enemySpeed = 12;
        float firstSpeed;
        float secondSpeed;
        float thirdSpeed;
        float fourthSpeed;

        public MainWindow()
        {
            InitializeComponent();
            Canvas.Focus();

            firstSpeed = secondSpeed = thirdSpeed = fourthSpeed = enemySpeed;


            MovementTimer = new DispatcherTimer(DispatcherPriority.Send);

            MovementTimer.Interval = TimeSpan.FromMilliseconds(10);
            MovementTimer.Tick += Movement;
           

            DP.Interval = TimeSpan.FromMilliseconds(tickSpeed);
            DP.Tick += GameTick;
            DP.Start();
        }

        void GameTick(object? sender, EventArgs e)
        {
            MoveEnemys();
            CheckWin();
        }

        private void CheckWin()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
            Rect winHB = new Rect(Canvas.GetLeft(WinZone), Canvas.GetTop(WinZone), WinZone.Width, WinZone.Height);

            if (PlayerHB.IntersectsWith(winHB)) WinGame();
        }

        private void WinGame()
        {
            ResultLabel.Content = "wow you actually won \n Press 'r' if you hate yourself";
            DP.Stop();
        }

        private void MoveEnemys()
        {
            Collide();

            Canvas.SetLeft(FirstRow, Canvas.GetLeft(FirstRow) + firstSpeed * -0.88f);
            if (Canvas.GetLeft(FirstRow) < 212) firstSpeed *= -1;
            if (Canvas.GetLeft(FirstRow) > 572) firstSpeed *= -1;

            Canvas.SetLeft(SecondRow, Canvas.GetLeft(SecondRow) + secondSpeed * 0.88f);
            if (Canvas.GetLeft(SecondRow) < 212) secondSpeed *= -1;
            if (Canvas.GetLeft(SecondRow) > 572) secondSpeed *= -1;

            Canvas.SetLeft(ThirdRow, Canvas.GetLeft(ThirdRow) + thirdSpeed * -0.88f);
            if (Canvas.GetLeft(ThirdRow) < 212) thirdSpeed *= -1;
            if (Canvas.GetLeft(ThirdRow) > 572) thirdSpeed *= -1;

            Canvas.SetLeft(FourthRow, Canvas.GetLeft(FourthRow) + fourthSpeed * 0.88f);
            if (Canvas.GetLeft(FourthRow) < 212) fourthSpeed *= -1;
            if (Canvas.GetLeft(FourthRow) > 572) fourthSpeed *= -1;
        }

        private void Movement(object? sender, EventArgs e)
        {
            //speedX = speedY = 0;

            if (up) speedY -= speed;
            if (down) speedY += speed;

            //Trace.WriteLine(left + " " + speedX);
            if (left) speedX -= speed;
            if (right) speedX += speed;

            speedX = speedX * 0.5f;
            speedY = speedY * 0.5f;

            
            Canvas.SetLeft(Player, Canvas.GetLeft(Player) + speedX);
            CollideX();

            Canvas.SetTop(Player, Canvas.GetTop(Player) + speedY);
            CollideY();
        }

        private void Collide()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            foreach(var x in Canvas.Children.OfType<Ellipse>())
            {
                if((string)x.Tag == "enemy")
                {                   
                    Rect collisionHB = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (PlayerHB.IntersectsWith(collisionHB))
                    {
                        GameOver();
                        return;
                    }
                }
            }
        }

        private void GameOver()
        {
            DP.Stop();
            ResultLabel.Content = "LOL Looser \n click 'R' to restart";
            
        }
        private void CollideX()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            foreach(var x in Canvas.Children.OfType<Rectangle>()) 
            {
                if ((string)x.Tag == "collider")
                {
                    Rect colliderHB = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (PlayerHB.IntersectsWith(colliderHB))
                    {
                        Canvas.SetLeft(Player, Canvas.GetLeft(Player) - speedX);
                        speedX = 0;
                        return;
                    }
                }
            }
        }


        private void CollideY()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);

            foreach (var x in Canvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "collider")
                {
                    Rect colliderHB = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (PlayerHB.IntersectsWith(colliderHB))
                    {
                        Canvas.SetTop(Player, Canvas.GetTop(Player) - speedY);
                        speedY = 0;
                        return;
                    }
                }
            }
        }

        private void ButtonPressed(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) return;

            switch (e.Key)
            {
                case Key.W: 
                    up = true;
                    break;
                
                case Key.S: 
                    down = true;
                    break;

                case Key.D:
                    right = true;
                    break;

                case Key.A:
                    left = true;
                    break;

                case Key.R:
                    resetGame();
                    break;
            }
            MovementTimer.Start();
            Movement(sender, e);
        }

        private void resetGame()
        {
            DP.Stop();
            Canvas.SetLeft(Player, 87); Canvas.SetTop(Player, 209);
            Canvas.SetLeft(FirstRow, 572); Canvas.SetTop(FirstRow, 149);
            Canvas.SetLeft(SecondRow, 212); Canvas.SetTop(SecondRow, 189);
            Canvas.SetLeft(ThirdRow, 572); Canvas.SetTop(ThirdRow, 229);
            Canvas.SetLeft(FourthRow, 212); Canvas.SetTop(FourthRow, 269);

            DP.Start();
        }

        private void ButtonReleased(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    up = false;
                    break;

                case Key.S:
                    down = false;
                    break;

                case Key.D:
                    right = false;
                    break;

                case Key.A:
                    left = false;
                    break;
            }

            if(!(left || right || up || down)) MovementTimer.Stop();
        }
    }
}
