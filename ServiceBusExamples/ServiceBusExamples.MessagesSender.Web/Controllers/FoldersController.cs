using System.Linq;
using MichalBialecki.com.Data.Dto;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace ServiceBusExamples.MessagesSender.NetCore.Web.Controllers
{
    [Route("Folders")]
    public class FoldersController : ODataController
    {
        [HttpGet]
        [Produces("application/json")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Folder> Get()
        {
                var folder = new FoldersAndFilesProvider().GetTreeStructure(100, 4);

                return folder.Folders.AsQueryable();
        }
    }
}