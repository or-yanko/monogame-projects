using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Todd.Camera;
using Todd.Managers;
using Todd.Moduls;
using Todd.Sprites;

namespace Todd.States
{
    public class GameState : State
    {
        #region data
        string level;
        SpriteFont font;
        float time = 0f;
        Sprite hero;
        List<Platform> platforms;
        List<Bomb> bombsList;
        Texture2D background;
        Camera_ camera;
        List<DrawablObject> objects;
        List<Enemy> enemies;
        List<Coin> coins;
        DrawablObject star;
        CoinsAndHeartsManager coinsAndHeartsManager;
        GameTime gt;

        #endregion

        #region constructor
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds, List<Enemy> enemies,
            List<Platform> platforms, List<DrawablObject> objects, List<Coin> coins, DrawablObject star, int level) : base(game, graphicsDevice, content, graphics_, sounds)
        {
            this.level = level.ToString();
            int h = CoinsAndHeartsManager.Load().curr.hearts;
            coinsAndHeartsManager = new CoinsAndHeartsManager(new CoinsAndHearts(0, 0, 0, h));

            //coins
            this.coins = coins;

            //font
            font = _content.Load<SpriteFont>("Fonts/font");

            //camera staff
            camera = new Camera_();

            //background
            background = _content.Load<Texture2D>("BackGrounds/Background800x480");

            //hero
            hero = new Sprite(SetHeroAnimation(), 15f)
            {
                Position = new Vector2(100, 100),
                Input = new Input()
                {
                    Up = Keys.Up,
                    Left = Keys.Left,
                    Right = Keys.Right,
                },
                hasJumped = true
            };

            //bombs
            bombsList = new List<Bomb>();

            //enemies platforms and objects
            this.enemies = enemies;
            this.platforms = platforms;
            this.objects = objects;

            //finishing star
            this.star = star;

        }

        #endregion

        #region draw update and postupdate 
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            //draw static backgound
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            coinsAndHeartsManager.Draw(spritebatch, _content);
            spritebatch.DrawString(font, time.ToString(), new Vector2(600, 5), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
            spritebatch.End();


            //draw game
            spritebatch.Begin(transformMatrix: camera.transform);

            foreach (Platform platform in platforms)
                platform.Draw(spritebatch);
            foreach (DrawablObject d in objects)
                d.Draw(spritebatch);
            foreach (Bomb bomb in bombsList)
                bomb.Draw(spritebatch);
            foreach (Coin coin in coins)
                coin.Draw(spritebatch);
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spritebatch);
                drawRectangle(spritebatch, enemy.rectangle, _content);
            }
            star.Draw(spritebatch);


            hero.Draw(spritebatch);
            drawRectangle(spritebatch, hero.rectangle, _content);

            spritebatch.End();
        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            gt = gametime;
            //time
            time += (float)gametime.ElapsedGameTime.TotalSeconds;

            star.Update(gametime);
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gametime, hero);
            }

            Enemy removedEnemy = null;
            Coin removedCoin = null;

            //gravity
            hero.hasJumped = true;
            foreach (Platform platform in platforms)
            {
                if (hero.rectangle.isOnTopOf(platform.rectangle))
                {
                    hero.velocity.Y = 0f;
                    hero.hasJumped = false;
                }
            }
            //colisions
            foreach (Enemy enemy in enemies)
            {
                if (hero.rectangle.isOnTopOf(enemy.rectangle))
                {
                    enemy.isPlayerDead = true;
                    hero.velocity.Y = -5;
                    removedEnemy = enemy;
                }
                else if (hero.rectangle.Intersects(enemy.rectangle))
                    hero.isPlayerDead = true;
                else if(enemy.isShooting == true)
                {
                    Vector2 heroMidPixPos = new Vector2((hero.rectangle.Left + hero.rectangle.Right) / 2, (hero.rectangle.Top + hero.rectangle.Bottom) / 2);
                    Vector2 enemyMidPixPos = new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2);
                    int distancePow = (int)((heroMidPixPos.X - enemyMidPixPos.X) * (heroMidPixPos.X - enemyMidPixPos.X) + (heroMidPixPos.Y - enemyMidPixPos.Y) * (heroMidPixPos.Y - enemyMidPixPos.Y));
                    if(enemy.rangeOfSight * enemy.rangeOfSight > distancePow)
                    {
                        if (enemy.timer > 0.75f)
                        {
                            enemy.timer %= 0.75f;
                            Random rand = new Random();
                            int num = rand.Next(0, 20);
                            num %= 14;
                            if (num == 0)
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/rock"), this._content.Load<Texture2D>("Bomb/rock"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });
                            else if (num < 2)
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/pinkBalloon"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });
                            else if (num < 3)
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/redBaloon"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });
                            else if (num < 4)
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/yellowBaloon"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });
                            else if (num < 7)
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/dynamite"), this._content.Load<Texture2D>("Bomb/exploadedBomb"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });
                            else if (num < 10)
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/bomb"), this._content.Load<Texture2D>("Bomb/exploadedBomb"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });
                            else
                                bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/molotov"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((enemy.rectangle.Left + enemy.rectangle.Right) / 2, (enemy.rectangle.Top + enemy.rectangle.Bottom) / 2), true) { isTaken = 1 });

                        }


                    }
                }
            }
            
            if (removedEnemy != null)
                enemies.Remove(removedEnemy);

            foreach (Coin coin in coins)
            {
                coin.Update(gametime);
                if (hero.rectangle.Intersects(coin.rectangle))
                {
                    coin.isTaken = true;
                    removedCoin = coin;
                    if (coin.type == "Gold")
                        coinsAndHeartsManager.curr.goldCoins += 1;
                    else if (coin.type == "Silver")
                        coinsAndHeartsManager.curr.silverCoins += 1;
                    else if (coin.type == "Bronze")
                        coinsAndHeartsManager.curr.bronzeCoins += 1;

                }
            }
            if (removedCoin != null)
                coins.Remove(removedCoin);

            //is touch bombs
            Bomb del = new Bomb(false, null, null, new Vector2(-100, -100));
            foreach (Bomb bomb in bombsList)
            {
                bomb.Update(gametime, hero);
                if (hero.rectangle.Intersects(bomb.rectangle))
                    hero.isPlayerDead = true;
                if (bomb.isDead == true)
                    del = bomb;
            }
            //del bombs
            if (del._position.X != -100 && del._position.Y != -100)
                bombsList.Remove(del);
            bombsList.Remove(del);


            if (hero.rectangle.Intersects(star.rectangle))
                winAction();

            hero.Update(gametime);


            try
            {
                camera.Follow(hero);
                if (hero.isPlayerDead == true)//player.is deed
                {
                    heroDiedAction();
                }
            }
            catch
            { }


        }



        #endregion


        #region other functions

        #region winnig losing functions
        private void heroDiedAction()
        {
            if(coinsAndHeartsManager.curr.hearts!= 0)
            {
                coinsAndHeartsManager.curr.hearts -= 1;
                hero.isPlayerDead = false;
                hero.Position = hero._position = new Vector2(hero._position.X, 0);
                hero.Update(gt);

                return;
            }
            CoinsAndHearts cah = new CoinsAndHearts(coinsAndHeartsManager.curr.goldCoins, coinsAndHeartsManager.curr.silverCoins, coinsAndHeartsManager.curr.bronzeCoins, CoinsAndHeartsManager.Load().curr.hearts - coinsAndHeartsManager.curr.hearts);
            _game.ChangeState(new PrizesState(_game, _graphicsDevice, _content, graphics, _soundsDict, cah, 1000000, level));

        }
        private void winAction()
        {
            CoinsAndHearts cah = new CoinsAndHearts(coinsAndHeartsManager.curr.goldCoins, coinsAndHeartsManager.curr.silverCoins, coinsAndHeartsManager.curr.bronzeCoins,  0 - (CoinsAndHeartsManager.Load().curr.hearts - coinsAndHeartsManager.curr.hearts));
            _game.ChangeState(new PrizesState(_game, _graphicsDevice, _content, graphics, _soundsDict, cah, (int)time, level));
        }
        #endregion

        #region textures and animations
        Dictionary<string, Animation> SetHeroAnimation()
        {
            return new Dictionary<string, Animation>()
            {
                { "WalkRight", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_005"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_006"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_007"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_008"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_009"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_010"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningR/00_Fallen_Angels_Running_011")
                                                                }
                ,12, 0.05f) },
                { "WalkLeft", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_005"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_006"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_007"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_008"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_009"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_010"),
                                                                _content.Load<Texture2D>("HeroAnimations/RunningL/0_Fallen_Angels_Running_011")
                }
                , 12,0.05f) },
                { "JumpUp", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("HeroAnimations/Jump/0_Fallen_Angels_Jump Start_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump/0_Fallen_Angels_Jump Start_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump/0_Fallen_Angels_Jump Start_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump/0_Fallen_Angels_Jump Start_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump/0_Fallen_Angels_Jump Start_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump/0_Fallen_Angels_Jump Start_005"),


                                                                _content.Load<Texture2D>("HeroAnimations/Jump Loop/0_Fallen_Angels_Jump Loop_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump Loop/0_Fallen_Angels_Jump Loop_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump Loop/0_Fallen_Angels_Jump Loop_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump Loop/0_Fallen_Angels_Jump Loop_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump Loop/0_Fallen_Angels_Jump Loop_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/Jump Loop/0_Fallen_Angels_Jump Loop_005")
                }
                , 12,0.05f) },
                { "FallDown", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("HeroAnimations/Falling/0_Fallen_Angels_Falling Down_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/Falling/0_Fallen_Angels_Falling Down_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/Falling/0_Fallen_Angels_Falling Down_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/Falling/0_Fallen_Angels_Falling Down_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/Falling/0_Fallen_Angels_Falling Down_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/Falling/0_Fallen_Angels_Falling Down_005")
                }
                , 6,0.05f) },
                { "Stand", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_005"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_006"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_007"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_008"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_009"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_010"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_011"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_012"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_013"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_014"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_015"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_016"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle Blinking_017"),

                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_000"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_001"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_002"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_003"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_004"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_005"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_006"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_007"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_008"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_009"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_010"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_011"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_012"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_013"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_014"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_015"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_016"),
                                                                _content.Load<Texture2D>("HeroAnimations/Standing/0_Fallen_Angels_Idle_017")


                }
                , 36,0.05f) }
            };
        }
        #endregion


        public static void drawRectangle(SpriteBatch sb, Rectangle rec, ContentManager _content)
        {
            Color color;
            Random rand = new Random();
            int num = rand.Next(100);
            num = num % 6;
            if (num == 0)
                color = Color.Blue;
            if (num == 1)
                color = Color.Red;
            if (num == 2)
                color = Color.Yellow;
            if (num == 3)
                color = Color.Orange;
            if (num == 4)
                color = Color.White;
            if (num == 5)
                color = Color.Purple;
            else
                color = Color.LightGreen;

            DrawLine(sb, new Vector2(rec.Left, rec.Top), new Vector2(rec.Right, rec.Top), color, _content);
            DrawLine(sb, new Vector2(rec.Left, rec.Bottom), new Vector2(rec.Right, rec.Bottom), color, _content);
            DrawLine(sb, new Vector2(rec.Left, rec.Top), new Vector2(rec.Left, rec.Bottom), color, _content);
            DrawLine(sb, new Vector2(rec.Right, rec.Top), new Vector2(rec.Right, rec.Bottom), color, _content);
        }

        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color, ContentManager _content)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(_content.Load<Texture2D>("Controls/point"),
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }


        #endregion

    }
}
