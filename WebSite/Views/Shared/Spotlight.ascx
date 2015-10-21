<%@ Control Language="C#" %>

<style>
#shadow-area-spot {
    position: absolute;
    z-index: 10000;
    top: -70px;
    bottom: -30px;
    left: 0;
    right: 0;
    background: url("/Content/images/spotlight-big.png") no-repeat 50% 50%;
    cursor: none;
}
</style>

<div id="shadow-area-spot"></div>

<script charset="utf-8" language="javascript" type="text/javascript">
/**
 * Inspired by "Text Shadow Box" by Zachary Johnson / 2009 / www.zachstronaut.com
 */
 
var spot;

init();

function init() {
    spot = document.getElementById('shadow-area-spot');
    
    document.getElementById('stage').onmousemove = onMouseMove;
    document.getElementById('stage').ontouchmove = function (e) {e.preventDefault(); e.stopPropagation(); onMouseMove({clientX: e.touches[0].clientX, clientY: e.touches[0].clientY});};
    
    onMouseMove({clientX: 300, clientY: 200});
}

function onMouseMove(e) {
    var xm = e.clientX - 300;
    var ym = e.clientY - 175;
    var d = Math.sqrt(xm*xm + ym*ym);
    
    xm = e.clientX - 2000;
    ym = e.clientY - 1500;
    spot.style.backgroundPosition = xm + 'px ' + ym + 'px';
}
</script>
