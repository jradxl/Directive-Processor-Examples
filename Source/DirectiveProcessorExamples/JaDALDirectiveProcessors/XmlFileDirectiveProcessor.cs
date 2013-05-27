using Microsoft.VisualStudio.TextTemplating;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BenjaminSchroeter.Dsl.DirectiveProcessors
{
    public class XmlFileDirectiveProcessor : FileDirectiveProcessorBase
    {
        protected override string DIRECTIVE_NAME
        {
            get { return "XmlFile"; }
        }

        protected override string CreateInjectingCode(IDictionary<string, string> arguments)
        {
            string filename;
            StringBuilder sb = new StringBuilder();

            //When the Solution first opens, templateFile is null.
            //I wonder if that is a Bug!
            if (this.templateFile != null)
            {
                //Arguments are passed with the Key in lowercase.
                if (!arguments.TryGetValue("filename", out filename))
                    throw new DirectiveProcessorException("Required argument 'FileName' not specified.");

                if (string.IsNullOrEmpty(filename))
                    throw new DirectiveProcessorException("argument 'FileName' is null or empty.");

                string fullPath = Path.Combine(Path.GetDirectoryName(this.templateFile), filename);

                sb.AppendLine("private XDocument _xmlFile;");
                sb.AppendLine("private XDocument XmlFile");
                sb.AppendLine("{");
                sb.AppendLine("   get");
                sb.AppendLine("   {");
                sb.AppendLine("      if (_xmlFile == null)");
                sb.AppendLine("         _xmlFile = XDocument.Load(@\"" + fullPath + "\");");
                sb.AppendLine("      return _xmlFile;");
                sb.AppendLine("   }");
                sb.AppendLine("}");

                sb.AppendLine("private string XmlFileName");
                sb.AppendLine("{");
                sb.AppendLine("   get");
                sb.AppendLine("   {");
                sb.AppendLine("      return @\"" + fullPath + "\";");
                sb.AppendLine("   }");
                sb.AppendLine("}");
            }
            return sb.ToString();
        }
    }
}