namespace PaginationHelper
{
    /// <summary>
    /// A wrapper around the requested data and it's related metadata
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Envelope<T> : IEnvelope
    {
        /// <summary>
        /// The actual data being returned
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Any errors about request
        /// </summary>
        public object Error { get; set; }

        /// <summary>
        /// Extra information about the response
        /// </summary>
        public Meta Meta { get; set; }
    }
}
