﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PlayMore_V2._0.Startup))]
namespace PlayMore_V2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
