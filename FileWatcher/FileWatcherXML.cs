using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace FileWatcher
{
    public static class fileXML<T> where T : new()
    { 
        public static T Read_XML(string pathFile, string fileName) {
            string path = "";
            try
            {
                string fullPath = GeneratePath(pathFile, "", fileName, out path);

                T deserializedContent = default;

                if (Directory.Exists(path))
                {
                    string file = null;
                    
                    foreach (string fileInTheRoute in Directory.GetFiles(path))
                    {
                        if (fileInTheRoute == fullPath)
                        {
                            file = fileInTheRoute;
                            break;
                        }
                    }
                    if (new FileInfo(file).Length == 0) 
                    {
                        using (StreamReader streamReader = new StreamReader(file))
                        {
                            XmlSerializer seralizadorXml = new XmlSerializer(typeof(T));
                            deserializedContent = (T)seralizadorXml.Deserialize(streamReader);
                        }
                    }
                    else
                    {
                        throw new FileNotFoundException($"This file name  {fileName} not exist.");
                    }
                }
                return deserializedContent;
            }
            catch (FileNotFoundException e)
            {
                
                throw new FileNotFoundException($"pathFile Non-existent { pathFile}", e);
            }
            catch (Exception e)
            {
                throw new FileNotFoundException($"Error reading text file { path}", e);
            }
        }

        private static string GeneratePath(string pathFile, string subFolder, string fileName, out string path)
        {
            if (!Directory.Exists(pathFile))
            {
                throw new FileNotFoundException("Invalid Path file ");
            }
            StringBuilder sbPath = new StringBuilder();
            sbPath.Append(Path.GetFullPath(pathFile));
            if (subFolder.Trim() != string.Empty)
            {
                sbPath.Append($@"\{subFolder}\");
            }
            else
            {
                sbPath.Append($@"\");
            }
            path = sbPath.ToString();
            sbPath.Append(fileName);
            if (!fileName.ToLower().Contains(".xml"))
            {
                sbPath.Append(".xml");
            }
            return sbPath.ToString();
        }
    }
}
