using System.Collections.Generic;
using Blazor.IndexedDB.Framework;
using ForStock.Shared.Model;
using Microsoft.JSInterop;

namespace ForStock.Client.Common
{
    public class MyIndexedDB : IndexedDb
    {
        public MyIndexedDB(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }
        public IndexedSet<CorporationInfo> CorporationInfo { get; set; }
        public IndexedSet<FinacialStatement> FinacialStatement { get; set; }
    }
}