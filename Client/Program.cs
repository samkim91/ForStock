using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ForStock.Client.ViewModels;
using Blazored.Modal;
using TG.Blazor.IndexedDB;
using System.Collections.Generic;

namespace ForStock.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            SetIndexedDB(builder);
            builder.Services.AddHttpClient<IIntroViewModel, IntroViewModel>
                ("ServerAPI", Client => Client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddTransient<IVisualizationViewModel, VisualizationViewModel>();
            builder.Services.AddBlazoredModal();

            await builder.Build().RunAsync();
        }

        public static void SetIndexedDB(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "ForStockDB";
                dbStore.Version = 1;
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "IntroModel",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>{
                        new IndexSpec{Name ="crtfc_key", KeyPath="crtfc_key", Auto=false},
                        new IndexSpec{Name="corp_name", KeyPath="corp_name", Auto=false},
                        new IndexSpec{Name="stock_code", KeyPath="stock_code", Auto=false},
                        new IndexSpec{Name="fs_div", KeyPath="fs_div", Auto=false}
                    }
                });
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "CorporationInfo",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>{
                        new IndexSpec {Name = "corp_code", KeyPath = "corp_code", Auto = false},
                        new IndexSpec {Name = "corp_name", KeyPath = "corp_name", Auto = false},
                        new IndexSpec {Name = "corp_name_eng", KeyPath = "corp_name_eng", Auto = false},
                        new IndexSpec {Name = "ceo_nm", KeyPath = "ceo_nm", Auto = false},
                        new IndexSpec {Name = "corp_cls", KeyPath = "corp_cls", Auto = false},
                        new IndexSpec {Name = "jurir_no", KeyPath = "jurir_no", Auto = false},
                        new IndexSpec {Name = "bizr_no", KeyPath = "bizr_no", Auto = false},
                        new IndexSpec {Name = "adres", KeyPath = "adres", Auto = false},
                        new IndexSpec {Name = "hm_url", KeyPath = "hm_url", Auto = false},
                        new IndexSpec {Name = "ir_url", KeyPath = "ir_url", Auto = false},
                        new IndexSpec {Name = "phn_no", KeyPath = "phn_no", Auto = false},
                        new IndexSpec {Name = "fax_no", KeyPath = "fax_no", Auto = false},
                        new IndexSpec {Name = "induty_code", KeyPath = "induty_code", Auto = false},
                        new IndexSpec {Name = "est_dt", KeyPath = "est_dt", Auto = false},
                        new IndexSpec {Name = "acc_mt", KeyPath = "acc_mt", Auto = false},
                        new IndexSpec {Name = "status", KeyPath = "status", Auto = false},
                        new IndexSpec {Name = "message", KeyPath = "message", Auto = false}
                    }
                });
                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "ChartDataModel",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec {Name = "Message", KeyPath = "Message", Auto = false},
                        new IndexSpec {Name = "QuarterAmounts", KeyPath = "QuarterAmounts", Auto = false},
                        new IndexSpec {Name = "YearAmounts", KeyPath = "YearAmounts", Auto = false},
                        new IndexSpec {Name = "AmountsGroupByQuarter", KeyPath = "AmountsGroupByQuarter", Auto = false},
                        new IndexSpec {Name = "AmountsGroupByYear", KeyPath = "AmountsGroupByYear", Auto = false},
                        new IndexSpec {Name = "Years", KeyPath = "Years", Auto = false},
                        new IndexSpec {Name = "YearAndQuarters", KeyPath = "YearAndQuarters", Auto = false},
                        new IndexSpec {Name = "DataSets", KeyPath = "DataSets", Auto = false}
                    }
                });
            });
        }
    }
}
