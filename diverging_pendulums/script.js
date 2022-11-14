let canvas, ctx;
let width, height, scl, hW, hH;

// lists of magnets and stuff
let magnets = [
	new Vector(1, 0),
	new Vector(-0.5, Math.sqrt(0.75)),
	new Vector(-0.5, -Math.sqrt(0.75))];
let pendulums = [];

// parameters and integrator variables
const mu = 0.04, g = 0.2, h = 0.22, dt = 0.02;
const numPends = 20, step = 0.001;

document.addEventListener("DOMContentLoaded", function() {
	canvas = document.getElementById("canvas");
	ctx = canvas.getContext("2d");

	// scaling constants
	width = canvas.width = window.innerWidth;
	height = canvas.height = window.innerHeight;
	hW = width / 2;
	hH = height / 2;
	scl = height / 4;

	// set canvas properties and initialize
	ctx.translate(hW, hH);
	ctx.fillStyle = ctx.strokeStyle = "white";
	addPends();
	
	loop();
});

function addPends() {
	// add pendulums
	let x = 1; 
	for (let i = 1; i <= numPends; i ++) {
		pendulums.push(new Pendulum(new Vector(1, x), colorRamp(i/20)));
		x += step;
	}
}

function updatePends() {
	let aN, f, k, r, s;
	// use Beeman integration to solve for bob position
	for (let p of pendulums) {
		// first half of integrator
		p.lP = p.p;
		p.p = p.p.add(p.v.scale(dt).add((p.a.scale(4).sub(p.aP)).scale(dt * dt / 6)));
		
		// calculate acceleration from magnets
		aN = new Vector(0, 0);
		for (let m of magnets) {
			r = m.sub(p.p);
			s = (h * h + r.dot(r)) ** 1.5;

			aN = aN.add(r.div(s));
		}

		// calculate other forces (spring and gravity)
		f = p.v.scale(mu);
		k = p.p.scale(g);
		aN = aN.sub(f.add(k));

		// second half of integrator
		p.v = p.v.add((aN.scale(2).add(p.a.scale(5).sub(p.aP))).scale(dt / 6));
		p.aP = p.a;
		p.a = aN;

		// update path and draw
		p.path.push(p.p);
		p.path = p.path.slice(-200);
		p.draw();
	}
}

function showMags() {
	// Show magnets on screen
	ctx.fillStyle = "white";
	for (let m of magnets) {
		ctx.beginPath();
		ctx.arc(scl * m.x, scl * m.y, 15, 0, 2 * Math.PI);
		ctx.fill();
	}
}

function loop() {
	// Main animation loop
	ctx.clearRect(-hW, -hH, width, height);
	showMags();
	updatePends();
	window.requestAnimationFrame(loop);
}