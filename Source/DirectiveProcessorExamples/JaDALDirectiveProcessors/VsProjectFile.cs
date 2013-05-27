using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BenjaminSchroeter.Dsl.DirectiveProcessors
{
    public class VsProjectFile
    {
        public XDocument Document { get; private set; }
        public string FileName { get; private set; }

        public VsProjectFile(XDocument doc)
        {
            this.Document = doc;
        }

        public VsProjectFile(string fileName)
        {
            this.Document = XDocument.Load(fileName);
            this.FileName = fileName;
        }

        public string[] GetAllFiles()
        {
            var ret = new string[] { };

            if (FileName.EndsWith(".vcproj", StringComparison.InvariantCultureIgnoreCase))
            {
                ret = VcprojGetAllFiles();
            }

            if (FileName.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
            {
                ret = CsprojGetAllFiles();
            }

            return ret;
        }

        public string[] CsprojGetAllFiles()
        {
            List<string> files = new List<string>();
            if (Document.Root != null)
            {
                foreach (XElement itemGroup in Document.Root.Elements().Where(x => x.Name.LocalName == "ItemGroup"))
                {
                    foreach (XElement item in itemGroup.Elements())
                    {
                        if ((item.Name.LocalName == "Compile"
                             || item.Name.LocalName == "None")
                             && item.Attribute("Include") != null)
                        {
                            string file = item.Attribute("Include").Value;
                            files.Add(file);
                        }
                    }
                }

            }

            return files.ToArray();
        }

        public string[] VcprojGetAllFiles()
        {
            if (Document.Root != null)
            {
                XElement root = Document.Root.Element("Files");

                List<string> files = new List<string>();

                if (root != null)
                    VcprojFindFiles(root, files);

                return files.ToArray();
            }

            return new string[] { };
        }

        private static void VcprojFindFiles(XElement root, List<string> files)
        {
            foreach (XElement file in root.Elements("File"))
            {
                XAttribute att = file.Attribute("RelativePath");
                if (att != null)
                    files.Add(att.Value);

                VcprojFindFiles(file, files);
            }

            foreach (XElement filter in root.Elements("Filter"))
                VcprojFindFiles(filter, files);
        }
    }
}
