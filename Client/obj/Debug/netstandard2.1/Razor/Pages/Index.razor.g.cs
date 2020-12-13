#pragma checksum "D:\MyProjects\ForStock\Client\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "096bb0e09ca5e66258495739ca533c0e4473d563"
// <auto-generated/>
#pragma warning disable 1591
namespace ForStock.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\MyProjects\ForStock\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\MyProjects\ForStock\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\MyProjects\ForStock\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\MyProjects\ForStock\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\MyProjects\ForStock\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\MyProjects\ForStock\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\MyProjects\ForStock\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\MyProjects\ForStock\Client\_Imports.razor"
using ForStock.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\MyProjects\ForStock\Client\_Imports.razor"
using ForStock.Client.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h1>Welcome to ForStock app!</h1>\r\n<br>\r\n\r\n");
            __builder.AddMarkupContent(1, "<p>안녕하세요. ForStock에 처음 오시는 분들을 위해, 간략한 사용법과 주의사항 등을 안내해드리겠습니다.</p>\r\n<br>\r\n\r\n");
            __builder.AddMarkupContent(2, @"<div class=""card"">
    <div class=""card-body"">
    <h2>사용법</h2>
        <ol>
            <li>아래 link에 들어가셔서 key를 받는다. 등록메일 인증도 한다.</li>
            <a></a>
            <li>로그인 후 API key 값을 복사한다.</li>
            <li>Intro tab에 가서 API key란에 복사한 key를 넣어준다.</li>
            <li>그 아래 주식코드를 입력한다.</li>
            <li>연결재무재표는 CFS, 개별재무재표는 OFS를 선택한다.</li>
            <li>가져오기를 클릭한다.</li>
        </ol>
    </div>
</div>
<br>

");
            __builder.AddMarkupContent(3, @"<div class=""card"">
    <div class=""card-body"">
        <h2>주의사항</h2>
        <ul>
            <li>기업 data를 가져오는 것은 1일 10,000번만 가능하다.</li>
            <li>1일 사용량 확인은 아래 link에서 가능하다.</li>
            <a></a>
            <li>금융회사의 재무재표는 확인할 수 없다.</li>
            <li>기업에서 제공한 재무재표에 오차/오류가 있을 수 있다. 참고하자.</li>
        </ul>
    </div>
</div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
