using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OperationMessaging
{
    //***** https://gist.github.com/wcharczuk/2284226
    /// <summary>
    /// Utiltiy to parse routes based on a token format.
    /// </summary>
    /// <remarks>
    /// Casing doesn't matter!
    /// </remarks>
    /// <example>
    /// <code>
    /// var parser = new RouteParser("{protocol}://mydomain.com/{itemCategory}/{itemId}");
    /// var variables = parser.Variables; //should be .Count == 3
    /// var values = parser.ParseRouteInstance("https://mydomain.com/foo/1");
    /// //values = { { "protocol" => "https"}, { "itemCategory" => "foo"}, { "itemId" => "1" } }
    /// </code>
    /// </example>
    [Serializable]
    public class RouteParser
    {
        public RouteParser(string route, char variableStartChar = '{', char variableEndChar = '}')
        {
            RouteFormat = route;
            VariableStartChar = variableStartChar;
            VariableEndChar = variableEndChar;
            ParseRouteFormat();
        }

        private const string RouteTokenPattern = @"[{0}].+?[{1}]"; //the 0 and 1 are used by the string.format function, they are the start and end characters.
        private const string VariableTokenPattern = "(?<{0}>[^,]*)"; //the <>'s denote the group name; this is used for reference for the variables later.

        /// <summary>
        /// This is the route template that values are extracted based on.
        /// </summary>
        /// <value>
        /// A string containing variables denoted by the <c>VariableStartChar</c> and the <c>VariableEndChar</c>
        /// </value>
        public string RouteFormat { get; set; }

        /// <summary>
        /// This is the character that denotes the beginning of a variable name.
        /// </summary>
        public char VariableStartChar { get; set; }

        /// <summary>
        /// This is the character that denotes the end of a variable name.
        /// </summary>
        public char VariableEndChar { get; set; }

        /// <summary>
        /// A hash set of all variable names parsed from the <c>RouteFormat</c>.
        /// </summary>
        public HashSet<string> Variables { get; set; }

        /// <summary>
        /// Initialize the Variables set based on the <c>RouteFormat</c>
        /// </summary>
        public void ParseRouteFormat()
        {
            var variableList = new List<string>();
            var matchCollection = Regex.Matches
                (
                    RouteFormat
                    , string.Format(RouteTokenPattern, VariableStartChar, VariableEndChar)
                    , RegexOptions.IgnoreCase
                );

            foreach (var match in matchCollection)
            {
                variableList.Add(RemoteVariableChars(match.ToString()));
            }
            Variables = new HashSet<string>(variableList);
        }

        /// <summary>
        /// Extract variable values from a given instance of the route you're trying to parse.
        /// </summary>
        /// <param name="routeInstance">The route instance.</param>
        /// <returns>A dictionary of Variable names mapped to values.</returns>
        public OperationResponse ParseRouteInstance(string routeInstance)
        {
            //*****
            var inputValues = new Dictionary<string, string>();
            string formatUrl = new string(RouteFormat.ToArray());
            foreach (string variable in Variables)
            {
                formatUrl = formatUrl.Replace(WrapWithVariableChars(variable), string.Format(VariableTokenPattern, variable));
            }

            //*****
            var regex = new Regex(formatUrl, RegexOptions.IgnoreCase);
            var matchCollection = regex.Match(routeInstance);

            if (!matchCollection.Success)
            {
                return new OperationResponse
                {
                    Succes = false,
                    Result = null,
                    NonSuccessMessage = "Route instance does not match format"
                };
            }

            //*****
            foreach (var variable in Variables)
            {
                var value = matchCollection.Groups[variable].Value;
                inputValues.Add(variable, value);
            }

            //*****
            return new OperationResponse
            {
                Succes = true,
                Result = inputValues
            };
        }

        /// <summary>
        /// Replace a variable in the <c>RouteFormat</c> with a specified value.
        /// </summary>
        /// <param name="variableName">The variable name to replace.</param>
        /// <param name="variableValue">The value to replace with.</param>
        /// <param name="workingRoute">An 'in progress' route that may contain values that have already been replaced.</param>
        /// <returns>A <c>workingRoute</c></returns>
        public String SetVariable(String variableName, String variableValue, String workingRoute = null)
        {
            if (!variableName.StartsWith(VariableStartChar.ToString()) && !variableName.EndsWith(VariableEndChar.ToString()))
                variableName = String.Format("{1}{0}{2}", variableName, VariableStartChar, VariableEndChar);

            if (!String.IsNullOrEmpty(workingRoute))
                return workingRoute.Replace(variableName, variableValue);
            else
                return RouteFormat.Replace(variableName, variableValue);
        }

        #region Private Helper Methods
        private String RemoteVariableChars(String input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return input;

            string result = new String(input.ToArray());
            result = result.Replace(VariableStartChar.ToString(), String.Empty).Replace(VariableEndChar.ToString(), String.Empty);
            return result;
        }

        private String WrapWithVariableChars(String input)
        {
            return String.Format("{0}{1}{2}", VariableStartChar, input, VariableEndChar);
        }
        #endregion
    }
}
