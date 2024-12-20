using PractiveLabs.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractiveLabs.Service
{
    public static class Session
    {
        public static user CurrentUser { get; set; }
        public static void RemoveCurrentUser() => CurrentUser = null;
    }
}
