namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class JsonResponse<T>
    {
        public string JsonRpc { get; set; }

        public T Result { get; set; }

        public BetfairApiException Error { get; set; }

        public object Id { get; set; }

        public bool HasError
        {
            get { return Error != null; }
        }
    }
}
