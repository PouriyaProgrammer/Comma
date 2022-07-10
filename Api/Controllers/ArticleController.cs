using Application.ApiTools;
using Application.DTOs.Article;
using Application.Roles;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.ApiTools.ApiResponse;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        [Route("GetAll/{skip}/{take}")]
        public async Task<IActionResult> GetAll(int skip, int take)
        {
            var res = await _articleService.GetAll(skip, take);

            return res.Status switch
            {
                ArticleStatus.Success => Ok(Success("عملیات با موفقیت انجام شد.", res.Result)),
                ArticleStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _articleService.Get(id);

            return res.Status switch
            {
                ArticleStatus.Success => Ok(Success("عملیات با موفقیت انجام شد.", res.Result)),
                ArticleStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }

        [HttpGet]
        [Route("GetBySlug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var res = await _articleService.GetBySlug(slug);

            return res.Status switch
            {
                ArticleStatus.Success => Ok(Success("عملیات با موفقیت انجام شد.", res.Result)),
                ArticleStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create([FromForm]CreateArticle article)
        {
            var res = await _articleService.Create(article);

            return res switch
            {
                CreateArticleStatus.Success => Ok(Success("عملیات با موفقیت انجام شد.", new { })),
                CreateArticleStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد")),
                CreateArticleStatus.ArticleIsExsits => BadRequest(Excetpion("امکان ثبت اطلاعات تکراری وجود ندارد")),
                CreateArticleStatus.PictureIsNull => BadRequest("آپلود عکس پست الزامی است."),
                CreateArticleStatus.PictureNotSave => BadRequest(Excetpion("مشکلی در ‌‌ذخیره کردن عکس پیش آمد"))
            };
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create([FromForm]EditArticle article)
        {
            var res = await _articleService.Edit(article);

            return res switch
            {
                EditArticleStatus.Success => Ok(Success("عملیات با موفقیت انجام شد.", new { })),
                EditArticleStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد")),
                EditArticleStatus.DuplicatedArticle => BadRequest(Excetpion("امکان ثبت اطلاعات تکراری وجود ندارد")),
                EditArticleStatus.ArticleIsNotExists => BadRequest(ApiResponse.NotFound("پستی برای ویرایش یافت نشد")),
                EditArticleStatus.PictureNotSave => BadRequest(Excetpion("مشکلی در ‌‌ذخیره کردن عکس پیش آمد")),
                EditArticleStatus.CategoryNotFound => BadRequest(ApiResponse.NotFound("دسته بندی وارد شده پیدا نشد."))
            };
        }

        [HttpGet]
        [Route("GetAllKeywords")]
        public async Task<IActionResult> GetAllKeywords()
        {
            var res = await _articleService.GetAllKeywords();

            return Ok(res);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(ArticleSearchModel searchModel)
        {
            var res = await _articleService.Search(searchModel);

            return res.Status switch
            {
                ArticleStatus.Success => Ok(Success("عملیات با موفقیت انجام شد.", res.Result)),
                ArticleStatus.Error => BadRequest(Excetpion("مشکلی پیش آمد"))
            };
        }
    }
}
