namespace PSL.MicroserviceTemplate.Api.Controllers;

public static class Routes
{
    private const string Root = "api";

    public static class V1
    {
        public const string VersionNumber = "1.0";

        private const string VersionName = "v{version:apiVersion}";
        private const string Base = Root + "/" + VersionName;

        public static class Templates
        {
            public const string Controller = Base + "/templates";

            public const string Get = Controller + "/{templateId:guid}";
        }
    }

}
