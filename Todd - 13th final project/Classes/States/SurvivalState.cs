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
using Todd.Moduls;
using Todd.Sprites;

namespace Todd.States
{
    public class SurvivalState : State
    {
        #region data
        int worldHight = 2500;
        int worldWidth = 3400;

        SpriteFont font;
        float time = 0f;
        float bossTimer = 0f;
        float endTimer = 0f;
        Sprite hero;
        List<Platform> platforms = new List<Platform>();
        List<Bomb> bombsList;
        List<Point> pathPoints;
        List<Point> bombsPointsList;//bombs point list
        List<string> bossCommands;
        Texture2D redDotTexture;
        Texture2D background;
        Camera_ camera;
        Boss boss;
        Boolean begin = false;
        List<DrawablObject> objects;

        #endregion


        #region constructor
        public SurvivalState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager graphics_, Dictionary<string, Song> sounds) : base(game, graphicsDevice, content, graphics_, sounds)
        {

            pathPoints = new List<Point>() { };
            //font
            font = _content.Load<SpriteFont>("Fonts/font");

            //camera staff
            camera = new Camera_();

            //background
            background = _content.Load<Texture2D>("BackGrounds/Background800x480");

            //red dot texture
            redDotTexture = content.Load<Texture2D>("Controls/point");

            //platforms
            addPlatforms(content);

            //hero
            hero = new Sprite(SetHeroAnimation(), 15f)
            {
                Position = new Vector2(1280, 640),
                Input = new Input()
                {
                    Up = Keys.Up,
                    Left = Keys.Left,
                    Right = Keys.Right,
                },
                hasJumped = true
            };

            //boss
            boss = new Boss(15f, getBossAnimation(), new Vector2(20, 700));

            //bombs
            bombsList = getBombsList(content);

            bombsPointsList = new List<Point>();
            foreach (Bomb b in bombsList)
            {
                if (b.isTaken == 0)
                {
                    bombsPointsList.Add(new Point((b.rectangle.Left + b.rectangle.Right) / 2, (b.rectangle.Top + b.rectangle.Bottom) / 2));
                }
            }

            //boss commands list
            bossCommands = handleFindingShortestWays(new Point((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2));

            //get all drawable object
            objects = getObjects(content);

        }
        #endregion


        #region draw update and postupdate 
        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            //draw static backgound
            spritebatch.Begin();
            spritebatch.Draw(background, new Vector2(0, 0), Color.White);
            spritebatch.DrawString(font, time.ToString(), new Vector2(100, 5), Color.Black, 0f, new Vector2(0, 0), 2f, SpriteEffects.None, 0f);
            spritebatch.End();

            //draw game
            spritebatch.Begin(transformMatrix: camera.transform);

            foreach (Platform platform in platforms)
                platform.Draw(spritebatch);
            foreach (DrawablObject d in objects)
                d.Draw(spritebatch);
            foreach (Bomb bomb in bombsList)
                bomb.Draw(spritebatch);
            hero.Draw(spritebatch);

            if (bombsPointsList.Count() != 0)
            {
                boss.Draw(spritebatch);
                foreach (Point p in pathPoints)
                    spritebatch.Draw(redDotTexture, new Vector2(p.X, p.Y), Color.White);
            }
            else
            {
                if (hero.Position.X > boss.position.X)
                    boss.Draw(spritebatch, "right");
                else
                    boss.Draw(spritebatch, "left");
            }
            spritebatch.End();
        }

        public override void postUpdate(GameTime gametime)
        {
        }

        public override void Update(GameTime gametime)
        {

            //time
            time += (float)gametime.ElapsedGameTime.TotalSeconds;

            //bombs and hero
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


            hero.Update(gametime);
            if (bombsPointsList.Count() == 0)
            {
                boss.velocity = new Vector2(0, 0);
                Random rand = new Random();

                endTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (endTimer > 0.75f)
                {
                    endTimer %= 0.75f;
                    int num = rand.Next(0, 20);
                    num %= 14;
                    if (num == 0)
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/rock"), this._content.Load<Texture2D>("Bomb/rock"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });
                    else if (num < 2)
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/pinkBalloon"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });
                    else if (num < 3)
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/redBaloon"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });
                    else if (num < 4)
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/yellowBaloon"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });
                    else if (num < 7)
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/dynamite"), this._content.Load<Texture2D>("Bomb/exploadedBomb"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });
                    else if (num < 10)
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/bomb"), this._content.Load<Texture2D>("Bomb/exploadedBomb"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });
                    else
                        bombsList.Add(new Bomb(true, this._content.Load<Texture2D>("Bomb/molotov"), this._content.Load<Texture2D>("Bomb/fire"), new Vector2((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2), true) { isTaken = 1 });

                }

            }
            else
            {
                //update boss
                bossTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
                if (bossTimer > 0.5f || bossCommands == null || !bossCommands.Any())
                {
                    //calculate commands every 0.5 second or the list empty
                    bossTimer %= 0.5f;
                    bossCommands = handleFindingShortestWays(new Point((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2));
                }
                //boss colisions
                foreach (Bomb b in bombsList)
                {
                    if (b.rectangle.Intersects(boss.rectangle) && b.isTaken == 0)
                    {
                        b.isTaken = 1;
                        //remove point from point list
                        Point delp = new Point(-1, -1);
                        foreach (Point p in bombsPointsList)
                            if (p.X == (b.rectangle.Left + b.rectangle.Right) / 2 && p.Y == (b.rectangle.Top + b.rectangle.Bottom) / 2)
                                delp = p;
                        if (delp.X != -1 && delp.Y != -1)
                            bombsPointsList.Remove(delp);
                        //update commands
                        bossCommands = handleFindingShortestWays(new Point((boss.rectangle.Left + boss.rectangle.Right) / 2, (boss.rectangle.Top + boss.rectangle.Bottom) / 2));
                    }
                }
                //moves
                try
                {
                    boss.Update(gametime, bossCommands.First());
                    bossCommands.Remove(bossCommands.First());
                }
                catch
                {
                    boss.Update(gametime, "lu");
                }

            }


            //update camera
            try
            {
                camera.Follow(hero);
            }
            catch
            { }
            if (hero.isPlayerDead == true)
                heroDiedAction();
            if (hero.rectangle.isOnTopOf(boss.rectangle))
                winAction();
            else if (hero.rectangle.Intersects(boss.rectangle))
                hero.isPlayerDead = true;


        }

        #endregion

        #region other functions

        #region winnig losing functions
        private void heroDiedAction()
        {
            CoinsAndHearts cah = new CoinsAndHearts(0, (int)time / 60 * 20, (int)time / 2 * 5, 0);
            _game.ChangeState(new PrizesState(_game, _graphicsDevice, _content, graphics, _soundsDict, cah, (int)time, "Survival"));

        }
        private void winAction()
        {
            CoinsAndHearts cah = new CoinsAndHearts(20, (int)time / 60 * 20, (int)time / 2 * 5, 0);
            _game.ChangeState(new PrizesState(_game, _graphicsDevice, _content, graphics, _soundsDict, cah, (int)time, "Survival"));
        }
        #endregion

        #region AI functions

        public List<string> handleFindingShortestWays(Point startingPoint)
        {
            Boolean[,] tFMat = getMapMatrix(platforms, hero);//mat of where we can go
            Double[,] disMat = getRangedMat(tFMat, startingPoint);//mat of distanses
            Point endPoint = getclosestRockOrBomb(bombsPointsList, disMat);//get end point
            //get commands and points
            List<string> whereToGoList;
            whereToGoList = getWhereToGoList(disMat, endPoint, bombsPointsList, startingPoint);


            ////------------
            //printMatrix(tFMat);
            //printMatrix(disMat);
            ////print||||||||||||||||||||||||||||||||============================================================-------------------------------------------------------
            //printList(whereToGoList);
            //printList(pathPoints);


            List<string> final = new List<string>();
            foreach (string cmd in whereToGoList)
            {
                for (int i = 0; i < 2; i++)
                {
                    final.Add(cmd);
                }
            }
            if (final.First() == "error")
            {
                Double a = disMat[(int)endPoint.Y /10, (int)endPoint.X / 10] ;

            }

            return final;
        }//get point the platforms array and the and return the commands and points array to the closest bomb

        public Point getclosestRockOrBomb(List<Point> plst, Double[,] mat)
        {
            Double minVal = 10000;
            Point minPoint;
            try
            {
                minPoint = plst[0];
            }
            catch
            {
                return new Point(-1, -1);
            }
            foreach (Point p in plst)
            {
                if (mat[p.Y / 10, p.X / 10] < minVal)
                {
                    minVal = mat[p.Y / 10, p.X / 10];
                    minPoint = p;
                }
            }
            return minPoint;

        }//get th distanses mat and return the closest bomb place ||| if ret the point (-1,-1) there are no more bombs

        public List<string> getWhereToGoList(Double[,] distanses, Point finishingPoint, List<Point> points, Point startPoint)
        {

            ///scan from the end to the beggining
            ///
            points = new List<Point>();
            List<string> commands = new List<string>();
            Point currPlace = new Point(finishingPoint.X / 10, finishingPoint.Y / 10);

            distanses[startPoint.Y / 10, startPoint.X / 10] = 0;

            while (true)
            {
                if (distanses[currPlace.Y, currPlace.X] == 0)
                    break;

                //find min direction
                Point minDir = getDirectionsArray1()[0];
                Double minVal = 10000;
                foreach (Point dirPoint in getDirectionsArray1())
                {
                    if (currPlace.Y + dirPoint.Y <= worldHight/10 && currPlace.Y + dirPoint.Y >= 0 && currPlace.X + dirPoint.X <= worldWidth/10 && currPlace.X + dirPoint.X >= 0)
                        if (distanses[currPlace.Y + dirPoint.Y, currPlace.X + dirPoint.X] < minVal)
                        {
                            minVal = distanses[currPlace.Y + dirPoint.Y, currPlace.X + dirPoint.X];
                            minDir = dirPoint;
                        }
                }

                //if cant go to nowere
                if (minVal == 10000)
                {
                    //printMatrix(distanses);
                    return (new List<string>() { "error" });

                }

                ///add to list the commands to the boss
                ///ld - left down
                ///l - left
                ///lu - left up
                ///u - up
                ///d - down
                ///rd - right down
                ///r - right
                ///ru - right up
                if (minDir.X > 0)//if go left in game
                {
                    if (minDir.Y > 0)//go up in game
                    {
                        commands.Add("lu");
                    }
                    else if (minDir.Y < 0)//if go down in game
                    {
                        commands.Add("ld");
                    }
                    else//if go letf in game
                    {
                        commands.Add("l");
                    }
                }
                else if (minDir.X < 0)//if go right in game
                {
                    if (minDir.Y > 0)//go up in game
                    {
                        commands.Add("ru");
                    }
                    else if (minDir.Y < 0)//if go down in game
                    {
                        commands.Add("rd");
                    }
                    else//if go right
                    {
                        commands.Add("r");
                    }
                }
                else//if go up and down in game
                {
                    if (minDir.Y > 0)//go up in game
                    {
                        commands.Add("u");
                    }
                    else if (minDir.Y < 0)//if go down in game
                    {
                        commands.Add("d");
                    }
                    else
                    {
                        throw new NotImplementedException("error in movement in sccaning the mat");
                    }
                }

                points.Add(new Point(currPlace.X * 10, currPlace.Y * 10));
                //go to the next point
                currPlace = new Point(currPlace.X + minDir.X, currPlace.Y + minDir.Y);
            }

            commands.Reverse();
            points.Reverse();
            pathPoints = points;
            return (commands);
        }// return array of how to go and points he be there

        public Double[,] getRangedMat(Boolean[,] isCanGo, Point startPoint)
        {
            Double[,] valMat = new Double[worldHight / 10, worldWidth / 10];
            Boolean[,] used = new Boolean[worldHight / 10, worldWidth / 10];
            List<Point> curr = new List<Point>();
            List<Point> next = new List<Point>();
            Point p = new Point(startPoint.X / 10, startPoint.Y / 10);

            //fill the place we cant there with huge number
            for (int i = 0; i < valMat.GetLength(0); i++)
            {
                for (int j = 0; j < valMat.GetLength(1); j++)
                {
                    if (isCanGo[i, j] == false)
                    {
                        valMat[i, j] = 10000;
                        used[i, j] = true;
                    }
                }
            }

            //do the numbering
            valMat[p.Y, p.X] = 0;
            used[p.Y, p.X] = true;

            curr.Add(p);

            while (true)
            {
                //when we arrive the last layer
                if ((curr == null) || (!curr.Any()))
                    break;

                //check for all the points left right up and own
                foreach (Point basePoint in curr)
                {
                    //check around
                    foreach (Point dirPoint in getDirectionsArray1())
                    {
                        if (basePoint.Y + dirPoint.Y <= worldHight / 10 - 1 && basePoint.Y + dirPoint.Y >= 0 && basePoint.X + dirPoint.X <= worldWidth / 10 - 1 && basePoint.X + dirPoint.X >= 0)
                        {
                            if (dirPoint.X * dirPoint.X != dirPoint.Y * dirPoint.Y)
                            {
                                //if you can go there
                                if (isCanGo[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] == true)
                                {
                                    if (used[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] == false)
                                    {
                                        //write the distance
                                        used[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] = true;
                                        valMat[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] = valMat[basePoint.Y, basePoint.X] + 10;
                                        next.Add(new Point(basePoint.X + dirPoint.X, basePoint.Y + dirPoint.Y));
                                    }
                                    else
                                    {
                                        if (valMat[basePoint.Y, basePoint.X] + 10 < valMat[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X])
                                        {
                                            valMat[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] = valMat[basePoint.Y, basePoint.X] + 10;
                                            //next.Add(new Point(basePoint.X + dirPoint.X, basePoint.Y + dirPoint.Y));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //if you can go there
                                if (isCanGo[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] == true)
                                {
                                    if (used[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] == false)
                                    {
                                        //write the distance
                                        used[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] = true;
                                        valMat[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] = valMat[basePoint.Y, basePoint.X] + 14.14;
                                        next.Add(new Point(basePoint.X + dirPoint.X, basePoint.Y + dirPoint.Y));
                                    }
                                    else
                                    {
                                        if (valMat[basePoint.Y, basePoint.X] + 14.14 < valMat[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X])
                                        {
                                            valMat[basePoint.Y + dirPoint.Y, basePoint.X + dirPoint.X] = valMat[basePoint.Y, basePoint.X] + 14.14;
                                            next.Add(new Point(basePoint.X + dirPoint.X, basePoint.Y + dirPoint.Y));
                                        }
                                    }
                                }

                            }
                        }


                    }
                }



                curr = next;
                next = new List<Point>();
            }

            return valMat;
        }//get a point and the t f matrix andreturn a matrix that with distanse from the pointt

        private Point[] getDirectionsArray1()
        {
            Point[] arr = new Point[8];
            int j = 0;
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x != 0 || y != 0)
                    {
                        arr[j++] = new Point(x, y);
                    }
                }
            }
            return arr;
        }//get the 8 directions vector

        public Boolean[,] getMapMatrix(List<Platform> list, Sprite s)
        {
            Boolean[,] matrix = new Boolean[worldHight / 10, worldWidth / 10];

            //make all true;
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = true;

            foreach (Platform plt in list)
            {
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        matrix[(int)(plt.rectangle.Top / 10f) + i, (int)(plt.rectangle.Left / 10f) + j] = false;
                    }
                }
            }

            int x = 10;
            for (int i = 0; i < 23; i++)
                for (int j = 0; j < 23; j++)
                    try
                    {
                        matrix[(int)(s.Position.Y / 10f) + i - x, (int)(s.Position.X / 10f) + j - x] = false;
                    }
                    catch
                    { }

            return matrix;
        }//take platforms and sprite and gives matrix that show there you can walk

        #region printing functions
        public void printMatrix(Boolean[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {

                    if (matrix[i, j] == true)
                        Console.Write(" ");
                    else
                        Console.Write("X");
                }
                Console.WriteLine();
            }

        }//print the matrix

        public void printMatrix(Double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j]);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }

        }//print the matrix

        public void printList(List<string> lst)
        {
            foreach (string str in lst)
            {
                Console.Write(str);
                Console.Write(" , ");

            }
            Console.WriteLine("");
        }//print list of strings
        public void printList(List<Point> lst)
        {
            Console.WriteLine("X   |   Y");
            Console.WriteLine("__________");



            foreach (Point p in lst)
            {
                Console.Write(p.X);
                Console.Write("  ");
            }
        }//print list of strings

        #endregion
        #endregion

        #region textures and animations
        private List<DrawablObject> getObjects(ContentManager content)
        {

            //add spikes in botton
            List < DrawablObject > lst = new List<DrawablObject>();
            for (int i = -512; i < 3200; i += 128)
                lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Spikes"), 100, new Vector2(i, 2350), false));

            //add components
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Decor_Ruins_01"), 80, new Vector2(256, 256), false));

            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Decor_Ruins_02"), 80, new Vector2(1024, 640), false)); 

            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Decor_Statue"), 100, new Vector2(490, 1024), false)); 

            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Fence"), 100, new Vector2(800, 256), false)); 
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Fence"), 100, new Vector2(1664, 1792), false)); 
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Fence"), 100, new Vector2(640, 1792), false));
            
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Little_Wreckage"), 100, new Vector2(768, 1792), false));
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Little_Wreckage"), 100, new Vector2(2432, 640), false));

            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(2176, 2048), false)); 
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(768, 1792), false)); 
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(1792, 1792), false)); 
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(384, 1536), false)); 
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_04"), 100, new Vector2(2560, 1792), false));
            
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(896, 256), false));
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_02"), 100, new Vector2(640, 1152), false));
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_01"), 100, new Vector2(768, 640), false));
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_02"), 100, new Vector2(1920, 896), false));
            lst.Add(new DrawablObject(content.Load<Texture2D>("Components/Sign_03"), 100, new Vector2(1280, 896), false));


            return lst;
            
        }

        private Dictionary<string, Animation> getBossAnimation()
        {
            return new Dictionary<string, Animation>()
            {
                { "Walk", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_000"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_001"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_002"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_003"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_004"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_005"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_006"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_007"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_008"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_009"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_010"),
                                                                _content.Load<Texture2D>("BossAnimations/Running/0_Fallen_Angels_Running_011")
                                                                }
                ,12, 0.05f) },
                { "Stand", new Animation(new List<Texture2D>(){
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_000"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_001"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_002"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_003"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_004"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_005"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_006"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_007"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_008"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_009"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_010"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_011"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_012"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_013"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_014"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_015"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_016"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle Blinking_017"),

                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_000"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_001"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_002"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_003"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_004"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_005"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_006"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_007"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_008"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_009"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_010"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_011"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_012"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_013"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_014"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_015"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_016"),
                                                                _content.Load<Texture2D>("BossAnimations/Standing/0_Fallen_Angels_Idle_017")


                }
                , 36,0.05f) }
            };
        }

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

        List<Bomb> getBombsList(ContentManager content)
        {
            Texture2D bomb = content.Load<Texture2D>("Bomb/bomb");
            Texture2D ex = content.Load<Texture2D>("Bomb/exploadedBomb");
            Texture2D fire = content.Load<Texture2D>("Bomb/fire");
            Texture2D mol = content.Load<Texture2D>("Bomb/molotov");
            Texture2D rock = content.Load<Texture2D>("Bomb/rock");
            Texture2D pbl = content.Load<Texture2D>("Bomb/pinkBalloon");
            Texture2D ybl = content.Load<Texture2D>("Bomb/yellowBaloon");
            Texture2D d = content.Load<Texture2D>("Bomb/dynamite");
            Texture2D rbl = content.Load<Texture2D>("Bomb/redBaloon"); 

             Random rand = new Random();
            List<Bomb> bmb = new List<Bomb>();
            List<Vector2> positions = new List<Vector2>()
            {
                { new Vector2(256,256)},
                { new Vector2(1920,128)},
                { new Vector2(2560,256)},
                { new Vector2(1792,512)},
                { new Vector2(2688,640)},
                { new Vector2(2304,1024)},
                { new Vector2(1792,1280)},
                { new Vector2(1280,1280)},
                { new Vector2(256,1536)},
                { new Vector2(2688,1536)},
                { new Vector2(2048,1792)},
                { new Vector2(1024,1920)},
                { new Vector2(256,2048)},
                { new Vector2(2560,2176)}
            };
            foreach( Vector2 pos in positions)
            {
                int num = rand.Next(0, 20);
                num %= 7;
                if (num == 0)
                    bmb.Add(new Bomb(false, rock, rock, pos));
                else if (num == 1)
                    bmb.Add(new Bomb(true, bomb, ex, pos));
                else if (num == 2)
                    bmb.Add(new Bomb(true, ybl, fire, pos));
                else if (num == 3)
                    bmb.Add(new Bomb(true, pbl, fire, pos));
                else if (num == 4)
                    bmb.Add(new Bomb(true, rbl, fire, pos));
                else if (num == 5)
                    bmb.Add(new Bomb(true, d, ex, pos));
                else
                    bmb.Add(new Bomb(true, mol, fire, pos));
            }



            return bmb;

        }

        void addPlatforms(ContentManager content)
        {
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(256, 640)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(384, 640)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(512, 640)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(256, 896)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(384, 896)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(640, 768)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(768, 768)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(768, 384)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(896, 384)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(1152, 512)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(1280, 512)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1024, 1024)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(1152, 1024)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1280, 1024)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Wooden_Barrel"), new Vector2(1152, 896)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(384, 1280)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(512, 1280)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(640, 1280)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(384, 1664)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(512, 1664)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(640, 1920)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(768, 1920)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(896, 1664)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1024, 1664)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(896, 1280)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1024, 1280)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1280, 1536)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1408, 1536)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(1408, 768)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(1536, 768)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(2048, 640)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(2176, 640)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(2432, 768)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(2560, 768)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(2304, 1280)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(2432, 1280)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(2048, 1536)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(2176, 1536)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1664, 1920)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(1792, 1920)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(1920, 1920)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(2304, 1920)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(2432, 1920)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(2560, 1920)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_10"), new Vector2(1792, 1024)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_11"), new Vector2(1920, 1024)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Ground_12"), new Vector2(2048, 1024)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_01"), new Vector2(2176, 2176)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(1536, 1664)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Wooden_Box"), new Vector2(1536, 1280)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Brick_02"), new Vector2(1792, 640)));

            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Wooden_Barrel"), new Vector2(2048, 1408)));
            platforms.Add(new Platform(content.Load<Texture2D>("Platforms/Wooden_Barrel"), new Vector2(2304, 1792)));

        }
        #endregion

        #endregion
    }
}
#region rectangle helper
static class RectangleHelper
{
    const int penetrationMargin = 5;
    public static bool isOnTopOf(this Rectangle r1, Rectangle r2)
    {
        return (r1.Bottom >= r2.Top - penetrationMargin &&
            r1.Bottom <= r2.Top + 1 &&
            r1.Right >= r2.Left + 5 &&
            r1.Left <= r2.Right - 5);
    }

}
#endregion