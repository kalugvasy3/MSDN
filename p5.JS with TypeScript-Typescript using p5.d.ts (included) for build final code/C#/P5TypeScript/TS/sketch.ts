namespace PROCESS {

    export class Sketch {

        private _p: p5;

        //inside p5.d.ts "createCanvas" function was changed to "p5.Element"
        private _canvas: p5.Element; // canvas should have "parent" property  (it should not be HTMLElement)
        private _morph: Morph;
        private _background: number;

        private _divId: string;
        private _divElement: HTMLElement;

        /**
         * Constructor for new Sketch
         * @param background Canvas background, optional. Default value = 255
         */
        public constructor(background: number = 255, divId : string = "") {
            this._background = background; //Must set _background number before create new p5
            this._divId = divId;
            this._divElement = document.getElementById(this._divId);
            this._p = new p5(this.sketch);
        }

        /** 
         *  "sketch" interface implementation, it must include "setup" and "draw", other function(s) - optional.
         */
        private sketch = (pp: p5) => {

            this._morph = new Morph();

            pp.preload = () => {

            }

            pp.setup = () => {
                //inside p5.d.ts "createCanvas" function was changed to "p5.Element"
                this._canvas = pp.createCanvas(this._divElement.offsetWidth, pp.windowHeight);
                this._canvas.parent(this._divId);
                this._morph.setup(pp);
            }

            pp.windowResized = () => {               
                pp.resizeCanvas(this._divElement.clientWidth, this._p.windowHeight);
            }

            pp.draw = () => {
                pp.background(this._background);
                this._morph.draw(pp);
            }
        }
    }
}