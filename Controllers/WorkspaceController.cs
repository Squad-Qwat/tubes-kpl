using Microsoft.AspNetCore.Mvc;
using PaperNest_API.Models;
using PaperNest_API.Services;

namespace PaperNest_API.Controllers
{
    [ApiController, Route("api/workspaces")]
    public class WorkspaceController : Controller
    {
        [HttpGet]
        public IActionResult GetAllWorkspaces()
        {
            var workspaces = WorkspaceService.GetAll();

            return Ok(new
            {
                message = "Berhasil mendapatkan semua workspace",
                data = workspaces
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetWorkspaceById(Guid id)
        {
            var workspace = WorkspaceService.GetById(id);

            if (workspace == null)
            {
                return NotFound(new
                {
                    message = "Workspace tidak ditemukan"
                });
            }

            return Ok(new
            {
                message = "Berhasil mendapatkan data workspace",
                data = workspace
            });
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetWorkspacesByUserId(Guid userId)
        {
            var workspaces = WorkspaceService.GetByUserId(userId);

            return Ok(new
            {
                message = "Berhasil mendapatkan workspace pengguna",
                data = workspaces
            });
        }

        [HttpPost]
        public IActionResult CreateWorkspace([FromBody] Workspace workspace)
        {
            workspace.Updated_at = DateTime.Now;
            WorkspaceService.Create(workspace);

            return CreatedAtAction(nameof(GetWorkspaceById), new { id = workspace.Id }, new
            {
                message = "Berhasil membuat workspace baru",
                data = workspace
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWorkspace(Guid id, [FromBody] Workspace workspace)
        {
            var existingWorkspace = WorkspaceService.GetById(id);

            if (existingWorkspace == null)
            {
                return NotFound(new
                {
                    message = "Workspace tidak ditemukan"
                });
            }

            WorkspaceService.Update(id, workspace);

            return Ok(new
            {
                message = "Workspace berhasil diperbarui",
                data = WorkspaceService.GetById(id)
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWorkspace(Guid id)
        {
            var existingWorkspace = WorkspaceService.GetById(id);

            if (existingWorkspace == null)
            {
                return NotFound(new
                {
                    message = "Workspace tidak ditemukan"
                });
            }

            WorkspaceService.Delete(id);

            return Ok(new
            {
                message = "Workspace berhasil dihapus"
            });
        }
    }
}
