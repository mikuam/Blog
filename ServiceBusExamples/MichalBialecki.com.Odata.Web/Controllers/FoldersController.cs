using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using MichalBialecki.com.Data.Dto;
using Microsoft.Data.OData;

namespace Bialecki.com.Odata.Web.Controllers
{
    public class FoldersController : ApiController
    {
        private IFoldersAndFilesProvider _provider;

        private ODataValidationSettings _validationSettings;

        public FoldersController(IFoldersAndFilesProvider provider)
        {
            _provider = provider;
            _validationSettings = new ODataValidationSettings();
        }

        [Route("odata/Folders")]
        [EnableQuery]
        public IQueryable<Folder> GetFolders()
        {
            var folders = _provider.GetFolders();
            return folders.AsQueryable();
        }

        // GET: odata/Folders(5)
        public IHttpActionResult GetFolder([FromODataUri] string key, ODataQueryOptions<Folder> queryOptions)
        {
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(_provider.GetFolders().FirstOrDefault(f => f.Name.Contains(key)));
        }

        // PUT: odata/Folders(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Folder> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(folder);

            // TODO: Save the patched entity.

            // return Updated(folder);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Folders
        public IHttpActionResult Post(Folder folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(folder);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Folders(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Folder> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(folder);

            // TODO: Save the patched entity.

            // return Updated(folder);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Folders(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
