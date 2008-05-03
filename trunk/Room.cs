using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DB.DoF.Entities;

namespace DB.DoF
{

    public class Room
    {
        public enum Layer
        {
            Background,
            Player,
            Foreground
        }

        public const int WidthInTiles = 25;
        public const int HeightInTiles = 17;

        public Point Size { get { return TileMap.SizeInPixels; } }

        public TileMap TileMap;
        IList<Entity> entities = new List<Entity>();
        Diver diver;
        Texture2D glowTexture;

        public Diver Diver
        {
            set
            {
                entities.Remove(diver);
                diver = value;
                entities.Add(diver);
            }

            get { return diver; }
        }

        public Room(SpriteGrid tileSet)
        {
            TileMap = new TileMap(tileSet, WidthInTiles, HeightInTiles);
            glowTexture = DiverGame.DefaultContent.Load<Texture2D>("glow");
        }

        public static Room FromFile(string filename, SpriteGrid tileSet)
        {
            Room room = new Room(tileSet);

            using (TextReader r = new StreamReader(filename))
            {
                room.TileMap.Load(r);

                IList<string> lines = new List<string>();
                
                foreach(string line in r.ReadToEnd().Split("\n".ToCharArray()))
                {
                    string linet = line.Trim(" \n\r\t".ToCharArray());
                    if (linet.Length > 0 && !linet.StartsWith("//"))
                    {
                        System.Console.WriteLine(linet);
                        lines.Add(linet);
                    }
                }

                room.LoadEntities(lines);
            }

            return room;
        }

        void LoadEntities(IList<string> lines)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using DB.DoF.Entities;");
            sb.AppendLine("namespace DB.DoF {");
            sb.AppendLine("public class CSCodeEvaler {");
            sb.AppendLine("public void InsertEntities(IList<Entity> entities) {");

            foreach (string line in lines)
            {
                sb.AppendLine("entities.Add(new " + line + ");");
            }

            sb.AppendLine("}}}");

            CodeDomProvider cdp = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                compilerParameters.ReferencedAssemblies.Add(asm.Location);
            }

            compilerParameters.GenerateInMemory = true;

            CompilerResults cr = cdp.CompileAssemblyFromSource(compilerParameters, sb.ToString());
            if (cr.Errors.Count > 0)
            {
                throw new Exception(cr.Errors[0].ErrorText);
            }

            Assembly a = cr.CompiledAssembly;
            object o = a.CreateInstance("DB.DoF.CSCodeEvaler");

            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("InsertEntities");

            mi.Invoke(o, new object[] { entities });
            
        }

        public void Draw(Gui.Graphics g, GameTime gt)
        {
            foreach (Layer layer in Enum.GetValues(typeof(Layer)))
            {
                if (layer == Layer.Player)
                {
                    diver.Draw(g, gt, layer);
                    TileMap.Draw(g);
                }

                foreach (Entity entity in entities)
                {
                    if (entity != diver)
                        entity.Draw(g, gt, layer);
                }
            }

            /*
            g.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            g.GraphicsDevice.RenderState.SourceBlend = Blend.DestinationColor;
            g.GraphicsDevice.RenderState.DestinationBlend = Blend.Zero;
            g.Draw(glowTexture, new Rectangle(-300, -300, 400 + 600, 300 + 600), new Color(255,255,255,128));
            g.End();
            */
        }

        public void Update(State s)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(s, this);

                if (OnLeftRoom != null
                    && (IsEntityLeftOfRoom(entity)
                    || IsEntityRightOfRoom(entity)
                    || IsEntityAboveRoom(entity)
                    || IsEntityBelowRoom(entity)))
                {
                    OnLeftRoom(entity);
                }
            }
        }

        public IList<Entity> GetCollidingEntities(Entity entity)
        {
            return GetCollidingEntities<Entity>(entity);
        }


        public IList<T> GetCollidingEntities<T>(Entity entity) where T : Entity
        {
            IList<T> result = new List<T>();

            foreach (Entity e in entities)
            {
                if (typeof(T).IsAssignableFrom(e.GetType()) && e.Dimension.Intersects(entity.Dimension))
                {
                    result.Add((T)e);
                }
            }

            return result;
        }

        public bool IsEntityLeftOfRoom(Entity entity)
        {
            return entity.X + entity.Width < 0;
        }

        public bool IsEntityRightOfRoom(Entity entity)
        {
            return entity.X > Size.X;
        }

        public bool IsEntityAboveRoom(Entity entity)
        {
            return entity.Y + entity.Height < 0;
        }

        public bool IsEntityBelowRoom(Entity entity)
        {
            return entity.Y > Size.Y;
        }

        public bool IsUpdateNeeded()
        {
            return false;
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public delegate void LeftRoomHandler(Entity entity);
        public event LeftRoomHandler OnLeftRoom;
    }
}
