using ProductSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ProductSearch.Services
{
    public class AmazonAffiliate : IAffiliate
    {
        private const string MY_AWS_ACCESS_KEY_ID = "ABCDEFG12HIJKLM34NOP";
        private const string MY_AWS_SECRET_KEY = "3wnDD+Frol050qGF6uHZc9zg8svjk2bhTnyHib9G";
        private const string MY_ASSOCIATE_TAG = "foobar-12";
        private const string DESTINATION = "webservices.amazon.com";
        private const string NAMESPACE = @"{http://webservices.amazon.com/AWSECommerceService/2011-08-01}";
        private const int MAX_SEARCH_RESULT = 5;
        private Affiliate affiliate;

        public int TotalPages { get; private set; }

        public Affiliate GetAffiliate()
        {
            return new Affiliate { Name = "Amazon" };
        }

        protected XDocument ItemSearch(string keywords, string searchIndex = "All", string responseGroup = "Large", int itemPage = 1)
        {
            var request = new Dictionary<string, string>();
            request["Service"] = "AWSECommerceService";
            request["Version"] = "2011-08-01";
            request["Operation"] = "ItemSearch";
            request["SearchIndex"] = searchIndex;
            request["ResponseGroup"] = responseGroup;
            request["Keywords"] = keywords;
            request["ItemPage"] = itemPage.ToString();

            var helper = new AmazonSignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, MY_ASSOCIATE_TAG, DESTINATION);
            var requestUrl = helper.Sign(request);
            var doc = XDocument.Load(requestUrl);

            TotalPages = Int32.Parse(doc
                .Element(NAMESPACE + "ItemSearchResponse")
                .Element(NAMESPACE + "Items")
                .Element(NAMESPACE + "TotalPages")
                .Value);

            // Amazon affiliate currently limits search results to 5 pages.
            TotalPages = TotalPages > MAX_SEARCH_RESULT ? MAX_SEARCH_RESULT : TotalPages;

            return doc;
        }

        public ICollection<Product> Find(string keywords, int page)
        {
            //var uri = HttpContext.Current.Server.MapPath(@"~/Services/Samples/Items.xml");
            //var doc = XDocument.Load(uri);
            var doc = ItemSearch(keywords, itemPage: page);

            var products = new List<Product>();
            foreach (var item in doc.Descendants(NAMESPACE + "Item"))
            {
                products.Add(ParseProduct(item));
            }

            return products;
        }

        private Product ParseProduct(XElement item)
        {
            var product = new Product();
            var attributes = item.Element(NAMESPACE + "ItemAttributes");

            product.Name = attributes.Element(NAMESPACE + "Title").Value;

            return product;
        }
    }
}