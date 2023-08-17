using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Application.DTOs.Datatable
{
    public class DatatableQuery
    {
        private readonly IReadOnlyDictionary<string, string> _dictionary;



        public DatatableQuery(IReadOnlyDictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
        }



        public string? this[string key] =>
            _dictionary.ContainsKey(key) ? _dictionary[key] : null;

        public string this[string key, string defaultValue] =>
            _dictionary.ContainsKey(key) ? _dictionary[key] : defaultValue;



        public async Task<T> Get<T>(string key, Func<Task<T>> defaultValueAction, Func<string, T> map) =>
            _dictionary.ContainsKey(key) ? map(_dictionary[key]) : await defaultValueAction();
    }
}