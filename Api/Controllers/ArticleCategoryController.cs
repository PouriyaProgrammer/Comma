using static Application.ApiTools.ApiResponse;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.ApiTools;
using Microsoft.AspNetCore.Authorization;
using Application.Roles;
using Application.DTOs.ArticleCategory;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleCategoryController : ControllerBase
    {
        private readonly IArticleCategoryService _service;

        public ArticleCategoryController(IArticleCategoryService service)
        {
            _service = service;
        }

        [HttpGet("GetAll/{skip}/{take}")]
        public async Task<IActionResult> GetAll(int skip, int take)
        {
            var res = await _service.GetAll(skip, take);

            return res.Status switch
            {
                ArticleCategoryStatus.Success => Ok(Success("عملیات با موفقیت انجام شد", res.Result)),
                ArticleCategoryStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _service.Get(id);

            return res.Status switch
            {
                ArticleCategoryStatus.Success => Ok(Success("عملیات با موفقیت انجام شد", res.Result)),
                ArticleCategoryStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }

        [HttpGet("Get/{slug}")]
        public async Task<IActionResult> Get(string slug)
        {
            var res = await _service.GetBySlug(slug);

            return res.Status switch
            {
                ArticleCategoryStatus.Success => Ok(Success("عملیات با موفقیت انجام شد", res.Result)),
                ArticleCategoryStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create([FromForm]CreateArticleCategory model)
        {
            var res = await _service.Create(model);

            return res switch
            {
                CreateArticleCategoryStatus.Success => Ok(Success("عملیات با موفقیت انجام شد", new { })),
                CreateArticleCategoryStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد")),
                CreateArticleCategoryStatus.PictureNotSave => BadRequest(Excetpion("مشکلی در ذخیره عکس پیش آمد")),
                CreateArticleCategoryStatus.CategoryIsExists => BadRequest(Excetpion("این دسته بندی از قبل در سیستم وجود دارد"))
            };
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Edit([FromForm]EditArticleCategory model)
        {
            var res = await _service.Edit(model);

            return res switch
            {
                EditArticleCategoryStatus.Success => Ok(Success("عملیات با موفقیت انجام شد", new { })),
                EditArticleCategoryStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد")),
                EditArticleCategoryStatus.PictureNotSave => BadRequest(Excetpion("مشکلی در ذخیره عکس پیش آمد")),
                EditArticleCategoryStatus.DuplicatedCategory => BadRequest(Excetpion("این دسته بندی از قبل در سیستم وجود دارد")),
                EditArticleCategoryStatus.CategoryNotFound => BadRequest(NotFound())
            };
        }

        [HttpGet("GetAllKeywords")]
        public async Task<List<string>> GetAllKeywords()
        {
            return await _service.GetAllKeywords();
        }

        [HttpGet]
        [Authorize]
        [Route("Search")]
        public async Task<IActionResult> Search(ArticleCategorySearchDto searchDto)
        {
            var res = await _service.Search(searchDto);

            return res.Status switch
            {
                ArticleCategoryStatus.Success => Ok(Success("عملیات با موفقیت انجام شد", res.Result)),
                ArticleCategoryStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }
    }
}
