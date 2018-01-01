using System.Collections.Generic;
using System.Linq;
using Bialecki.Data.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ServiceBusExamples.MessagesSender.Web.Controllers
{
    public class FilesAndFoldersController : Controller
    {
        [HttpGet]
        public IQueryable<Folder> List()
        {
                var folder = new FoldersAndFilesProvider().GetTreeStructure(100, 10);

                return new List<Folder>{ folder }.AsQueryable();
        }
    }
}