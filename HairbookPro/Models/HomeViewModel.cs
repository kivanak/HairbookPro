using Hairbookpro.Models;
using System.Collections.Generic;

namespace Hairbookpro.Models
{
    public class HomeViewModel
    {
        public List<Service> Services { get; set; }

        public List<Stylist> Stylists { get; set; }

        public List<BlogPost> BlogPosts { get; set; }
    }
}