using AutoMapper;
using CMSBlog.API.Extensions;
using CMSBlog.Core.Domain.Content;
using CMSBlog.Core.Domain.Identity;
using CMSBlog.Core.Helpers;
using CMSBlog.Core.Models;
using CMSBlog.Core.Models.Content;
using CMSBlog.Core.SeedWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static CMSBlog.Core.SeedWorks.Constants.Permissions;


// nhan request tu client
namespace CMSBlog.API.Controllers.AdminApi
{
    [Route("api/admin/post")]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public PostController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Posts.Create)]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.Posts.IsSlugAlreadyExisted(request.Slug))
            {
                return BadRequest("Đã tồn tại slug");
            }
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            var postId = Guid.NewGuid();
            post.Id = postId;
            var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
            post.CategoryName = category.Name;
            post.CategorySlug = category.Slug;

            var userId = User.GetUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            post.AuthorUserId = userId;
            post.AuthorName = user.GetFullName();
            post.AuthorUserName = user.UserName;
            _unitOfWork.Posts.Add(post);


            //Process tag
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tagSlug = TextHelper.ToUnsignedString(tagName);
                    var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
                    Guid tagId;
                    if (tag == null)
                    {
                        tagId = Guid.NewGuid();
                        _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug });

                    }
                    else
                    {
                        tagId = tag.Id;
                    }
                    await _unitOfWork.Posts.AddTagToPost(postId, tagId);
                }
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        [Authorize(Posts.Edit)]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.Posts.IsSlugAlreadyExisted(request.Slug, id))
            {
                return BadRequest("Đã tồn tại slug");
            }

            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
                return NotFound();

            // update category
            if (post.CategoryId != request.CategoryId)
            {
                var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
                post.CategoryName = category.Name;
                post.CategorySlug = category.Slug;
            }

            _mapper.Map(request, post);

            /* =====================================================
               XỬ LÝ TAG
            ===================================================== */

            // Lấy danh sách tag hiện tại của bài viết
            var currentTags = await _unitOfWork.Posts.GetTagsByPostId(id);
            var currentSlugs = currentTags.Select(x => TextHelper.ToUnsignedString(x)).ToList();

            // === TRƯỜNG HỢP 1: FE GỬI MẢNG RỖNG => XÓA HẾT TAG ===
            if (request.Tags != null && request.Tags.Length == 0)
            {
                await _unitOfWork.Posts.RemoveAllTagsOfPost(id);
            }

            // === TRƯỜNG HỢP 2: FE GỬI TAG MỚI => GIỮ CŨ + THÊM MỚI ===
            if (request.Tags != null && request.Tags.Length > 0)
            {
                foreach (var tagName in request.Tags)
                {
                    var tagSlug = TextHelper.ToUnsignedString(tagName);

                    // nếu tag đã tồn tại thì bỏ qua
                    if (currentSlugs.Contains(tagSlug))
                        continue;

                    // kiểm tra xem tag đã có trong bảng Tags chưa
                    var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
                    Guid tagId;

                    if (tag == null)
                    {
                        tagId = Guid.NewGuid();
                        _unitOfWork.Tags.Add(new Tag()
                        {
                            Id = tagId,
                            Name = tagName,
                            Slug = tagSlug
                        });
                    }
                    else
                    {
                        tagId = tag.Id;
                    }

                    await _unitOfWork.Posts.AddTagToPost(id, tagId);
                }
            }

            await _unitOfWork.CompleteAsync();
            return Ok();
        }
        [HttpDelete]
        [Authorize(Posts.Delete)]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.Posts.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpGet]
        [Route("paging")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<PagedResult<PostInListDto>>> GetPostsPaging(
            string? keyword, Guid? categoryId,
            int pageIndex = 1, int pageSize = 10)  // mặc định pageIndex = 1
        {
            // đảm bảo pageIndex và pageSize >= 1
            pageIndex = Math.Max(pageIndex, 1);
            pageSize = Math.Max(pageSize, 1);

            var userId = User.GetUserId();
            var result = await _unitOfWork.Posts.GetAllPaging(keyword, userId, categoryId, pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet]
        [Route("series-belong/{postId}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<SeriesInListDto>>> GetSeriesBelong(Guid postId)
        {
            var result = await _unitOfWork.Posts.GetAllSeries(postId);
            return Ok(result);
        }



        [HttpGet("approve/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<IActionResult> ApprovePost(Guid id)
        {
            await _unitOfWork.Posts.Approve(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("approval-submit/{id}")]
        [Authorize(Posts.Edit)]
        public async Task<IActionResult> SendToApprove(Guid id)
        {
            await _unitOfWork.Posts.SendToApprove(id, User.GetUserId());
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpPost("return-back/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<IActionResult> ReturnBack(Guid id, [FromBody] ReturnBackRequest model)
        {
            await _unitOfWork.Posts.ReturnBack(id, User.GetUserId(), model.Reason);
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

        [HttpGet("return-reason/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<ActionResult<string>> GetReason(Guid id)
        {
            var note = await _unitOfWork.Posts.GetReturnReason(id);
            return Ok(note);
        }

        [HttpGet("activity-logs/{id}")]
        [Authorize(Posts.Approve)]
        public async Task<ActionResult<List<PostActivityLogDto>>> GetActivityLogs(Guid id)
        {
            var logs = await _unitOfWork.Posts.GetActivityLogs(id);
            return Ok(logs);
        }
        [HttpGet("tags")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<string>>> GetAllTags()
        {
            var logs = await _unitOfWork.Posts.GetAllTags();
            return Ok(logs);
        }

        [HttpGet("tags/{postId}")]
        [Authorize(Posts.View)]
        public async Task<ActionResult<List<string>>> GetPostTags(Guid postId)
        {
            var tagNames = await _unitOfWork.Posts.GetTagsByPostId(postId);
            return Ok(tagNames);
        }

    }
}
