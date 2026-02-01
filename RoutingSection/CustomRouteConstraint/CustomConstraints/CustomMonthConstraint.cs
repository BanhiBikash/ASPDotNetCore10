
using System.Text.RegularExpressions;

namespace CustomRouteConstraint.CustomConstraints
{
    public class CustomMonthConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            //
            if(!values.ContainsKey(routeKey))
            {
                return false; 
            }

            //taking the month value given
            string? month = values[routeKey].ToString();

            //allowed values
            Regex monthData = new Regex($"^(jan|apr|aug|dec)$");


            //checking if month value is valid
            if (monthData.IsMatch(month))
            {
                return true;
            }

            return false; ;
        }
    }
}
