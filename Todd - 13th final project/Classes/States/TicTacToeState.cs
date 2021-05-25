using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Todd.Sprites;

namespace Todd.States
{
    class TicTacToeState : State
    {
        #region data
        string text = "";
        string player = "O";
        string bot = "X";
        bool isPlayerTurn = true;

        Dictionary<int, string> board = new Dictionary<int, string>()
        {
            { 1, " " },{ 2, " " },{ 3, " " }
            ,{ 4, " " },{ 5, " " },{ 6, " " }
            ,{ 7, " " },{ 8, " " },{ 9, " " }
        };
        Texture2D background;

        List<Button> btnlst;


        #endregion


        #region constructor
        public TicTacToeState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) : base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load backgroound
            background = _content.Load<Texture2D>("TicTacToe/TicTacToeBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle the buttons
            var p1 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(47, 160),
                Color.Black, Color.White, btnTextFont, "", 100);
            p1.Click += p1Click;

            var p2 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(147, 160),
                Color.Black, Color.White, btnTextFont, "", 100);
            p2.Click += p2Click;

            var p3 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(247, 160),
                Color.Black, Color.White, btnTextFont, "", 100);
            p3.Click += p3Click;

            var p4 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(47, 260),
                Color.Black, Color.White, btnTextFont, "", 100);
            p4.Click += p4Click;

            var p5 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(147, 260),
                Color.Black, Color.White, btnTextFont, "", 100);
            p5.Click += p5Click;

            var p6 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(247, 260),
                Color.Black, Color.White, btnTextFont, "", 100);
            p6.Click += p6Click;

            var p7 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(47, 360),
                Color.Black, Color.White, btnTextFont, "", 100);
            p7.Click += p7Click;

            var p8 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(147, 360),
                Color.Black, Color.White, btnTextFont, "", 100);
            p8.Click += p8Click;

            var p9 = new Button(_content.Load<Texture2D>("TicTacToe/Empty"), new Vector2(247, 360),
                Color.Black, Color.White, btnTextFont, "", 100);
            p9.Click += p9Click;

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;


            btnlst = new List<Button>()
            {
                p1,p2,p3,p4,p5,p6,p7,p8,p9, backButton
            };
            
        }




        #endregion


        #region update postupdate and draw

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);

            foreach (Button btn in btnlst)
                btn.Draw(spritebatch);
            spritebatch.DrawString(_content.Load<SpriteFont>("Fonts/font"), text, new Vector2(50, 50), Color.Red);
            spritebatch.End();

        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            if (isGameFinished())
            {
                Thread.Sleep(2500);
                clearBoard();
                text = "";
            }


            if (isPlayerTurn == true)
                foreach (Button btn in btnlst)
                {
                    btn.Update(gametime);
                }
            else
            {
                computerMove();
                isPlayerTurn = true;
            }


            for (int key = 1; key < 10; key++)
            {
                if(board[key]==bot)
                    btnlst[key - 1].texture = _content.Load<Texture2D>("TicTacToe/X");
                else if(board[key] == player)
                    btnlst[key - 1].texture = _content.Load<Texture2D>("TicTacToe/O");
                else
                    btnlst[key - 1].texture = _content.Load<Texture2D>("TicTacToe/Empty");

            }


        }


        #endregion

        #region other func

        bool spaceIsFree(int pos)
        {
            if (board[pos] == " ")
                return true;
            return false;
        }

        void insertLetter(string letter, int pos)
        {
            if (spaceIsFree(pos))
            {
                text = "";
                board[pos] = letter;
                if (checkDraw())
                    text = "Draw...";
                else if (checkForWin())
                {
                    if (letter == bot)
                        text = "Bot Won!";
                    else if (letter == player)
                        text = "Player Won!";
                }
            }
            else
                text = "invalid input !!!!!!";

        }

        bool checkForWin()
        {
            //check rows
            for (int i = 1; i < 10; i += 3)
                if (board[i] == board[i + 1] && board[i] == board[i + 2] && board[i] != " ")
                    return true;
            //check cols
            for (int i = 1; i < 4; i ++)
                if (board[i] == board[i + 3] && board[i] == board[i + 6] && board[i] != " ")
                    return true;
            //check alachsons
            if (board[1] == board[5] && board[1] == board[9] && board[1] != " ")
                return true;
            else if (board[7] == board[5] && board[7] == board[3] && board[7] != " ")
                return true;

            return false;
        }

        private bool checkDraw()
        {
            for (int i = 1; i < 10; i++)
                if (board[i] == " ")
                    return false;
            return true;
        }

        bool checkWhichMarkWon(string mark)
        {
            if (board[1] == board[2] && board[1] == board[3] && board[1] == mark)
                return true;
           else if(board[4] == board[5] && board[4] == board[6] && board[4] == mark)
                return true;
            else if (board[7] == board[8] && board[7] == board[9] && board[7] == mark)
                return true;
            else if (board[1] == board[4] && board[1] == board[7] && board[1] == mark)
                return true;
            else if (board[2] == board[5] && board[2] == board[8] && board[2] == mark)
                return true;
            else if (board[3] == board[6] && board[3] == board[9] && board[3] == mark)
                return true;
            else if (board[1] == board[5] && board[1] == board[9] && board[1] == mark)
                return true;
            else if (board[7] == board[5] && board[7] == board[3] && board[7] == mark)
                return true;
            return false;

        }

        void clearBoard()
        {
            board = new Dictionary<int, string>()
            {
                { 1, " " },{ 2, " " },{ 3, " " }
                ,{ 4, " " },{ 5, " " },{ 6, " " }
                ,{ 7, " " },{ 8, " " },{ 9, " " }
            };
        }

        bool isGameFinished()
        {
            if (text == "Draw..." || text == "Bot Won!" || text == "Player Won!")
                return true;
            return false;

        }

        int minimax(int depth, bool isMaximizing)
        {
            if (checkWhichMarkWon(bot))
                return 1;
            else if (checkWhichMarkWon(player))
                return -1;
            else if (checkDraw())
                return 0;
            if (isMaximizing)
            {
                int bestScore = -800;
                for (int key = 1; key < 10; key++)
                {
                    if (board[key] == " ")
                    {
                        board[key] = bot;
                        int score = minimax(depth + 1, false);
                        board[key] = " ";
                        if (score > bestScore)
                            bestScore = score;

                    }
                }
                return bestScore;
            }

            else
            {
                int bestScore = 800;
                for (int key = 1; key < 10; key++)
                {
                    if (board[key] == " ")
                    {
                        board[key] = player;
                        int score = minimax(depth + 1, true);
                        board[key] = " ";
                        if (score < bestScore)
                            bestScore = score;

                    }
                }
                return bestScore;
            }
        }

        void computerMove()
        {
            int bestScore = -800;
            int bestMove = 0;
            for (int key = 1; key < 10; key++)
            {
                if (board[key] == " ")
                {
                    board[key] = bot;
                    int score = minimax(0, false);
                    board[key] = " ";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = key;

                    }
                }
            }
            insertLetter(bot, bestMove);
        }
        #endregion

        #region btn click actions

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new miniGameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }//back to main menu


        private void p9Click(object sender, EventArgs e)
        {
            insertLetter(player, 9);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p8Click(object sender, EventArgs e)
        {
            insertLetter(player, 8);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p7Click(object sender, EventArgs e)
        {
            insertLetter(player, 7);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p6Click(object sender, EventArgs e)
        {
            insertLetter(player, 6);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p5Click(object sender, EventArgs e)
        {
            insertLetter(player, 5);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p4Click(object sender, EventArgs e)
        {
            insertLetter(player, 4);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p3Click(object sender, EventArgs e)
        {
            insertLetter(player, 3);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p2Click(object sender, EventArgs e)
        {
            insertLetter(player, 2);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }

        private void p1Click(object sender, EventArgs e)
        {
            insertLetter(player, 1);
            if (text != "invalid input !!!!!!")
                isPlayerTurn = false;
        }


        #endregion


    }
}
