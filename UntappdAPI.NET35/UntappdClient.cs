using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using UntappdAPI.DataContracts;
using System.Windows.Threading;


namespace UntappdAPI
{
    public class UntappdClient
    {
        public enum SearchSortType
        {
            Name,
            Count
        }

        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public event EventHandler<ResponseArgs<BeerSearchResponse>> BeerSearchResultsComplete;
        public event EventHandler<ResponseArgs<BeerInfoResponse>> BeerInfoComplete;
        public event EventHandler<ResponseArgs<BreweryInfoResponse>> BreweryInfoComplete;
        public event EventHandler<ResponseArgs<UserInfoResponse>> UserInfoComplete;
        public event EventHandler<ResponseArgs<UserFeedResponse>> UserFeedComplete;
        public event EventHandler<ResponseArgs<TrendingResponse>> TrendingComplete;
        public event EventHandler<ResponseArgs<CheckinResponse>> CheckinComplete;
        public event EventHandler<ResponseArgs<ErrorResponse>> RemoteError;


        public string FoursquareUrl { get; set; }
        public string FoursquareClientId { get; set; }
        public string FoursquareSecret { get; set; }
        public event EventHandler<ResponseArgs<IEnumerable<Venue>>> VenueSearchComplete;

        public UntappdClient()
        {
        }

        public UntappdClient(string userName, string password, string key)
            : this("http://api.untappd.com/v3", userName, password, key, null, null)
        {

        }

        public UntappdClient(string key)
            : this("http://api.untappd.com/v3", null, null, key, null, null)
        {

        }

        public UntappdClient(string userName, string password, string key, string foursquareClientId, string foursquareSecret)
            : this("http://api.untappd.com/v3", userName, password, key, foursquareClientId, foursquareSecret)
        {

        }

        public UntappdClient(string baseUrl, string userName, string password, string key, string foursquareClientId, string foursquareSecret)
        {
            BaseUrl = baseUrl;
            UserName = userName;
            Password = password;
            Key = key;

            FoursquareUrl = "https://api.foursquare.com/v2/";
            FoursquareClientId = foursquareClientId;
            FoursquareSecret = foursquareSecret;
        }

        public string TestMethod(string methodName, Dictionary<string, string> optionalParams)
        {
            GetResponseMessage<String>(methodName, optionalParams);
            return null;
        }

        public void FetchTrending() {
            GetResponseMessage<TrendingResponse>("trending");
        }

        public void FetchUserInfo(string userName)
        {
            var parms = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(userName))
                parms.Add("user", userName);

            parms.Add("nocache", DateTime.Now.Ticks.ToString());
            GetResponseMessage<UserInfoResponse>("user", parms);
        }

        public void FetchUserInfo()
        {
            FetchUserInfo(null);
        }

        public void FetchUserFeed(string userName, int offset = 0)
        {
            var parms = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(userName))
                parms.Add("user", userName);
            if (offset > 0)
                parms.Add("offset", offset.ToString());

            parms.Add("nocache", DateTime.Now.Ticks.ToString());
            GetResponseMessage<UserFeedResponse>("user_feed", parms);
        }

        public void FetchUserFeed(int offset = 0)
        {
            FetchUserFeed(null, offset);
        }

        public void FetchFriendFeed(int offset = 0)
        {
            var parms = new Dictionary<string, string>();
            if (offset > 0)
                parms.Add("offset", offset.ToString());
            GetResponseMessage<UserFeedResponse>("feed", parms);
        }

        public void FetchBeerInfo(int beerId)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("bid", beerId.ToString());
            GetResponseMessage<BeerInfoResponse>("beer_info", parms);
        }

        public void FetchBreweryInfo(int breweryId) {
            var parms = new Dictionary<string, string>();
            parms.Add("brewery_id", breweryId.ToString());
            GetResponseMessage<BreweryInfoResponse>("brewery_info", parms);
        }

        public void FetchBeerSearchResults(string query, int offset = 0, SearchSortType sortType = SearchSortType.Name)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("q", query);
            if (offset > 0)
                parms.Add("offset", offset.ToString());
            if (sortType == SearchSortType.Count)
                parms.Add("sort", "count");
            GetResponseMessage<BeerSearchResponse>("beer_search", parms);
        }

        public void CheckInBeer(int beerId, double gmtOffset, string comment, bool postToFacebook, bool postToTwitter)
        {
            CheckInBeer(beerId, gmtOffset, null, null, null, comment, null, postToFacebook, postToTwitter, false, false);
        }

        public void CheckInBeer(int beerId, double gmtOffset)
        {
            CheckInBeer(beerId, gmtOffset, null, null, null, null, null, false, false, false, false);
        }

        public void CheckInBeer(int beerId, double gmtOffset, string foursquareId, float? locationLat, float? locationLng, string comment, int? rating, bool postToFacebook, bool postToTwitter, bool postToFoursquare, bool testCheckin)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("bid", beerId.ToString());
            parms.Add("gmt_offset", gmtOffset.ToString("F2"));
            if (!String.IsNullOrEmpty(foursquareId))
                parms.Add("foursquare_id", foursquareId);
            if (locationLng.HasValue && locationLat.HasValue)
            {
                parms.Add("user_lat", locationLat.ToString());
                parms.Add("user_lng", locationLng.ToString());
            }
            if (!String.IsNullOrEmpty(comment))
                parms.Add("shout", comment);
            if (rating.HasValue && rating >= 1 && rating <= 5)
                parms.Add("rating_value", rating.ToString());
            if (postToFacebook)
                parms.Add("facebook", "on");
            if (postToTwitter)
                parms.Add("twitter", "on");
            if (postToFoursquare)
                parms.Add("foursquare", "on");

            var methodName = testCheckin ? "checkin_test" : "checkin";
            GetResponseMessage<CheckinResponse>(methodName, parms, "POST");
        }

        private void GetResponseMessage<T>(string methodName)
        {

            GetResponseMessage<T>(methodName, null, "GET");
        }

        private void GetResponseMessage<T>(string methodName, Dictionary<string, string> optionalParams)
        {
            GetResponseMessage<T>(methodName, optionalParams, "GET");
        }

        private void GetResponseMessage<T>(string methodName, Dictionary<string, string> optionalParams, string httpMethod)
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            if (!String.IsNullOrEmpty(UserName))
                client.Headers[HttpRequestHeader.Authorization] = GetAuthHeader();

            var uri = new StringBuilder("/" + methodName + "?key=" + Key);
            var optionalParamValues = new StringBuilder();
            if (optionalParams != null)
            {
                foreach (var key in optionalParams.Keys)
                {
                    if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
                        uri.AppendFormat("&{0}={1}", key, optionalParams[key]);
                    else
                        optionalParamValues.AppendFormat("{0}={1}&", key, optionalParams[key]);
                }
            }

            var timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(20)
            };

            timer.Tick += (o, e) => {
                timer.Stop();
                if (client != null) {
                    client.CancelAsync();
                    RaiseError(client, new WebException("Operation timed out: GET " + typeof(T).ToString()));
                }
            };

            if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                client.DownloadStringCompleted += (sender, e) => {
                    if (timer.IsEnabled) {
                        timer.Stop();
                        client = null;

                        //Only process result if timer was enabled - if it was already disabled, it was timed out and this is a late request
                        if (e.Error == null) {
                            ProcessResponse(e.Result, e.UserState.ToString(), sender);
                        } else {
                            RaiseError(sender, e.Error);
                        }
                    }
                };
                client.DownloadStringAsync(new Uri(BaseUrl + uri), typeof(T).ToString() + ":" + methodName);
                timer.Start();
            }
            else //assume post
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.UploadStringCompleted += (sender, e) => {
                    if (timer.IsEnabled) {
                        timer.Stop();
                        client = null;

                        //Only process result if timer was enabled - if it was already disabled, it was timed out and this is a late request
                        if (e.Error == null) {
                            ProcessResponse(e.Result, e.UserState.ToString(), sender);
                        } else {
                            RaiseError(sender, e.Error);
                        }
                    }
                };
                client.UploadStringAsync(new Uri(BaseUrl + uri), "POST", optionalParamValues.Remove(optionalParamValues.Length - 1, 1).ToString(), typeof(T) + ":" + methodName);
                timer.Start();
            }
        }

        private void RaiseError(object sender, Exception x)
        {
            if (RemoteError != null)
            {
                string rawMessage = string.Empty;
                var webEx = x as WebException;
                var errResponse = new ErrorResponse()
                {
                    Error = x,
                };

                if (webEx != null)
                {
                    if (webEx.Response != null) {
                        using (var resp = webEx.Response)
                        using (var streamReader = new StreamReader(resp.GetResponseStream())) {
                            errResponse.Reponse = resp;
                            rawMessage = streamReader.ReadToEnd();
                            if (!String.IsNullOrEmpty(rawMessage)) {
                                var json = JObject.Parse(rawMessage);
                                if (json["result"] != null)
                                    errResponse.ErrorCode = json["result"].Value<String>();
                                else if (json["http_code"] != null)
                                    errResponse.ErrorCode = json["http_code"].Value<String>();
                                else
                                    errResponse.ErrorCode = webEx.Status.ToString();

                                if (json["error"] != null)
                                    errResponse.ErrorMessage = json["error"].Value<String>();
                                else
                                    errResponse.ErrorMessage = webEx.Message;
                            } else {
                                errResponse.ErrorCode = webEx.Status.ToString();
                                errResponse.ErrorMessage = webEx.Message;
                                rawMessage = webEx.ToString();
                            }
                        }
                    } else {
                        errResponse.ErrorCode = webEx.Status.ToString();
                        errResponse.ErrorMessage = webEx.Message;
                        rawMessage = webEx.ToString();
                    }
                }

                RemoteError(sender, new ResponseArgs<ErrorResponse>(errResponse, rawMessage));
            }
        }

        private void ProcessResponse(string result, string userState, object sender)
        {
            var typeName = userState;
            string methodName = String.Empty;

            if (typeName.Contains(":"))
            {
                var arr = typeName.Split(':');
                typeName = arr[0];
                methodName = arr[1];
            }
            switch (typeName)
            {
                case "UntappdAPI.DataContracts.BeerSearchResponse":
                    if (BeerSearchResultsComplete != null)
                    {
                        var resp = JSONHelper.Deserialise<BeerSearchResponse>(result);
                        BeerSearchResultsComplete(sender, new ResponseArgs<BeerSearchResponse>(resp, result));
                    }
                    break;
                case "UntappdAPI.DataContracts.BeerInfoResponse":
                    if (BeerInfoComplete != null)
                    {
                        var resp = JSONHelper.Deserialise<BeerInfoResponse>(result);
                        BeerInfoComplete(sender, new ResponseArgs<BeerInfoResponse>(resp, result));
                    }
                    break;
                case "UntappdAPI.DataContracts.BreweryInfoResponse":
                    if (BreweryInfoComplete != null) {
                        var resp = JSONHelper.Deserialise<BreweryInfoResponse>(result);
                        BreweryInfoComplete(sender, new ResponseArgs<BreweryInfoResponse>(resp, result));
                    }
                    break;
                case "UntappdAPI.DataContracts.UserInfoResponse":
                    if (UserInfoComplete != null)
                    {
                        var resp = JSONHelper.Deserialise<UserInfoResponse>(result);
                        UserInfoComplete(sender, new ResponseArgs<UserInfoResponse>(resp, result));
                    }
                    break;
                case "UntappdAPI.DataContracts.UserFeedResponse":
                    if (UserFeedComplete != null)
                    {
                        var resp = JSONHelper.Deserialise<UserFeedResponse>(result);
                        if (methodName.Equals("user_feed", StringComparison.OrdinalIgnoreCase))
                            resp.FeedType = "User";
                        else if (methodName.Equals("feed", StringComparison.OrdinalIgnoreCase))
                            resp.FeedType = "Friend";

                        UserFeedComplete(sender, new ResponseArgs<UserFeedResponse>(resp, result));
                    }
                    break;
                case "UntappdAPI.DataContracts.TrendingResponse":
                    if (TrendingComplete != null) {
                        var resp = JSONHelper.Deserialise<TrendingResponse>(result);
                        TrendingComplete(sender, new ResponseArgs<TrendingResponse>(resp, result));
                    }
                    break;
                case "UntappdAPI.DataContracts.CheckinResponse":
                    if (CheckinComplete != null)
                    {
                        var resp = JSONHelper.Deserialise<CheckinResponse>(result);
                        CheckinComplete(sender, new ResponseArgs<CheckinResponse>(resp, result));
                    }
                    break;
            }
        }

        private string GetAuthHeader()
        {
            var passwordHash = Security.GetMD5Hash(Password);
            var basicHashBytes = Encoding.UTF8.GetBytes(UserName + ":" + passwordHash);
            var basicHash = Convert.ToBase64String(basicHashBytes);
            return "Basic " + basicHash;
        }

        private void GetFoursquareResponseMessage(string endpoint, Dictionary<string, string> optionalParams)
        {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;

            var uri = new StringBuilder(endpoint + "?client_id=" + FoursquareClientId + "&client_secret=" + FoursquareSecret);
            if (optionalParams != null)
            {
                foreach (var key in optionalParams.Keys)
                {
                    uri.AppendFormat("&{0}={1}", key, optionalParams[key]);
                }
            }

            client.DownloadStringCompleted += FoursquareDownloadStringCompleted;
            client.DownloadStringAsync(new Uri(FoursquareUrl + uri));
        }

        private void FoursquareDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (VenueSearchComplete == null) return;

                var json = JObject.Parse(e.Result);
                var code = json["meta"]["code"].Value<int>();
                if (code != 200)
                {
                    var ex =
                        new Exception("code: " + code + " errorType: " + json["meta"]["errorType"].Value<String>() +
                                      " errorDetail: " + json["meta"]["errorDetail"].Value<String>());
                    RaiseError(sender, ex);
                    return;
                }
                var groups = (JArray)json["response"]["groups"];
                var items = (JArray)groups[0]["items"];
                var searchResults = new List<Venue>();

                foreach (var item in items)
                {
                    var venue = new Venue
                                    {
                                        Id = item["id"].Value<String>(),
                                        Name = item["name"].Value<String>(),
                                        Location = new VenueLocation
                                                       {
                                                           Address = item["location"]["address"] != null ? item["location"]["address"].Value<String>() : null,
                                                           City = item["location"]["city"] != null ? item["location"]["city"].Value<String>() : null,
                                                           State = item["location"]["state"] != null ? item["location"]["state"].Value<String>() : null,
                                                           ZipCode = item["location"]["postalCode"] != null ? item["location"]["postalCode"].Value<String>() : null,
                                                           Latitude = item["location"]["lat"].Value<Single>(),
                                                           Longitude = item["location"]["lng"].Value<Single>(),
                                                           Distance = item["location"]["distance"].Value<Int32>()
                                                       }
                                    };

                    var categories = (JArray)item["categories"];
                    var primaryCategory = categories.Where(t => t["primary"] != null ? t["primary"].Value<Boolean>().Equals(true) : false).SingleOrDefault();
                    if (primaryCategory != null)
                    {
                        venue.PrimaryCategory = new VenueCategory
                                                    {
                                                        Id = primaryCategory["id"].Value<String>(),
                                                        Name = primaryCategory["name"].Value<String>(),
                                                        PluralName = primaryCategory["pluralName"].Value<String>(),
                                                        Icon = primaryCategory["icon"].Value<String>(),
                                                        IsPrimary = true
                                                    };
                    }


                    searchResults.Add(venue);

                }

                VenueSearchComplete(sender, new ResponseArgs<IEnumerable<Venue>>(searchResults, e.Result));

            }
            else
            {
                RaiseError(sender, e.Error);
            }
        }

        public void VenueSearch(float lat, float lng, string query = null)
        {
            var parms = new Dictionary<string, string>();
            parms.Add("ll", lat + "," + lng);
            if (!String.IsNullOrEmpty(query))
                parms.Add("query", query);

            GetFoursquareResponseMessage("venues/search", parms);
        }
    }
}
