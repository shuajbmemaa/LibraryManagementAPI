namespace LMS.Application.Wrappers
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public string Errors { get; set; }
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;

        public static BaseResponse BadRequest(string error)
        {
            return new BaseResponse { Errors = error, Success = false, Status = 400 };
        }

        public static BaseResponse ServerError(string error)
        {
            return new BaseResponse { Errors = error, Success = false, Status = 500 };
        }

        public static BaseResponse NotFound(string error)
        {
            return new BaseResponse { Errors = error, Success = false, Status = 404 };
        }

        public static BaseResponse Ok(string message)
        {
            return new BaseResponse { Message = message };
        }
    }

    public class BaseResponse<T>
    {
        public string Errors { get; set; }
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;
        public T Data { get; set; }

        public static BaseResponse<T> BadRequest(string error)
        {
            return new BaseResponse<T> { Errors = error, Success = false, Status = 400 };
        }

        public static BaseResponse<T> NotFound(string error)
        {
            return new BaseResponse<T> { Errors = error, Success = false, Status = 404 };
        }

        public static BaseResponse<T> Ok(T t)
        {
            return new BaseResponse<T> { Data = t };
        }
    }
}
