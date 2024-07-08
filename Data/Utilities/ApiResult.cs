namespace Data.Utilities
{
    public class ApiResult<TEntity>
    {
        private string getByIdSuccess;
        private object entity;

        public bool Success { get; set; }
        public string Message { get; set; }
        public TEntity Data { get; set; }

        public ApiResult(bool success, string message, TEntity data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public ApiResult(string message, TEntity data)
        {
            Success = data != null;
            Message = message;
            Data = data;
        }

        public ApiResult(string getByIdSuccess, object entity)
        {
            this.getByIdSuccess = getByIdSuccess;
            this.entity = entity;
        }
        //public ApiResult(string getByIdError, object entity)
        //{
        //    this.getByIdError= getByIdError;
        //    this.entity = entity;
        //}
    }
}
