using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

using Microsoft.Xna.Framework;

namespace DB.Diver
{

    public class Room
    {
        public enum Layer
        {
            Background,
            Player,
            Foreground
        }

        public const int WidthInTiles = 20;
        public const int HeightInTiles = 15;

        public TileMap TileMap;
        IList<Entity> entities = new List<Entity>();
        Diver diver;

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
            foreach (string line in lines)
            {
                string[] splitted = line.Trim().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string className = splitted[0];
                Type type = Type.GetType("DB.Diver." + className, true, false);

                if (splitted.Length == 1)
                {
                    ConstructorInfo c = type.GetConstructor(new Type[] {});
                    entities.Add((Entity)c.Invoke(new object[] {}));
                }
                else if (splitted.Length == 3)
                {
                    string XStr = splitted[1];
                    string YStr = splitted[2];

                    ConstructorInfo c = type.GetConstructor(new Type[] { 0.GetType(), 0.GetType() });
                    entities.Add((Entity)c.Invoke(new object[] { int.Parse(XStr), int.Parse(YStr) }));
                }
                else if (splitted.Length == 4)
                {
                    string XStr = splitted[1];
                    string YStr = splitted[2];
                    string TagStr = splitted[3];

                    ConstructorInfo c = type.GetConstructor(new Type[] { 0.GetType(), 0.GetType(), "".GetType() } );
                    entities.Add((Entity)c.Invoke(new object[] { int.Parse(XStr), int.Parse(YStr), TagStr }));
                }
                else
                {
                    throw new Exception("Error in room file: " + line);
                }
            }
            
            /*
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace DB.Diver{");
            sb.AppendLine("public class CSCodeEvaler{");
            sb.AppendLine("public void InsertEntities(IList<DB.Diver.Entity> entities){");
            foreach(string line in lines)
            {
                sb.AppendLine("entities.Add(new " + line + ");");
            }            
            sb.AppendLine("}}}");

            CodeDomProvider cdp = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.ReferencedAssemblies.Add("system.dll");
            //compilerParameters.ReferencedAssemblies.Add(Assembly.GetAssembly(this.GetType()).GetName().Name);
            //compilerParameters.ReferencedAssemblies.Add("db-diver");
            compilerParameters.CompilerOptions = "/t:library";
            compilerParameters.GenerateInMemory = true;

            CompilerResults cr = cdp.CompileAssemblyFromSource(compilerParameters, sb.ToString());
            if( cr.Errors.Count > 0 ){
              throw new Exception(cr.Errors[0].ErrorText);
            }

            Assembly a = cr.CompiledAssembly;
            object o = a.CreateInstance("DB.Diver.CSCodeEvaler");

            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("InsertEntities");

            mi.Invoke(o, new object[] { entities });*/
        }

        public void Draw(Gui.Graphics g, GameTime gt)
        {
            foreach (Layer layer in Enum.GetValues(typeof(Layer)))
            {
                if(layer == Layer.Player)
                    TileMap.Draw(g);
                foreach (Entity entity in entities)
                {
                    entity.Draw(g, gt, layer);
                }
            }
        }

        public void Update(State s)
        {
            foreach (Entity entity in entities)
            {
                entity.Update(s, this);
            }
        }
    }
}
