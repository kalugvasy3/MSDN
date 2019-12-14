// Some usefull links, PROJECT uses p5.js library - see link

//https://www.youtube.com/watch?v=bkGf4fEHKak
//https://p5js.org
//https://processing.org/handbook/
//https://forum.processing.org/two/discussion/10913/saving-the-p5-js-canvas-as-an-image-on-my-server
//https://docs.microsoft.com/en-us/azure/cognitive-services/face/quickstarts/javascript
//https://docs.microsoft.com/en-us/azure/cognitive-services/emotion/quickstarts/javascript


/////////////////////////////////////////////////////////////////////////////////////
// Instance MODE 

var myVideo = function (p) { // p could be any variable name

    p.setup = function () {
        p.canv = p.createCanvas(320, 240);
        p.canv.parent('imgVideo');   //attache to the < div id = "imgVideo" ></div >

        p.video = p.createCapture(p.VIDEO);
        p.video.size(320, 240);
        p.video.hide();
    };

    p.draw = function () {
        p.image(p.video, 0, 0, 320, 240);
    };
};
// Create new instance - for VIDEO
var myV = new p5(myVideo);

/////////////////////////////////////////////////////////////////////////////////////
// Instance MODE

var myPhoto = function (p) { // p could be any variable name

    p.setup = function () {
        p.canv = p.createCanvas(320, 240);
        p.canv.parent('imgSnap');  //attach to the < div id = "imgSnap" ></div >
    };
  

    p.mouseClicked = function () {
        p.image(myV.video, 0, 0, 320, 240);

        p.takeSnap();
    };

    p.takeSnap = function () {
        //processImage(); // How to Save ...
        getFaceDetails();
    };
};
// Create new instance - for Image.
var myP = new p5(myPhoto);

/////////////////////////////////////////////////////////////////////////////////////

// Instance mode
var res = function (p) { // p could be any variable name

    p.setup = function () {
        p.canv = p.createCanvas(960, 400);
        p.canv.parent('txtResult');   // attache to the <div id="txtResult"></div>
    };

    p.drawWords = function (mytext, x, y) {
        p.fill(0);
        p.text(mytext, x, y);
    };
};
// Create instance for RESULT
var myRes = new p5(res);

/////////////////////////////////////////////////////////////////////////////////////

// Function send image to Azure face detect ...
function getFaceDetails() {

    var imgData = myP.canv.canvas.toDataURL('image/bmp', 1.0);  // Take "canv.canvs" and trnsfer to DataURL (it is base64 word)
    var idata = imgData.split(',')[1];  // we do not need a preamble

    var sampleArr = base64ToArrayBuffer(idata);  // Convert to byte array

    var xmlHttp = new XMLHttpRequest();

    // This all options ... 
    var strAttr = "returnFaceId=true&returnFaceLandmarks=true";
        strAttr = strAttr + "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,";
        strAttr = strAttr + "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

    var url = "https://eastus.api.cognitive.microsoft.com/face/v1.0/detect?" + strAttr;

    xmlHttp.open("POST", url, true);

    // Use stream  
    xmlHttp.setRequestHeader("Content-Type", "application/octet-stream");
    // instead of "bd3ab02cd38b46eb931f7b77c90aab73" insert YOUR CODE - see your subscription.  
    // this code will be disabled .
    xmlHttp.setRequestHeader("Ocp-Apim-Subscription-Key", "bd3ab02cd38b46eb931f7b77c90aab73");

    xmlHttp.send(sampleArr);
    xmlHttp.onreadystatechange = function (response) {
        if (this.readyState == 4 && this.status == 200) {   // For valid responce some options (not ALL) will be displaied. 
            let face = JSON.parse(this.responseText)
            myP.stroke(127, 63, 120);
            myP.noFill();
            myP.rect(face[0].faceRectangle.left, face[0].faceRectangle.top, face[0].faceRectangle.width, face[0].faceRectangle.height);

            // Get emotion confidence scores
            var emotion = face[0].faceAttributes.emotion;

            var iii = 0;
            myRes.background(255);

            // Append to DOM
            for (var prop in emotion) {
                myRes.drawWords(prop, 230, 20 + 15 * iii);
                myRes.drawWords(emotion[prop], 330, 20 + 15 * iii);
                iii = iii + 1;
            }
            iii += 1;
            var age = face[0].faceAttributes.age;
            myRes.drawWords("age", 230, 20 + 15 * iii);
            myRes.drawWords(age, 330, 20 + 15 * iii);

        } else {
            //alert(JSON.stringify(this.responseText));
        };
    };
};

/////////////////////////////////////////////////////////////////////////////////////

// Convert to bytes ..
function base64ToArrayBuffer(base64) {    // 
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}

// How to dowload - example ...
function saveByteArray(reportName, byte) {
    var blob = new Blob([byte], { type: "image/bmp" });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = reportName;
    link.download = fileName;
    link.click();
};

// Example for saving to image file ...you can also use jpeg/png/gif
function processImage() {
    var imgData = myP.canv.canvas.toDataURL('image/bmp', 1.0);
    var idata = imgData.split(',')[1];

    var sampleArr = base64ToArrayBuffer(idata);
    saveByteArray("Sample Report", sampleArr);

};


//https://github.com/fabean/javascript-p2p-chat   - P2P library , for future uses.



