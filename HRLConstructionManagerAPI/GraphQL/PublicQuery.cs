using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Services;
using HotChocolate.Authorization;

namespace HRLConstructionManagerAPI.GraphQL;
public class PublicQuery
{
    public string HealthCheck()
    {
        return "API Running";
    }
}
