// Setup() 이라 불리는 function 안에 Chart.js component를 init 하기 위한 js file.
// Canvas Id와 Blazor component로부터 config options를 받고, chart js로 넘겨줌.
window.setup = (id,config) => {
    var ctx = document.getElementById(id).getContext('2d');
    new Chart(ctx, config);
}