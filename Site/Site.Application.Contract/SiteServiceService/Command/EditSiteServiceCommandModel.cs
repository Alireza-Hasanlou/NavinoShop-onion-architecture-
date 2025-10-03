namespace Site.Application.Contract.SiteServiceService.Command
{
    public class EditSiteServiceCommandModel : CreateSiteServiceCommnadModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
    }
}
