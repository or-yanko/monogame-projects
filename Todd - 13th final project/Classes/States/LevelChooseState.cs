using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.Moduls;
using Todd.Sprites;

namespace Todd.States
{
    class LevelChooseState: State
    {
        
        #region data

        //array of all the components
        private List<Button> _components;

        //background picture
        private Texture2D background;

        #endregion

        #region constaructor
        public LevelChooseState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) :
            base(game, graphicsDevice, content, graphics_, sounds)
        {
            //load backgroound
            background = _content.Load<Texture2D>("Backgrounds/mainMenuBackground");

            //font for the text (i dont use it because i dont write text on the button)
            SpriteFont btnTextFont = _content.Load<SpriteFont>("Fonts/font");

            //handle backButton button
            var backButton = new Button(_content.Load<Texture2D>("Buttons/backBtn"), new Vector2(5, 5),
                Color.Black, Color.White, btnTextFont, "", 15);
            backButton.Click += backButtonClick;

            //handle levels buttons
            var lvl1 = new Button(_content.Load<Texture2D>("Buttons/lvl1"), new Vector2(330, 200),
                Color.Black, Color.White, btnTextFont, "", 100);

            var lvl2 = new Button(_content.Load<Texture2D>("Buttons/lvl2"), new Vector2(330, 285),
                Color.Black, Color.White, btnTextFont, "", 100);

            var lvl3 = new Button(_content.Load<Texture2D>("Buttons/lvl3"), new Vector2(330, 370),
                Color.Black, Color.White, btnTextFont, "", 100);

            lvl1.Click += lvl1click;
            lvl2.Click += lvl2click;
            lvl3.Click += lvl3click;
            backButton.Click += backButtonClick;


            //create the list
            _components = new List<Button>()
        {
            backButton,lvl1,lvl2,lvl3
        };
        }

        #endregion

        #region update draw and postUpdate
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(spritebatch);

            spritebatch.End();

        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {
            foreach (var component in _components)
                component.Update(gametime);
        }
        #endregion


        #region buttons click actions

        private void backButtonClick(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new GameMenuState(_game, _graphicsDevice, _content, graphics, _soundsDict));
        }

        private void lvl3click(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict,new GameState(_game, _graphicsDevice, _content, graphics, _soundsDict, getLevel3Enemies(), getLevel3Platforms(), getLevel3Objects(), getLevel3Coins(), getLevel3Star(), 3),4.5f));
        }

        private void lvl2click(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict, new GameState(_game, _graphicsDevice, _content, graphics, _soundsDict, getLevel2Enemies(), getLevel2Platforms(), getLevel2Objects(), getLevel2Coins(), getLevel2Star(),2), 4.5f));
        }

        private void lvl1click(object sender, EventArgs e)
        {
            btnClickSoundAction();
            _game.ChangeState(new LoadingState(_game, _graphicsDevice, _content, graphics, _soundsDict, new GameState(_game, _graphicsDevice, _content, graphics, _soundsDict, getLevel1Enemies(), getLevel1Platforms(), getLevel1Objects(), getLevel1Coins(), getLevel1Star(),1), 4.5f));
        }
        #endregion

        #region textures and animations

        Dictionary<string, Animation> getPatrollingAnimation()
        {
            Random rand = new Random();
            int num = rand.Next(0, 69);
            num %= 6;
            if (num == 0)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_000"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_001"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_002"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_003"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_004"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_005"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_006"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_007"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_008"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_009"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_010"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G1/0_Golem_Running_011")
                    }
                    ,12,0.05f)
                    }
                };

            }
            else if (num == 1)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_000"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_010"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_002"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_003"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_004"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_005"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_006"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_007"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_008"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_009"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_010"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G2/0_Golem_Running_011")
                    }
                    ,12,0.05f)
                    }
                };

            }
            else if(num == 2)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_000"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_010"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_002"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_003"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_004"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_005"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_006"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_007"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_008"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_009"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_010"),
                                                                    _content.Load<Texture2D>("GolemAnimation/G3/0_Golem_Running_011")
                    }
                    ,12,0.05f)
                    }
                };

            }
            else if(num == 3)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_000"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_001"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_002"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_003"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_004"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_005"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_006"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_007"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_008"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_009"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_010"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O1/0_Goblin_Running_011"),
                    }
                    ,12,0.05f)
                    }
                };

            }
            else if(num == 4)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_000"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_001"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_002"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_003"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_004"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_005"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_006"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_007"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_008"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_009"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_010"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O2/0_Ogre_Running_011"),
                    }
                    ,12,0.05f)
                    }
                };

            }
            else if(num == 5)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_000"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_001"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_002"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_003"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_004"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_005"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_006"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_007"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_008"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_009"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_010"),
                                                                    _content.Load<Texture2D>("OrcAnimation/O3/0_Orc_Running_011"),
                    }
                    ,12,0.05f)
                    }
                };

            }
            else
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_000"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_001"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_002"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_003"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_004"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_005"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_006"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_007"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_008"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_009"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_010"),
                                                                    _content.Load<Texture2D>("ReaperAnimation/R1/0_Reaper_Man_Running_011"),
                    }
                    ,12,0.05f)
                    }
                };

            }
        }
        Dictionary<string, Animation> getFlyPatrollingAnimation()
        {
            
            Random rand = new Random();
            int num = rand.Next(0, 69);
            num %= 3;
            if (num == 0)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_000"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_001"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_002"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_003"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_004"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_005"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_006"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_007"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_008"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_009"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_010"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F1/Wraith_01_Moving Forward_011"),
                    },12,0.05f)
                    }
                };

            }

            else if (num == 1)
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_000"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_001"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_002"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_003"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_004"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_005"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_006"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_007"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_008"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_009"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_010"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F2/Wraith_02_Moving Forward_011"),
                    },12,0.05f)
                    }
                };

            }

            else
            {
                return new Dictionary<string, Animation>()
                {
                    { "WalkRight", new Animation(new List<Texture2D>(){
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_000"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_001"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_002"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_003"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_004"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_005"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_006"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_007"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_008"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_009"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_010"),
                                                                    _content.Load<Texture2D>("FlyingCreatureAnimation/F3/Wraith_03_Moving Forward_011"),
                    },12,0.05f)
                    }
                };

            }

        }
        Dictionary<string, Animation> SetGoldAnimation()
        {
            return new Dictionary<string, Animation>()
            {
                { "spin", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_1"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_2"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_3"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_4"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_5"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_6"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_7"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_8"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_9"),
                                                                _content.Load<Texture2D>("CoinAnimation/Gold/Gold_10")
                }
                ,10,0.1f)
                }
            };
        }
        Dictionary<string, Animation> SetBronzeAnimation()
        {
            return new Dictionary<string, Animation>()
            {
                { "spin", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_10"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_1"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_2"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_3"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_4"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_5"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_6"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_7"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_8"),
                                                                _content.Load<Texture2D>("CoinAnimation/Bronze/Bronze_9")
                }
                        ,10,0.1f)
                }
            };
        }
        Dictionary<string, Animation> SetSilverAnimation()
        {
            return new Dictionary<string, Animation>()
            {
                { "spin", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_5"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_6"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_7"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_8"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_9"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_10"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_4"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_3"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_2"),
                                                                _content.Load<Texture2D>("CoinAnimation/Silver/Silver_1"),
                }
                        ,10,0.1f)
                }
            };
        }

        #endregion

        #region components functions

        #region level 1
        List<Platform> getLevel1Platforms()
        {
            List<Platform> lst = new List<Platform>();

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(128, 256)));
            for (int i = 256; i < 1400; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(i, 256)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1408, 256)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1408, 896)));
            for (int i = 1536; i < 3000; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(i, 896)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(3072, 896)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(3072, 1536)));
            for (int i = 3200; i < 4300; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(i, 1536)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(4352, 1536)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(4480, 1280)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(4608, 1024)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(4480, 768)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(4352, 512)));
            return lst;
        }
        List<DrawablObject> getLevel1Objects()
        {
            List<DrawablObject> lst = new List<DrawablObject>();

            for (int i = -1024; i < 5500; i += 128)
                lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Spikes"), 100, new Vector2(i, 2350), false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Ruins_01"), 80, new Vector2(1920, 512), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Ruins_02"), 80, new Vector2(2304, 512), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(1408, 128), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(3072, 768), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_02"), 100, new Vector2(4352, 1408), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(1408, 768), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(3072, 1408), false));

            return lst;
        }
        List<Enemy> getLevel1Enemies()
        {
            List<Enemy> lst = new List<Enemy>();
            int i = 170;

            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(2048, 512), new Vector2(1, 0), 1900, 2800, true, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 896 - i), new Vector2(1, 0), 1536, 3072, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 896 - i), new Vector2(1.2f, 0), 1536, 3072, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 896 - i), new Vector2(1.4f, 0), 1536, 3072, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 896 - i), new Vector2(1.5f, 0), 1536, 3072, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 896 - i), new Vector2(0.5f, 0), 1536, 3072, false, 400));

            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(1.1f, 0), 3200, 4352, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(1.3f, 0), 3200, 4352, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(-2, 0), 3200, 4352, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(1, 0), 3200, 4352, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(-1.8f, 0), 3200, 4352, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(-1, 0), 3200, 4352, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(330, 1536 - i), new Vector2(1.9f, 0), 3200, 4352, false, 400));

            return lst;
        }
        List<Coin> getLevel1Coins()
        {
            List<Coin> lst = new List<Coin>();
            Random rand = new Random();


            for (int i = 256; i < 1700; i += 70)
            {
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, 165), "Gold"));
                else if(num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, 165), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, 165), "Bronze"));
            }

            for (int i = 1408; i < 3200; i += 70)
            {
                int a = 810;
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, a), "Gold"));
                else if (num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, a), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, a), "Bronze"));
            }

            for (int i = 3072; i < 4480; i += 70)
            {
                int a = 1440;
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, a), "Gold"));
                else if (num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, a), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, a), "Bronze"));
            }

            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(4500, 1152), "Silver"));
            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(4628, 896), "Silver"));
            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(4500, 640), "Silver"));
            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(4372, 384), "Silver"));

            lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(4480, 256), "Gold"));


            return lst;
        }
        DrawablObject getLevel1Star()
        {
            return new DrawablObject(_content.Load<Texture2D>("Components/star"), 100, new Vector2(4224, 256), true);
        }
        #endregion

        #region level 2
        List<Platform> getLevel2Platforms()
        {
            List<Platform> lst = new List<Platform>();

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(128, 256)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(256, 256)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(384, 384)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(512, 384)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(640, 512)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(768, 512)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(896, 640)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1024, 640)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1152, 1024)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1280, 1024)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(512, 1152)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(640, 1152)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(768, 1152)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(896, 1152)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(128, 1408)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(512, 1536)));
            for (int i = 640; i < 3300; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(i, 1536)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(3328, 1536)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(3456, 1280)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(3328, 1024)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(3200, 768)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(3072, 512)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(3200, 256)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(3584, 128)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(3712, 256)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(3840, 256)));
            return lst;
        }
        List<DrawablObject> getLevel2Objects()
        {
            List<DrawablObject> lst = new List<DrawablObject>();

            for (int i = -1024; i < 5500; i += 128)
                lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Spikes"), 100, new Vector2(i, 2350), false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Statue"), 100, new Vector2(1152, 768), false)); 

            for (int i = 512; i < 3456; i += 128)
                lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Grass_02"), 100, new Vector2(i, 1472), false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Plant"), 100, new Vector2(3456, 1300), false)); 
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Plant"), 100, new Vector2(3328, 1044), false)); 
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Plant"), 100, new Vector2(3200, 788), false)); 
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Plant"), 100, new Vector2(3072, 532), false)); 
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Decor_Plant"), 100, new Vector2(3200, 276), false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(1024, 512), false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(1152, 896), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_05"), 100, new Vector2(1280, 896), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_06"), 100, new Vector2(3072, 1408), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_06"), 100, new Vector2(3200, 1408), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_06"), 100, new Vector2(3328, 1408), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_01"), 100, new Vector2(3840, 128), false, false));



            return lst;
        }
        List<Enemy> getLevel2Enemies()
        {
            List<Enemy> lst = new List<Enemy>();
            int i = 170;

            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(2048, 1280), new Vector2(1.5f, 0), 1900, 2800, true, 500));
            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(1280, 1280), new Vector2(2, 0), 1408, 2816, true, 500));
            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(1408, 768), new Vector2(1, 0), 1408, 2048, true, 500));

            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(1, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(1.5f, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(2, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(3, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(4, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(2.7f, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(3.3f, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(5f, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(1.1f, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(1.9f, 0), 512, 3456, false, 400));
            lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1536 - i), new Vector2(-1, 0), 512, 3456, false, 400));

            return lst;
        }
        List<Coin> getLevel2Coins()
        {
            List<Coin> lst = new List<Coin>();
            Random rand = new Random();


            for (int i = 512; i < 3500; i += 70)
            {
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, 1446), "Gold"));
                else if (num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, 1446), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, 1446), "Bronze"));
            }

            for (int i = 512; i < 1024; i += 70)
            {
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, 1062), "Gold"));
                else if (num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, 1062), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, 1062), "Bronze"));
            }

            int a = 90;
            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(1152, 934), "Silver"));
            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(1152 + a, 934), "Silver"));
            lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(1152 + a + a, 934), "Silver"));

            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(384, 294), "Bronze"));
            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(384 + a, 294), "Bronze"));
            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(384 + a + a, 294), "Bronze"));

            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(640, 422), "Bronze"));
            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(640 + a, 422), "Bronze"));
            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(640 + a + a, 422), "Bronze"));

            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(896, 550), "Bronze"));
            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(896 + a, 550), "Bronze"));
            lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(896 + a + a, 550), "Bronze"));

            return lst;
        }
        DrawablObject getLevel2Star()
        {
            return new DrawablObject(_content.Load<Texture2D>("Components/star"), 100, new Vector2(4224, 256), true);
        }


        #endregion

        #region level 3
        List<Platform> getLevel3Platforms()
        {
            List<Platform> lst = new List<Platform>();

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(128, 256)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(256, 512)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(512, 768)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(768, 1152)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(512, 1536)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(896, 1792)));



            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1280, 1792)));
            for (int i = 1408; i < 3800; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(i, 1792)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(3840, 1792)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(896, 1792)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(2944, 1536)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(2816, 1536)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(2688, 1536)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Wooden_Barrel"), new Vector2(2688, 1408)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(2176, 1408)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(2048, 1152)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(1920, 896)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(1920, 896)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(1792, 640)));

            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(2304, 640)));
            for (int i = 2432; i < 4000; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(i, 640)));
            lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(4096, 640)));

            for (int i = 4352; i < 4800; i += 128)
                lst.Add(new Platform(_content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(i, 1280)));


            return lst;

        }
        List<DrawablObject> getLevel3Objects()
        {
            List<DrawablObject> lst = new List<DrawablObject>();

            for (int i = -1024; i < 5500; i += 128)
                lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Spikes"), 100, new Vector2(i, 2350), false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(128, 128), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(256, 384), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(512, 640), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(768, 1024), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(512, 1408), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_01"), 100, new Vector2(896, 1664), false, false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_01"), 100, new Vector2(2688, 1280), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_02"), 100, new Vector2(2176, 1280), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(2048, 1024), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(1920, 768), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_01"), 100, new Vector2(1792, 512), false, false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(4096, 512), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(4352, 1152), false, true));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(4736, 1152), false, false));

            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_06"), 100, new Vector2(3200, 1664), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_05"), 100, new Vector2(3328, 1664), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_05"), 100, new Vector2(3456, 1664), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(3584, 1664), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(3712, 1664), false, false));
            lst.Add(new DrawablObject(_content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(3840, 1664), false, false));


            return lst;
        }
        List<Enemy> getLevel3Enemies()
        {
            List<Enemy> lst = new List<Enemy>();

            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(1408, 1024), new Vector2(1, 0), 1024, 1792, true, 500));
            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(1408, 1024), new Vector2(3, 0), 2560, 4480, true, 500));
            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(1408, 256), new Vector2(3, 0), 2560, 4480, true, 500));
            lst.Add(new Enemy(getFlyPatrollingAnimation(), 40, new Vector2(1, 1152), new Vector2(0.5f, 2), 0, 512, true, 500));

            int a = 170;
            for (float i = -4; i < 4.3; i += 0.5f)
                lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(1536, 1792 - a), new Vector2(i, 0), 1408, 3840, false, 400));
            for (float i = -4; i < 4.3; i += 0.5f)
                lst.Add(new Enemy(getPatrollingAnimation(), 24, new Vector2(3000, 640 - a), new Vector2(i, 0), 2304, 4096, false, 400));


            return lst;
        }
        List<Coin> getLevel3Coins()
        {
            List<Coin> lst = new List<Coin>();
            Random rand = new Random();

            for (int i = 1280; i < 3800; i += 70)
            {
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, 1702), "Gold"));
                else if (num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, 1702), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, 1702), "Bronze"));
            }

            for (int i = 2304; i < 4100 ; i += 70)
            {
                int num = rand.Next(0, 1000);
                if (num % 100 == 0)
                    lst.Add(new Coin(SetGoldAnimation(), 10, new Vector2(i, 550), "Gold"));
                else if (num % 10 == 0)
                    lst.Add(new Coin(SetSilverAnimation(), 10, new Vector2(i, 550), "Silver"));
                else
                    lst.Add(new Coin(SetBronzeAnimation(), 10, new Vector2(i, 550), "Bronze"));
            }


            return lst;
        }
        DrawablObject getLevel3Star()
        {
            return new DrawablObject(_content.Load<Texture2D>("Components/star"), 100, new Vector2(4608, 1792), true);
        }


        #endregion

        #endregion
    }
}

