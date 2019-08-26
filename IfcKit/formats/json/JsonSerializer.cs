// Name:        JsonSerializer.cs
// Description: JSON serializer
// Author:      Tim Chipman
// Origination: Work performed for BuildingSmart by Constructivity.com LLC.
// Copyright:   (c) 2017 BuildingSmart International Ltd.
// License:     http://www.buildingsmart-tech.org/legal

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BuildingSmart.Serialization.Xml;

namespace BuildingSmart.Serialization.Json
{
    public class JsonSerializer : XmlSerializer
    {
        public JsonSerializer(Type type) : base(type)
        {
        }

        /// <summary>
        /// Terminates the opening tag, to allow for sub-elements to be written
        /// </summary>
        protected override void WriteOpenElement(StreamWriter writer)
        {
            // do nothing
        }
        protected override void WriteOpenElement(StreamWriter writer, bool newLine)
        {
            //do nothing
        }
        /// <summary>
        /// Terminates the opening tag, with no subelements
        /// </summary>
        protected override void WriteCloseElementEntity(StreamWriter writer, ref int indent)
        {
            indent--;
            this.WriteIndent(writer, indent);
            writer.WriteLine("}");
        }

        protected override void WriteCloseElementAttribute(StreamWriter writer, ref int indent)
        {
            //   do   nothing
        }

        protected override void WriteEntityStart(StreamWriter writer, ref int indent)
        {
            this.WriteIndent(writer, indent);
            writer.WriteLine("{");
            indent++;
        }

        protected override void WriteEntityEnd(StreamWriter writer, ref int indent)
        {
            indent--;
            this.WriteIndent(writer, indent);
            writer.WriteLine("}");
        }

        protected override void WriteStartElementEntity(StreamWriter writer, ref int indent, string name)
        {
            this.WriteIndent(writer, indent);
            writer.WriteLine("{");
            indent++;
            this.WriteType(writer, indent, this.stringToJson(name));
        }

        protected override void WriteStartElementAttribute(StreamWriter writer, ref int indent, string name)
        {
            this.WriteIndent(writer, indent);
            //多了回车符
            //writer.WriteLine("\"" + name + "\": ");
            writer.Write("\"" + this.stringToJson(name) + "\": ");
        }

        protected override void WriteEndElementEntity(StreamWriter writer, ref int indent, string name)
        {
            indent--;
            this.WriteIndent(writer, indent);
            writer.WriteLine("}");
        }

        protected override void WriteEndElementAttribute(StreamWriter writer, ref int indent, string name)
        {
            // do nothing
        }

        protected override void WriteIdentifier(StreamWriter writer, int indent, string oid)
        {
            this.WriteIndent(writer, indent);
            writer.Write("\"id\":\" ");
            writer.Write(oid);
            writer.WriteLine("\",");
        }

        protected override void WriteReference(StreamWriter writer, int indent, string oid)
        {
            this.WriteIndent(writer, indent);
            writer.Write("\"href\":\" ");
            writer.Write(oid);
            //writer.WriteLine();
            writer.WriteLine("\"");//WriteLine输完字符后会加上回车和换行
        }

        protected override void WriteType(StreamWriter writer, int indent, string type)
        {
            this.WriteIndent(writer, indent);
            writer.Write("\"type\": \"");
            // Modified by Jifeng, 考虑字符串中回车符
            //writer.Write(type);
            writer.Write(this.stringToJson(type));
            writer.WriteLine("\",");
        }

        protected override void WriteTypedValue(StreamWriter writer, ref int indent, string type, string value)
        {
            this.WriteEntityStart(writer, ref indent);
            // Modified by Jifeng, 考虑字符串中回车符
            //this.WriteType(writer, indent, type);
            this.WriteType(writer, indent, this.stringToJson(type));
            this.WriteIndent(writer, indent);
            //writer.WriteLine("\"value\": \"" + value + "\"");
            writer.WriteLine("\"value\": \"" + this.stringToJson(value) + "\"");
            this.WriteEntityEnd(writer, ref indent);
        }

        protected override void WriteStartAttribute(StreamWriter writer, int indent, string name)
        {
            this.WriteIndent(writer, indent);
            // Modified by Jifeng, 考虑字符串中回车符
            //writer.Write("\"" + name + "\": \"");
            writer.Write("\"" + this.stringToJson(name) + "\": \"");
        }

        protected override void WriteEndAttribute(StreamWriter writer)
        {
            writer.Write("\"");
        }

        protected override void WriteHeader(StreamWriter writer)
        {
            writer.WriteLine("{");
            // 替换此行，indent而不是空格
            // writer.WriteLine("  \"ifc\": [");
            this.WriteIndent(writer, 1);
            writer.WriteLine("\"ifc\": [");


            // 不能确认是否真的要注释，by jifeng
            // writer.WriteLine("  {");
        }

        protected override void WriteFooter(StreamWriter writer)
        {
            // 与前面对应，by jifeng
            //writer.WriteLine("  }");
            writer.WriteLine("  ]");
            writer.WriteLine("}");
        }

        protected override void WriteAttributeDelimiter(StreamWriter writer)
        {
            writer.WriteLine(",");
        }

        protected override void WriteAttributeTerminator(StreamWriter writer)
        {
            writer.WriteLine(); // ensure closing bracket is on next line
        }

        protected override void WriteCollectionDelimiter(StreamWriter writer, int indent)
        {
            this.WriteIndent(writer, indent);
            writer.WriteLine(",");
        }

        protected override void WriteCollectionStart(StreamWriter writer, ref int indent)
        {
            this.WriteIndent(writer, indent);
            writer.WriteLine("[");
            indent++;
        }

        protected override void WriteCollectionEnd(StreamWriter writer, ref int indent)
        {
            writer.WriteLine();
            indent--;
            this.WriteIndent(writer, indent);
            writer.WriteLine("]");
        }

        protected override void WriteRootDelimeter(StreamWriter writer)
        {
            writer.WriteLine(",");
        }

        // JSON字符串中回车符的处理, modified by Jifeng
        protected string stringToJson(string str)
        {
            return str.Replace("\n", "\\n").Replace("\r", "").Replace("\\", "\\\\");
        }
    }
}
