namespace Musicfy.API.Entities.DataModel
{
    public class MusicfyEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();

            return builder.GetEdmModel();
        }
    }
}
