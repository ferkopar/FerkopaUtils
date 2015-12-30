using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;
//using System.Web.UI;


namespace FerkopaUtils
{
    /*
    Replace "yourkey" with your google api key. Get one here: http://code.google.com/apis/maps/signup.html 
    Include in your project, reference the class through a using directive. 
    Call get the coordinates like this: 
    Coordinate coordinate = Geocode.GetCoordinates("1600 Amphitheatre Parkway Mountain View, CA 94043");
    decimal latitude = coordinate.Latitude;
    decimal longitude = coordinate.Longitude; 
    The maximum # of Geocode requests that can be completed in one day are 50,000 (details).
    */
    public interface ISpatialCoordinate
    {
        decimal Latitude { get; set; }
        decimal Longitude { get; set; }
    }

    /// <summary>
    /// Coordiate structure. Holds Latitude and Longitude.
    /// </summary>
    public struct Coordinate : ISpatialCoordinate
    {
        private decimal _latitude;
        private decimal _longitude;

        public Coordinate(decimal latitude, decimal longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        #region ISpatialCoordinate Members

        public decimal Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                this._latitude = value;
            }
        }

        public decimal Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                this._longitude = value;
            }
        }

        #endregion
    }

    public class Geocode
    {
        private const string _googleUri = "https://maps.googleapis.com/maps/api/geocode/";
        private const string _googleKey = "AIzaSyB8F92xeadHQIN-P7tGBZJJN5L65HypdPY";
        private const string _outputType = "json"; // Available options: csv, xml, kml, json

        private static Uri GetGeocodeUri(string address)
        {
            //address = HttpUtility.UrlEncode(address);
            //https://maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=API_KEY
            return new Uri(String.Format("{0}{1}?address={2}&key={3}", _googleUri,_outputType,address, _googleKey));
        }

        /// <summary>
        /// Gets a Coordinate from a address.
        /// </summary>
        /// <param name="address">An address.
        /// <remarks>
        /// <example>1600 Amphitheatre Parkway Mountain View, CA 94043</example>
        /// </remarks>
        /// </param>
        /// <returns>A spatial coordinate that contains the latitude and longitude of the address.</returns>
        public static GoogleGeoCodeResponse  GetCoordinates(string address)
        {
            var client = new WebClient();
            Uri uri = GetGeocodeUri(address);
            var jss = new JavaScriptSerializer();

            var geocodeInfoJsonString = client.DownloadString(uri);
            var geocodeInfo = jss.Deserialize<GoogleGeoCodeResponse>(geocodeInfoJsonString);

            return geocodeInfo;
        }

    }

    public class GoogleGeoCodeResponse : ISpatialCoordinate
    {
       
        public results[] results { get; set; }
        public string status { get; set; }

        public decimal Latitude
        {
            get { return Utils.ObjectToDecimal(results[0].geometry.location.lat); }
            set {  }
        }

        public decimal Longitude
        {
            get { return Utils.ObjectToDecimal(results[0].geometry.location.lng); }
            set { }
        }
    }

    public class results
    {
        public address_component[] address_components { get; set; }
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string[] types { get; set; }
    }

    public class address_component
    {
        String long_name { get; set; }
        String short_name { get; set; }
        String types { get; set; }
        String partial_match { get; set; }

    }

    public class geometry
    {
        public bounds bounds { get; set; }
        public location location { get; set; }
        public string location_type { get; set; }
        public viewport viewport { get; set; }
    }

    public class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class viewport
    {
        public northeast northeast { get; set; }
        public southwest southwest { get; set; }
    }

    public class bounds
    {
        public northeast northeast { get; set; }
    }

    public class northeast
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class southwest
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }
}


