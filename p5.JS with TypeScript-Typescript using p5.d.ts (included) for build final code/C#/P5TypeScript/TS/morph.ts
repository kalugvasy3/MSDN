//https://processing.org/examples/morph.html

namespace PROCESS {

    export class Morph {

        // Arrays to store the vertices for the shapes
        private shapes: { points: p5.Vector[], color: p5.Color }[];
        private currentShape: number;
        // An array for the set of vertices begin drawn into the window
        private _morph: p5.Vector[];

        // This boolean variable will control if we are morphing to a circle or square

        public setup(p: p5) {

            // Setup shapes array
            this.shapes = [];
            this.currentShape = 0;

            this.shapes.push({ points: Shapes.star(p, 0, 0, 30, 70, 5), color: p.color('#E23838') });
            this.shapes.push({ points: Shapes.circle(p, 100), color: p.color('#009CDF') });
            this.shapes.push({ points: Shapes.circle(p, 150), color: p.color(255, 204, 0) });
            this.shapes.push({ points: Shapes.square(p, 50), color: p.color(175, 100, 220) });
            this.shapes.push({ points: Shapes.triangle(p, -100, 0, 0, 100, 100, 0), color: p.color('#009CDF') });

            // setup morph array
            this._morph = new Array<p5.Vector>();
            let highestCount = 0;
            for (var i = 0; i < this.shapes.length; i++) {
                highestCount = Math.max(highestCount, this.shapes[i].points.length);
            }
            for (var i = 0; i < highestCount; i++) {
                this._morph.push(new p5.Vector());
            }
        }


        public draw(p: p5) {
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

        private recalc(p: p5) {
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
                this.currentShape++;//= !this.state;
                if (this.currentShape >= this.shapes.length) {
                    this.currentShape = 0;
                }
            }
        }

    }
}