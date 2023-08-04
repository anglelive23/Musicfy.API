using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

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
