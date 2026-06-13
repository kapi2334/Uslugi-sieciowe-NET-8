using BlogCMS.Interfaces;
using BlogCMS.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IRepository<Post> _loginRepository;

    public PostsController(IRepository<Post> _loginRepository)
    {
        _loginRepository = _loginRepository;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _loginRepository.GetAllAsync();
        return Ok(posts);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var post = await _loginRepository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    // POST: api/posts
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePost([FromBody] Post post)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newPostId = await _loginRepository.AddAsync(post);
        return CreatedAtAction(nameof(GetPostById), new { id = newPostId }, post);
    }

    // PUT: api/posts/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] Post post)
    {
        if (id != post.Id)
        {
            return BadRequest();
        }

        var result = await _loginRepository.UpdateAsync(post);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    // DELETE: api/posts/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _loginRepository.GetByIdAsync(id);
        var result = await _loginRepository.DeleteAsync(post);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}