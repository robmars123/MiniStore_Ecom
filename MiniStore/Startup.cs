﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MiniStore.Startup))]
namespace MiniStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
