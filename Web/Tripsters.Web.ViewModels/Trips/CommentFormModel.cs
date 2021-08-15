namespace Tripsters.Web.ViewModels.Trips
{
    using System.ComponentModel.DataAnnotations;

    public class CommentFormModel
    {
        [Required]
        public string Text { get; set; }

        public string TripId { get; set; }

        public string UserName { get; set; }
    }
}
