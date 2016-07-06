using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(aspnetmvc5.Startup))]

namespace aspnetmvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
