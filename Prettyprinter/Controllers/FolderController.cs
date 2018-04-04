﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prettyprinter.DAL;
using Prettyprinter.Models;

namespace Prettyprinter.Controllers
{
    public class FolderController : Controller
    {
        public FolderGateway folderGateway;
        public ApplicationDbContext applicationDbContext;
        //private readonly ApplicationDbContext _context;
        private static String serverPath = @"2107 File Server\";
        private static String currentUserID = "161616";
        public FolderController(ApplicationDbContext context)
        {
            folderGateway = new FolderGateway(context);
            applicationDbContext = context;

            if (!System.IO.File.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }
            //if (!System.IO.File.Exists(serverPath + HttpContext.Session.GetString("currentUserID"))){
            //    Directory.CreateDirectory(serverPath + HttpContext.Session.GetString("currentUserID"));
            //}
            if (!System.IO.File.Exists(serverPath + currentUserID))
            {
                Directory.CreateDirectory(serverPath + currentUserID);
            }
        }

        // GET: Folder
        public ActionResult Index(String param,String id)
        {
            var path = "";
            //System.Diagnostics.Debug.WriteLine("*** HAHA"+ HttpContext.Request.Path.ToString(), "HAHA");
            if (String.IsNullOrEmpty(param))
            {
                if (HttpContext.Session.GetString("Path") == null)
                    HttpContext.Session.SetString("Path", currentUserID);
                else
                    HttpContext.Session.SetString("Path", currentUserID);

            }
            else
            {
                //Append the SESSION PATH
                var currentPath = HttpContext.Session.GetString("Path") + "/" + param;
                HttpContext.Session.SetString("Path", currentPath);
               
            }
            path = HttpContext.Session.GetString("Path");
            ViewBag.Path = path;

            return View(folderGateway.SelectAll(path, "ParentID"));

        }


        // POST: Folder/Create
        public ActionResult Create(string folderName,String creationPath)
        {
            string parentId;

            //This is the part im abit confused
            //System.Diagnostics.Debug.WriteLine("*** HAHA" + theParentID, "HAHA");
            if (!String.IsNullOrEmpty(creationPath)) {
                parentId = creationPath;
            }
            else
            {
                //parentId = HttpContext.Session.GetString("Path");
                parentId = currentUserID;
            }

            int type = Folder.TYPE;
            string name = folderName;
            string id = Guid.NewGuid().ToString();
            DateTime dateNow = DateTime.Now;

            AccessControl accessControl = new AccessControl(
                Guid.NewGuid().ToString(),
                id,
                currentUserID,
                true,
                true);

            List<AccessControl> accessControls = new List<AccessControl>();
            accessControls.Add(accessControl);

            Metadata metadata = new Metadata(id, currentUserID, folderName, Folder.TYPE, dateNow, "", parentId, accessControls);

            Folder folder = new Folder();
            folder._id = id;
            folder.parentId = parentId;
            folder.type = Folder.TYPE;
            folder.name = name;
            folder.accessControl = new string[4];
            folder.date = dateNow;

            folderGateway.CreateFile(folder);

            new MetadataController(applicationDbContext).AddMetadata(metadata);

            //Create a real folder locally in file server
            FileStorageGateway.createFolder(parentId, id);
          
            return RedirectToAction(nameof(Index));
        }

        // POST: Folder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string deleteId)
        {
            folderGateway.DeleteFile(deleteId);
            //Delete the file locally from file server
            FileStorageGateway.deleteFile(HttpContext.Session.GetString("serverPath"), deleteId);

            //deleteFile(HttpContext.Session.GetString("serverPath"), deleteId);
            return RedirectToAction(nameof(Index));
        }

        private bool FolderExists(string id)
        {
            return (folderGateway.SelectById(id) != null);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Move(string moveId, string movePath)
        {
            folderGateway.MoveFile(moveId, movePath);
            FileStorageGateway.moveFile(HttpContext.Session.GetString("serverPath"), movePath, moveId);

            return RedirectToAction(nameof(Index));
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(string copyId, string copyPath)
        {
            string createdId = folderGateway.CopyFile(copyId, copyPath);
            FileStorageGateway.copyFile(HttpContext.Session.GetString("serverPath"),copyPath,copyId);
            return RedirectToAction(nameof(Index));
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rename(string renameId, string newName)
        {
            
            folderGateway.RenameFile(renameId, newName);
            return RedirectToAction(nameof(Index));
        }
        

    }
}
