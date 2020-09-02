namespace TeamABootcampAplication.Models
{
    #pragma warning disable SA1600 // Elements should be documented

    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
    #pragma warning restore SA1600 // Elements should be documented
}