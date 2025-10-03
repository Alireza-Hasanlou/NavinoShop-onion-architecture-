namespace Site.Application.Contract.SliderService.Command
{
    public class EditSliderCommandModel : CreateSliderCommandModel
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
    }
}
