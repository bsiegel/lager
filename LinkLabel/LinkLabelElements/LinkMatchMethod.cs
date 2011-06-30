namespace CompletIT.Windows.Controls.LinkLabelElements
{
    public enum LinkMatchMethod
    {
        /// <summary>
        /// Match links by LinkPattern
        /// </summary>
        ByLinkPattern,

        /// <summary>
        /// Match only valid URIs
        /// </summary>
        ByUriPattern,

        /// <summary>
        /// Match valid URIs and links by LinkPattern
        /// </summary>
        ByUriAndLinkPattern
    }
}
