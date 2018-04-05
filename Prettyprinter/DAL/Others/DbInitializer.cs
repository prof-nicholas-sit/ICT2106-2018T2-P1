﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prettyprinter.Models;

namespace Prettyprinter.DAL
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.File.Any())
            {
                var file = new File { };
                context.File.Add(file);
                context.SaveChanges();
            }
            if (!context.Folder.Any())
            {
                var folder = new Folder { };
                context.Folder.Add(folder);
                context.SaveChanges();
            }
        }
    }
}