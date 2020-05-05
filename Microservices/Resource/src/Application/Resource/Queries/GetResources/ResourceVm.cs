using System.Collections.Generic;
using Resource.Application.TodoLists.Queries.GetTodos;

namespace Resource.Application.Resource.Queries.GetResources
{
    public class ResourceVm
    {
        public IList<ResourceDto> List { get; set; }
    }
}