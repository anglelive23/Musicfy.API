﻿namespace Musicfy.API.Entities.DataModel
{
    public class MusicfyEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Artist>("Artists");

            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }
    }
}
