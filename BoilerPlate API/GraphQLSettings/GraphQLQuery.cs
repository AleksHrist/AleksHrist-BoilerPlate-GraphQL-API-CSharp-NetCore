using Newtonsoft.Json.Linq;

namespace BoilerPlate_API.GraphQLSettings
{
    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}

