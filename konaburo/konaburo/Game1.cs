using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace konaburo
{
    /// <summary>
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D   texture;


        /* ブロック崩しアイテム */
        Ball[] ball = new Ball[1];
        Block[] block = new Block[1024];
        Block_Particle[] block_particle = new Block_Particle[10240*2];
        Bar bar = new Bar();

        /* ゲーム領域 */
        Field field = new Field();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します。

            base.Initialize();
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            // 新規の SpriteBatch を作成します。これはテクスチャーの描画に使用できます。
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: this.Content クラスを使用して、ゲームのコンテンツを読み込みます。
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            texture = Content.Load<Texture2D>("white");

            /* ブロック崩し　初期化 */
            for (int i = 0; i < ball.Length; i++)
            {
                ball[i] = new Ball();
            }
            for (int i = 0; i < ball.Length; i++)
            {
                Random r = new Random((int)System.DateTime.Now.Ticks);
                ball[i].pos.X = r.Next() % (int)(field.rect.Width - ball[i].size.X);
                ball[i].pos.Y = field.rect.Height - ball[i].size.Y * 2;

                ball[i].enable = true;
            }

            const int Block_X_Count = 16;
            const int Block_Y_Count = 24;
            const int Block_X_Space = 1;
            const int Block_Y_Space = 1;

            for (int i = 0; i < block.Length; i++)
            {
                block[i] = new Block();
            }
            for (int i = 0; i < block.Length; i++)
            {
                if (Block_Y_Count < (i / Block_X_Count)) break;

                block[i].pos.X = (i % Block_X_Count) * (block[i].size.X + Block_X_Space);
                block[i].pos.Y = (i / Block_X_Count) * (block[i].size.Y + Block_Y_Space);
                block[i].enable = true;    
            }

            for (int i = 0; i < block_particle.Length; i++)
            {
                block_particle[i] = new Block_Particle();
            }

            bar.pos.X = field.rect.Width / 2 - bar.size.X / 2;
            bar.pos.Y = field.rect.Bottom - 100;
        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
        }

        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard_state = Keyboard.GetState(PlayerIndex.One);

            // ゲームの終了条件をチェックします。
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            

            // TODO: ここにゲームのアップデート ロジックを追加します。

            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ||
                keyboard_state.IsKeyDown(Keys.Left) == true )
            {
                bar.pos.X -= 5.0f;
            }
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ||
                keyboard_state.IsKeyDown(Keys.Right) == true)
            {
                bar.pos.X += 5.0f;
            }

            for (int i = 0; i < ball.Length; i++)
            {
                if (ball[i].enable == true)
                {
                    ball[i].Move();

                    /* 反射ロジック */
                    if (ball[i].pos.X < field.rect.Left) ball[i].speed.X = Math.Abs(ball[i].speed.X);
                    if (ball[i].pos.Y < field.rect.Top) ball[i].speed.Y = Math.Abs(ball[i].speed.Y);
                    if (ball[i].pos.X > field.rect.Right - ball[i].size.X) ball[i].speed.X = -Math.Abs(ball[i].speed.X);
                    if (ball[i].pos.Y > field.rect.Bottom - ball[i].size.Y) ball[i].speed.Y = -Math.Abs(ball[i].speed.Y);

                    Rectangle ball_rect = new Rectangle((int)ball[i].pos.X, (int)ball[i].pos.Y, (int)ball[i].size.X, (int)ball[i].size.Y);


                    /* バーとの重なり検出 */
                    if (ball_rect.Intersects(new Rectangle((int)bar.pos.X, (int)bar.pos.Y, (int)bar.size.X, (int)bar.size.Y)) == true)
                    {
                        ball[i].speed.Y *= -1;
                    }

                    /* ブロックとの重なり検出 */
                    for (int j = 0; j < block.Length; j++)
                    {
                        if (block[j].enable == true)
                        {
                            Rectangle block_rect = new Rectangle((int)block[j].pos.X, (int)block[j].pos.Y, (int)block[j].size.X, (int)block[j].size.Y);

                            if (block_rect.Intersects(ball_rect) == true)
                            {
                                block[j].enable = false;

                                /* エフェクト生成 */
                                int eff_count = 0;
                                for (int k = 0; k < block_particle.Length; k++)
                                {
                                    if (block_particle[k].enable == false)
                                    {
                                        block_particle[k].enable = true;
                                        block_particle[k].pos.X = block[j].pos.X + (eff_count % block[j].size.X);
                                        block_particle[k].pos.Y = block[j].pos.Y + (eff_count / block[j].size.X);
                                        block_particle[k].speed.X = ball[i].speed.X + (block_particle[k].pos.X - (ball[i].pos.X + ball[i].size.X / 2)) / 10.0f;
                                        block_particle[k].speed.Y = ball[i].speed.Y + (block_particle[k].pos.Y - (ball[i].pos.Y + ball[i].size.Y / 2)) / 10.0f;
                                        eff_count++;
                                        if (eff_count > block[j].size.X * block[j].size.Y)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /* ブロック破片移動 */
            for (int i = 0; i < block_particle.Length; i++)
            {
                block_particle[i].Move();
                // if (field.rect.Intersects(new Rectangle((int)block_particle[i].pos.X, (int)block_particle[i].pos.Y,
                //    (int)block_particle[i].size.X, (int)block_particle[i].size.Y)) == false)
                {
                    if (field.rect.Bottom < block_particle[i].pos.Y)
                    {
                        block_particle[i].speed.Y = -Math.Abs(block_particle[i].speed.Y) * 0.4f;

                        if (Math.Abs(block_particle[i].speed.Y) < 0.1f)
                        {
                            block_particle[i].enable = false;
                        }

                        /* 表示範囲外移動でブロック破片無効化 */
                        //block_particle[i].enable = false;
                    }

                    if (field.rect.Top > block_particle[i].pos.Y)
                    {
                        block_particle[i].speed.Y = Math.Abs(block_particle[i].speed.Y);
                    }
                    if (field.rect.Left > block_particle[i].pos.X)
                    {
                        block_particle[i].speed.X = Math.Abs(block_particle[i].speed.X);
                    }
                    if (field.rect.Right < block_particle[i].pos.X)
                    {
                        block_particle[i].speed.X = -Math.Abs(block_particle[i].speed.X);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: ここに描画コードを追加します。

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, "KONABURO! - konagona burokku", new Vector2(0.0f, 0.0f), Color.Black);
            
            
            /* ボール描画 */
            for (int i = 0; i < ball.Length; i++)
            {
                if (ball[i].enable == true)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)ball[i].pos.X, (int)ball[i].pos.Y, (int)ball[i].size.X, (int)ball[i].size.Y), Color.White);
                }
            }

            /* ブロック描画 */
            for (int i = 0; i < block.Length; i++)
            {
                if (block[i].enable == true)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)block[i].pos.X, (int)block[i].pos.Y, (int)block[i].size.X, (int)block[i].size.Y), Color.White);
                }
            }

            /* ブロック破片描画 */
            for (int i = 0; i < block_particle.Length; i++)
            {
                if (block_particle[i].enable == true)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)block_particle[i].pos.X, (int)block_particle[i].pos.Y, (int)block_particle[i].size.X, (int)block_particle[i].size.Y), Color.White);
                }
            }

            /* バー描画 */
            spriteBatch.Draw(texture, new Rectangle((int)bar.pos.X, (int)bar.pos.Y, (int)bar.size.X, (int)bar.size.Y), Color.White);


            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
