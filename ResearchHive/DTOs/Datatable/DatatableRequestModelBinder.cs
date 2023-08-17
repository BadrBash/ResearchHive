using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Application.DTOs.Datatable
{
    public class DatatableRequestModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var dictionary = bindingContext.HttpContext.Request.Query.Where(pair => pair.Key.StartsWith("query["))
                                           .Select(pair => new
                                            {
                                                key = pair.Key.Substring(6,
                                                                         pair.Key.Length - 7),
                                                value = pair.Value.ToString()
                                            })
                                           .ToDictionary(p => p.key,
                                                         p => p.value);

            var query = new DatatableQuery(dictionary);

            // these are required
            var rawPage = bindingContext.ValueProvider.GetValue("pagination[page]");
            var rawPages = bindingContext.ValueProvider.GetValue("pagination[pages]");
            var rawPerPage = bindingContext.ValueProvider.GetValue("pagination[perpage]");
            var rawTotal = bindingContext.ValueProvider.GetValue("pagination[total]");

            // optional
            var rawField = bindingContext.ValueProvider.GetValue("pagination[field]");
            var rawSort = bindingContext.ValueProvider.GetValue("pagination[sort]");
            var rawSortSort = bindingContext.ValueProvider.GetValue("sort[sort]");
            var rawSortField = bindingContext.ValueProvider.GetValue("sort[field]");

            if (string.IsNullOrEmpty(rawPage.FirstValue) ||
                string.IsNullOrEmpty(rawPerPage.FirstValue))
            {
                return Task.CompletedTask;
            }

            var pages = string.IsNullOrEmpty(rawPages.FirstValue) ? "1" : rawPages.FirstValue;
            var total = string.IsNullOrEmpty(rawTotal.FirstValue) ? "0" : rawTotal.FirstValue;
            var model = new DatatableRequest
            {
                Pagination = new DatatablePagination
                {
                    Page = int.Parse(rawPage.FirstValue ?? "1"),
                    Pages = int.Parse(pages),
                    PerPage = int.Parse(rawPerPage.FirstValue ?? "10"),
                    Total = int.Parse(total),
                    Field = rawField.FirstValue
                },
                Query = query,
                Sort = new DatatableSort
                {
                    Field = rawSortField.FirstValue
                }
            };


            // now, we need the to fix the sort field
            if (rawSort.FirstValue != null &&
                Enum.TryParse(rawSort.FirstValue,
                              true,
                              out DatatableSortOption sort))
            {
                model.Pagination.Sort = sort;
            }

            if (rawSortSort.FirstValue != null &&
                Enum.TryParse(rawSortSort.FirstValue,
                              true,
                              out DatatableSortOption sortSort))
            {
                model.Pagination.Sort = sortSort;
            }

            // need to debug this to get the actual structure that gets here.
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}