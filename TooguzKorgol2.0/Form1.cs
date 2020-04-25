using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace TooguzKorgol2._0
{
    public partial class Form1 : Form
    {
        public enum Player
        {
            A,
            B
        }

        //array to store min and sec
        private int[] times;

        private Timer timer_1;
        private Timer timer_2;


        private bool tuzPlayer1 = true;
        private bool tuzPlayer2 = true;

        Player currentplayer = Player.A;

        int currentIndex;

        List<Button> kazans;


        int player1Point = 0;
        int player2Point = 0;

        public Form1()
        {
            InitializeComponent();

            times = new int[4];
            timer1 = new Timer();
            timer1.Interval = 1000;
            timer2 = new Timer();
            timer2.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer2.Tick += new EventHandler(timer2_Tick);
            resetGame();
        }

        private void onClickKoorgol(object sender, EventArgs e)
        {
            var currentButton = (Button) sender;
            if (!currentButton.Text.Equals("0") && !currentButton.Text.Equals("ТУЗ"))
            {
                int num = moving(currentButton);
                currentButton.Text = "" + num;
                Check(kazans[currentIndex]);
                switchPlayer();
                activatePlayer(currentplayer);
            }
        }

        private void loadKazan()
        {
            kazans = new List<Button>
            {
                button1, button2, button3,
                button4, button5, button6,
                button7, button8, button9,
                button10, button11, button12,
                button13, button14, button15,
                button16, button17, button18
            };
        }

        private void resetGame()
        {
            Stop_Timers();
            loadKazan();
            for (var i = 0; i < times.Length; i++)
            {
                times[i] = 0;
            }

            label_timer1.Text = "00:00";
            label_timer2.Text = "00:00";
            foreach (var button in kazans)
            {
                button.Text = "9";
                button.BackColor = DefaultBackColor;
            }

            //debagging 
            /*for (int i = 0; i < 9; i++)
            {
                kazans[i].Text = "9";
                kazans[i].BackColor = DefaultBackColor;
            }

            for (int i = 9; i < 18; i++)
            {
                kazans[i].Text = "9";
                kazans[i].BackColor = DefaultBackColor;
            }*/

            tuzPlayer1 = true;
            tuzPlayer2 = true;

            player1Point = 0;
            player2Point = 0;

            score1.Text = "" + player1Point;
            score2.Text = "" + player2Point;
            CheckWhoWin();

            activatePlayer(currentplayer);
        }

        private void activatePlayer(Player x)
        {
            if (x == Player.A)
            {
                for (int i = 0; i < 9; i++)
                {
                    kazans[i].Enabled = true;
                }

                for (int i = 9; i < 18; i++)
                {
                    kazans[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 9; i < 18; i++)
                {
                    kazans[i].Enabled = true;
                }

                for (int i = 0; i < 9; i++)
                {
                    kazans[i].Enabled = false;
                }
            }
        }

        void CheckWhoWin()
        {
            if (Convert.ToInt16(score2.Text) == 81 & Convert.ToInt16(score1.Text) == 81)
            {
                string message = "Ничья";
                show_message_end(message);
            }

            if (Convert.ToInt16(score2.Text) > 81)
            {
                string message = "Победил верхний игрок";
                show_message_end(message);
            }

            if (Convert.ToInt16(score1.Text) > 81)
            {
                string message = "Победил нижний игрок";
                show_message_end(message);
            }
        }

        void show_message_end(string message)
        {
            Stop_Timers();
            DialogResult result = MessageBox.Show(message + "! Вы хотите начать новую игру?", "Тогуз коргоол",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                resetGame();
            }

            if (result == DialogResult.No)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        void Stop_Timers()
        {
            timer1.Stop();
            timer2.Stop();
        }

        void CheckTuz(int btn, Button button)
        {
            if (currentplayer == Player.A && !button.Enabled && currentIndex != 8)
            {
                int index = (currentIndex + 9) % 18;
                if (tuzPlayer1 && !kazans[index].Text.Equals("ТУЗ"))
                {
                    player1Point += btn;
                    score1.Text = "" + player1Point;
                    button.Text = "ТУЗ";
                    button.BackColor = Color.Red;
                    tuzPlayer1 = false;
                }
            }

            if (currentplayer == Player.B && !button.Enabled && currentIndex != 17)
            {
                int index = (currentIndex + 9) % 18;
                if (tuzPlayer2 && !kazans[index].Text.Equals("ТУЗ"))
                {
                    player2Point += btn;
                    score2.Text = "" + player2Point;
                    button.Text = "ТУЗ";
                    button.BackColor = Color.Red;
                    tuzPlayer2 = false;
                }
            }
        }

        private void Check(Button button)
        {
            if (button.Text.Equals("ТУЗ"))
            {
                if (currentplayer == Player.A)
                {
                    player1Point += 1;
                    score1.Text = "" + player1Point;
                }
                else
                {
                    player2Point += 1;
                    score2.Text = "" + player2Point;
                }
            }
            else
            {
                int btn = Convert.ToInt32(button.Text);
                if (btn == 3)
                {
                    CheckTuz(btn, button);
                }


                if (currentplayer == Player.A && btn % 2 == 0 && !button.Enabled)
                {
                    player1Point += btn;
                    score1.Text = "" + player1Point;
                    button.Text = "0";
                }

                if (currentplayer == Player.B && btn % 2 == 0 && !button.Enabled)
                {
                    player2Point += btn;
                    score2.Text = "" + player2Point;
                    button.Text = "0";
                }
            }


            CheckWhoWin();
        }

        void switchPlayer()
        {
            if (currentplayer == Player.A)
            {
                currentplayer = Player.B;
            }
            else
            {
                currentplayer = Player.A;
            }
        }

        private void restartGame(object sender, EventArgs e)
        {
            resetGame();
        }

        private int moving(Button btn)
        {
            if (currentplayer == Player.A)
            {
                timer2.Start();
                timer1.Stop();
            }
            else
            {
                timer1.Start();
                timer2.Stop();
            }

            foreach (var button in kazans)
            {
                if (button.Text.Equals("ТУЗ")) continue;
                button.BackColor = DefaultBackColor;
            }

            int c = Convert.ToInt32(btn.Text);

            currentIndex = kazans.IndexOf(btn);

            int index = -1;

            if (c <= 1)
            {
                currentIndex = (currentIndex + 1) % 18;
                if (!kazans[currentIndex].Text.Equals("ТУЗ"))
                {
                    kazans[currentIndex].Text = Convert.ToString(Convert.ToInt32(kazans[currentIndex].Text) + 1);
                }

                index = 0;
            }
            else
            {
                kazans[currentIndex].BackColor = Color.Orchid;
                for (int i = c - 1; i > 0; i--)
                {
                    currentIndex = (currentIndex + 1) % 18;
                    if (!kazans[currentIndex].Text.Equals("ТУЗ"))
                    {
                        kazans[currentIndex].Text =
                            Convert.ToString(Convert.ToInt32(kazans[currentIndex].Text) + 1);
                    }

                    if (kazans[currentIndex].Text == "ТУЗ") continue;
                    kazans[currentIndex].BackColor = Color.Chartreuse;
                    index = i;
                    Thread.Sleep(500);
                    Update();
                }
                init_Color();
            }


            return index;
        }

        private void init_Color()
        {
            foreach (var button in kazans)
            {
                button.BackColor = DefaultBackColor;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            times[1] += 1;

            int min = times[0];
            int sec = times[1];

            string time = "";
            if (sec >= 60)
            {
                times[0] = (times[0] + 1) % 60;
                times[1] = 0;
            }

            if (min < 10)
            {
                time += "0" + min;
            }
            else
            {
                time += "" + min;
            }

            time += ":";
            if (sec < 10)
            {
                time += "0" + sec;
            }
            else
            {
                time += "" + sec;
            }

            //update label
            label_timer1.Text = time;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            times[3] += 1;

            int min = times[2];
            int sec = times[3];

            string time = "";

            if (sec >= 60)
            {
                times[2] = (times[2] + 1) % 60;
                times[3] = 0;
            }

            if (min < 10)
            {
                time += "0" + min;
            }
            else
            {
                time += "" + min;
            }

            time += ":";
            if (sec < 10)
            {
                time += "0" + sec;
            }
            else
            {
                time += "" + sec;
            }

            //update label
            label_timer2.Text = time;
        }
    }
}