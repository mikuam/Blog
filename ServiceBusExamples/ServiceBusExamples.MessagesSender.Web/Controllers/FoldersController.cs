using System.Linq;
using Bialecki.Data.Dto;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace ServiceBusExamples.MessagesSender.Web.Controllers
{
    public class FoldersController : ODataController
    {
        [Produces("application/json")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Folder> Get()
        {
                var folder = new FoldersAndFilesProvider().GetTreeStructure(100, 4);

                return folder.Folders.AsQueryable();
        }
    }
}