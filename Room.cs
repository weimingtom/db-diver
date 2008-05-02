using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace DB.Diver
{
    class Room
    {
        public const int WidthInTiles = 20;
        public const int HeightInTiles = 15;

        public TileMap TileMap;
        IList<Object> entities = new List<Object>();

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("namespace DB.Diver{");
            sb.AppendLine("public class CSCodeEvaler{");
            sb.AppendLine("public void InsertEntities(IList<Object> entities){");
            foreach(string line in lines)
            {
                sb.AppendLine("entities.Add(new " + line + ");");
            }            
            sb.AppendLine("}}}");

            CodeDomProvider cdp = new CSharpCodeProvider();
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.ReferencedAssemblies.Add("system.dll");
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

            mi.Invoke(o, new object[] { entities });
        }

        public void Draw(Gui.Graphics g)
        {
            TileMap.Draw(g);
        }
    }
}
