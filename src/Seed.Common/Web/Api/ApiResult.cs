namespace Seed.Common.Web.Api
{
    public class ApiResult : IApiResult
    {
        // C'tors
        private ApiResult(object data, object meta, object error)
        {
            Data = data;
            Meta = meta;
            Error = error;
        }

        // Properties
        public object Data { get; }

        public object Meta { get; }
        public object Error { get; }

        // Static Factory Members
        public static ApiResult From(object data, object meta = null)
        {
            return new ApiResult(data, meta ?? new object(), null);
        }

        public static ApiResult FromError(object error)
        {
            return new ApiResult(null, null, error);
        }
    }
}