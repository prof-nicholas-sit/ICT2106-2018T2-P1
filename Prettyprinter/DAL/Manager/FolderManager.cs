﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Prettyprinter.DAL
{
    public class FolderManager : DocumentManager
    {

        private static String serverPath = @"2107 File Server\";

        //Implementations of Abstract Methods
        public override String getParentOfDocument(String fileID)
        {
            String ParentName = Directory.GetParent(fileID).ToString();
            return ParentName;
        }
        
        public override Boolean createDocument(String path, String fileID)
        {
            String Path = path + fileID;

            String pathToFile = serverPath + path;
            List<String> AllEntries = Directory.GetDirectories(pathToFile).ToList();
            
            foreach (String line in AllEntries)
            {
                String currentFolder = line;
                currentFolder = currentFolder.Replace(pathToFile + @"\", "");
                if (currentFolder.Equals(fileID))
                {
                    return false;
                }
            }   
            Directory.CreateDirectory(pathToFile + @"\" + fileID);
            return true;
        }
        
        public override Boolean deleteDocument(String path, String fileID)
        {
            try
            {
                var dir = new DirectoryInfo(serverPath + path + @"\" + fileID);
                dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                dir.Delete(true);
                return true;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}