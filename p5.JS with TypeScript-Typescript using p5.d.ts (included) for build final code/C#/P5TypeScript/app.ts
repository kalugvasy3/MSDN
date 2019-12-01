
namespace PROCESS {
    let scetch0: PROCESS.Sketch;
    let scetch1: PROCESS.Sketch;


    window.onresize = () => {
        //let element1 = document.getElementById("morph1");
        //element1.style.width = (window.innerWidth / 2).toString();

        //let element2 = document.getElementById("morph2");
        //element2.style.width = (window.innerWidth / 2).toString();
    }
    
    window.onload = () => {
        scetch0 = new PROCESS.Sketch(128,"morph1");
        scetch1 = new PROCESS.Sketch(196,"morph2");
    }
}
