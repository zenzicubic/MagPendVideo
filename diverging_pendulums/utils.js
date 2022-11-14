// 2D vector class for the bob attributes
class Vector {
	constructor(x, y) {
		this.x = x;
		this.y = y;
	}

	add(v) {
		return new Vector(this.x + v.x, this.y + v.y);
	}

	sub(v) {
		return new Vector(this.x - v.x, this.y - v.y);
	}

	scale(s) {
		return new Vector(this.x * s, this.y * s);
	}

	div(s) {
		return new Vector(this.x / s, this.y / s);
	}

	dot(v) {
		return this.x*v.x+this.y*v.y;
	}

	clone() {
		return new Vector(this.x, this.y);
	}
}

class Pendulum {
	constructor(p, col) {
		this.p = p;
		this.v = new Vector(0, 0);
		this.a = new Vector(0, 0);
		this.aP = new Vector(0, 0);
		this.col = col;
		this.path = [];
	}

	draw() {
		// draw the bob
		ctx.fillStyle = this.col;
		ctx.beginPath();
		ctx.arc(scl * this.p.x, scl * this.p.y, 5, 0, 2 * Math.PI);
		ctx.fill();

		// draw the pendulum's path
		ctx.strokeStyle = this.col;
		ctx.beginPath();
		for (let p of this.path) {
			ctx.lineTo(p.x * scl, p.y * scl);
		}
		ctx.stroke();
	}
}

// Gnuplot colors for a nice, simple color map
const colorRamp = (t) => (`rgb(
	${Math.round(255 * Math.sqrt(t))},
	${Math.round(255 * t * t * t)},
	${Math.round(255 * Math.max(0, Math.sin(2 * Math.PI * t)))}`);