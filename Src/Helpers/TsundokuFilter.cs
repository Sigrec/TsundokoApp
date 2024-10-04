namespace Tsundoku.Helpers
{
    public enum TsundokuFilter
    {
        [StringValue("Ongoing")] Ongoing,
        [StringValue("Finished")] Finished,
        [StringValue("Hiatus")] Hiatus,
        [StringValue("Cancelled")] Cancelled,
        [StringValue("Complete")] Complete,
        [StringValue("Incomplete")] Incomplete,
        [StringValue("Favorites")] Favorites,
        [StringValue("Manga")] Manga,
        [StringValue("Novel")] Novel,
        [StringValue("Shounen")] Shounen,
        [StringValue("Shoujo")] Shoujo,
        [StringValue("Seinen")] Seinen,
        [StringValue("Josei")] Josei,
        [StringValue("Publisher")] Publisher,
        [StringValue("Read")] Read,
        [StringValue("Unread")] Unread,
        [StringValue("Rating")] Rating,
        [StringValue("Value")] Value,
        [StringValue("Query")] Query,
        [StringValue("None")] None,
    }
}