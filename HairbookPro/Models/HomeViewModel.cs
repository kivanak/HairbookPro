using Hairbookpro.Models;
using System.Collections.Generic;

namespace HairBookPro.Models
{
    public class HomeViewModel
    {
        public List<Service> Services { get; set; }

        public List<Stylist> Stylists { get; set; }

        public List<BlogPost> BlogPosts { get; set; }
    }
}