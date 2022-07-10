namespace Application.ApiTools
{
    public record class ApiResult(bool status, short code, string title, object result);

    public static class ApiResponse
    {
        public static ApiResult Success(string title, object result)
            => new(true, 200, title, result);

        public static ApiResult Excetpion(string title)
            => new(false, 500, title, new { });

        public static ApiResult NotFound(string title)
            => new(false, 404, title, new { });

        public static ApiResult AccessDenied(string title)
            => new(false, 403, title, new { });
    }
}
