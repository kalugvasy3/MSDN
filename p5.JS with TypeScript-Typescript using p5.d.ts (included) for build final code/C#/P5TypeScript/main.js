var PROCESS;
(function (PROCESS) {
    let scetch0;
    let scetch1;
    window.onresize = () => {
        //let element1 = document.getElementById("morph1");
        //element1.style.width = (window.innerWidth / 2).toString();
        //let element2 = document.getElementById("morph2");
        //element2.style.width = (window.innerWidth / 2).toString();
    };
    window.onload = () => {
        scetch0 = new PROCESS.Sketch(128, "morph1");
        scetch1 = new PROCESS.Sketch(196, "morph2");
    };
})(PROCESS || (PROCESS = {}));
//https://processing.org/examples/morph.html
var PROCESS;
(function (PROCESS) {
    class Morph {
        // This boolean variable will control if we are morphing to a circle or square
        setup(p) {
            // Setup shapes array
            this.shapes = [];
            this.currentShape = 0;
            this.shapes.push({ points: PROCESS.Shapes.star(p, 0, 0, 30, 70, 5), color: p.color('#E23838') });
            this.shapes.push({ points: PROCESS.Shapes.circle(p, 100), color: p.color('#009CDF') });
            this.shapes.push({ points: PROCESS.Shapes.circle(p, 150), color: p.color(255, 204, 0) });
            this.shapes.push({ points: PROCESS.Shapes.square(p, 50), color: p.color(175, 100, 220) });
            this.shapes.push({ points: PROCESS.Shapes.triangle(p, -100, 0, 0, 100, 100, 0), color: p.color('#009CDF') });
            // setup morph array
            this._morph = new Array();
            let highestCount = 0;
            for (var i = 0; i < this.shapes.length; i++) {
                highestCount = Math.max(highestCount, this.shapes[i].points.length);
            }
            for (var i = 0; i < highestCount; i++) {
                this._morph.push(new p5.Vector());
            }
        }
        draw(p) {
            this.recalc(p);
            const color = this.shapes[this.currentShape].color;
            const points = this.shapes[this.currentShape].points;
            // Draw relative to center
            p.translate(p.width / 2, p.height / 2);
            p.strokeWeight(4);
            // Draw a polygon that makes up all the vertices
            p.beginShape();
            p.noFill();
            p.stroke(color);
            for (var i = 0; i < points.length; i++) {
                var v = this._morph[i];
                p.vertex(v.x, v.y);
            }
            p.endShape(p.CLOSE);
        }
        recalc(p) {
            // We will keep how far the vertices are from their target
            var totalDistance = 0;
            // Look at each vertex
            const points = this.shapes[this.currentShape].points;
            for (var i = 0; i < points.length; i++) {
                // Are we lerping to the circle or square?
                var v1 = points[i];
                // Get the vertex we will draw
                var v2 = this._morph[i];
                // Lerp to the target
                v2.lerp(v1, 0.1);
                // Check how far we are from target
                totalDistance += p5.Vector.dist(v1, v2);
            }
            // If all the vertices are close, switch shape
            if (totalDistance < 0.05) {
                this.currentShape++; //= !this.state;
                if (this.currentShape >= this.shapes.length) {
                    this.currentShape = 0;
                }
            }
        }
    }
    PROCESS.Morph = Morph;
})(PROCESS || (PROCESS = {}));
var PROCESS;
(function (PROCESS) {
    class Shapes {
        static circle(p, size) {
            // Create a circle using vectors pointing from center
            const points = new Array();
            for (var angle = 0; angle < 360; angle += 6) {
                // Note we are not starting from 0 in order to match the
                // path of a circle.  
                var v = p5.Vector.fromAngle(p.radians(angle - 135));
                v.mult(size);
                points.push(v);
            }
            return points;
        }
        static square(p, size) {
            const points = new Array();
            // A square is a bunch of vertices along straight lines
            // Top of square
            for (var x = -size; x < size; x += 10) {
                points.push(new p5.Vector(x, -size));
            }
            // Right side
            for (var y = -size; y < size; y += 10) {
                points.push(new p5.Vector(size, y));
            }
            // Bottom
            for (var x = size; x > -size; x -= 20) {
                points.push(new p5.Vector(x, size));
            }
            // Left side
            for (var y = size; y > -size; y -= 10) {
                points.push(new p5.Vector(-size, y));
            }
            return points;
        }
        // star(0, 0, 30, 70, 5); 
        static star(p, x, y, radius1, radius2, npoints) {
            var angle = p.TWO_PI / npoints;
            var halfAngle = angle / 2.0;
            const points = new Array();
            for (var a = 0; a < p.TWO_PI; a += angle) {
                var sx = x + p.cos(a) * radius2;
                var sy = y + p.sin(a) * radius2;
                points.push(new p5.Vector(sx, sy));
                sx = x + p.cos(a + halfAngle) * radius1;
                sy = y + p.sin(a + halfAngle) * radius1;
                points.push(new p5.Vector(sx, sy));
            }
            return points;
        }
        // triangle(.............);
        static triangle(p, x1, y1, x2, y2, x3, y3) {
            const points = new Array();
            // A triangle is a bunch of vertices along straight lines
            points.push(new p5.Vector(x1, y1));
            points.push(new p5.Vector(x2, y2));
            points.push(new p5.Vector(x3, y3));
            return points;
        }
    }
    PROCESS.Shapes = Shapes;
})(PROCESS || (PROCESS = {}));
var PROCESS;
(function (PROCESS) {
    class Sketch {
        /**
         * Constructor for new Sketch
         * @param background Canvas background, optional. Default value = 255
         */
        constructor(background = 255, divId = "") {
            /**
             *  "sketch" interface implementation, it must include "setup" and "draw", other function(s) - optional.
             */
            this.sketch = (pp) => {
                this._morph = new PROCESS.Morph();
                pp.preload = () => {
                };
                pp.setup = () => {
                    //inside p5.d.ts "createCanvas" function was changed to "p5.Element"
                    this._canvas = pp.createCanvas(this._divElement.offsetWidth, pp.windowHeight);
                    this._canvas.parent(this._divId);
                    this._morph.setup(pp);
                };
                pp.windowResized = () => {
                    pp.resizeCanvas(this._divElement.clientWidth, this._p.windowHeight);
                };
                pp.draw = () => {
                    pp.background(this._background);
                    this._morph.draw(pp);
                };
            };
            this._background = background; //Must set _background number before create new p5
            this._divId = divId;
            this._divElement = document.getElementById(this._divId);
            this._p = new p5(this.sketch);
        }
    }
    PROCESS.Sketch = Sketch;
})(PROCESS || (PROCESS = {}));
//# sourceMappingURL=main.js.map