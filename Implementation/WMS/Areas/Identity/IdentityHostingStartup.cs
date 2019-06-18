﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WMS.Areas.Identity.Data;
using WMS.Models;

[assembly: HostingStartup(typeof(WMS.Areas.Identity.IdentityHostingStartup))]
namespace WMS.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WMSContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AzureConnection")));

                services.AddDefaultIdentity<WMSUser>().
                    AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<WMSContext>();
            });
        }
    }
}