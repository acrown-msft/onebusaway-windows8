﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Creates a list of initialization parameters.
    /// </summary>
    public class PageInitializationParameters
    {
        /// <summary>
        /// A dictionary of name value pairs.
        /// </summary>
        private Dictionary<string, object> nameValuePairs;

        /// <summary>
        /// Creates page parameters with no parameters
        /// </summary>
        public PageInitializationParameters()
        {
            this.nameValuePairs = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates page parameters from a specific query string.
        /// </summary>
        public PageInitializationParameters(string queryString)
        {
            this.nameValuePairs = (from argValuePair in queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                                   let pair = argValuePair.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)
                                   where pair.Length == 2
                                   select new
                                   {
                                       Name = pair[0],
                                       Value = pair[1]
                                   }).ToDictionary(kv => kv.Name, kv => kv.Value as object);
        }

        /// <summary>
        /// Attempts to parse a query string and initialize a parameters object.
        /// </summary>
        public static bool TryCreate(string queryString, out PageInitializationParameters parameters)
        {
            int queryIndex = queryString.IndexOf("?");
            if (queryIndex > -1)
            {
                // Strip out the ? from the string:
                queryString = queryString.Substring(queryIndex + 1);
                parameters = new PageInitializationParameters(queryString);
                return true;
            }

            parameters = null;
            return false;
        }

        /// <summary>
        /// Returns the value of a particular parameter, or 
        /// </summary>
        public T GetParameter<T>(string parameterName)
        {
            T value = default(T);

            object valueObject = null;
            if (this.nameValuePairs.TryGetValue(parameterName, out valueObject))
            {
                value = (T)Convert.ChangeType(valueObject, typeof(T));
            }

            return value;
        }

        /// <summary>
        /// Sets a parameter.
        /// </summary>
        public void SetParameter<T>(string parameterName, T value)
        {
            this.nameValuePairs[parameterName] = value;
        }

        /// <summary>
        /// Converts the parameters to a query string. Includes the question mark at the begining!
        /// </summary>
        public string ToQueryString()
        {
            return "?" + string.Join("&",
                (from parameterName in this.nameValuePairs.Keys
                 select string.Format("{0}={1}", parameterName, this.nameValuePairs[parameterName])));
        }
    }
}
