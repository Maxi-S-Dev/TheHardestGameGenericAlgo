using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
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

        int tickSpeed = 32;

        float playerSpeedX, playerSpeedY;

        bool up = false, down = false, left = false, right = false;


        float enemySpeed = 12, firstSpeed, secondSpeed, thirdSpeed, fourthSpeed;

        List<Chromosome> playerRects = new List<Chromosome>();

        int interval = 8;

        public MainWindow()
        {
            InitializeComponent();
            Canvas.Focus();

            for (int i = 0; i < 16; i++)
            {
                List<MoveDirections> list = getDirectionPattern(i);

                Trace.WriteLine($"{list[0]} {list[1]}");
            }

            CreateCromosomes();

            firstSpeed = secondSpeed = thirdSpeed = fourthSpeed = enemySpeed;

            DP.Tick += GameTick;
            //DP.Start();
        }

        void GameTick(object? sender, EventArgs e)
        {
            Movement();
            MoveEnemys();
            CheckWin();
        }

        private void CreateCromosomes()
        {
            for (int i = 0; i < 16; i++)
            {
                Rectangle r = new Rectangle();
                Chromosome chromosome = new();

                chromosome.mesh = r;

                Canvas.Children.Add(r);
                r.Width = 26; r.Height = 26;
                r.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)); r.StrokeThickness = 3;
                r.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));

                Canvas.SetLeft(r, 87); Canvas.SetTop(r, 209);

                playerRects.Add(chromosome);
            }
        }


        private List<MoveDirections> getDirectionPattern (int variation)
        {
            List<MoveDirections> list = new();

            switch(variation / 4)
            {
                case 0:
                    list.Add(MoveDirections.up);
                    break;

                case 1:
                    list.Add(MoveDirections.down);
                    break;

                case 2:
                    list.Add(MoveDirections.left);
                    break;

               case 3:
                    list.Add(MoveDirections.right);
                    break;
            }

            switch(variation % 4)
            {
                case 0:
                    list.Add(MoveDirections.up);
                    break;

                case 1:
                    list.Add(MoveDirections.down);
                    break;

                case 2:
                    list.Add(MoveDirections.left);
                    break;

                case 3:
                    list.Add(MoveDirections.right);
                    break;
            }



            return list;
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


        private void CheckWin()
        {
            Rect PlayerHB = new Rect(Canvas.GetLeft(Player), Canvas.GetTop(Player), Player.Width, Player.Height);
            Rect winHB = new Rect(Canvas.GetLeft(WinZone), Canvas.GetTop(WinZone), WinZone.Width, WinZone.Height);

            if (PlayerHB.IntersectsWith(winHB)) WinGame();
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
                        Canvas.SetLeft(Player, Canvas.GetLeft(Player) - playerSpeedX);
                        playerSpeedX = 0;
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
                        Canvas.SetTop(Player, Canvas.GetTop(Player) - playerSpeedY);
                        playerSpeedY = 0;
                        return;
                    }
                }
            }
        }


        private void WinGame()
        {
            ResultLabel.Content = "wow you actually won \n Press 'r' if you hate yourself";
            DP.Stop();
        }

        private void GameOver()
        {
            DP.Stop();
            ResultLabel.Content = "LOL Looser \n click 'R' to restart";

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
    }
}




///TODO
///Spawn 50 rectangles
///Add AI Logic (good things, bad things)
///Connect AI with rects
