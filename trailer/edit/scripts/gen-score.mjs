// Synthesises the trailer score: 96 BPM, Am-F-C-G, one chord per bar.
// Usage: node scripts/gen-score.mjs public/score.wav
import { writeFileSync } from 'node:fs';

const SR = 44100;
const BPM = 96;
const BEAT = 60 / BPM;
const BARS = Number(process.argv[3] ?? 20);
if (!Number.isInteger(BARS) || BARS < 4 || BARS > 100) throw new Error('Bars must be an integer from 4 to 100');
const LEN = BARS * 4 * BEAT + 1.5;
const L = new Float32Array(Math.ceil(LEN * SR));
const R = new Float32Array(L.length);

const hz = (m) => 440 * Math.pow(2, (m - 69) / 12);
const chords = [
  [57, 60, 64], // Am
  [53, 57, 60], // F
  [48, 55, 64], // C
  [55, 59, 62], // G
];

function add(t, dur, fn, gain, pan = 0) {
  const s0 = Math.floor(t * SR);
  const n = Math.floor(dur * SR);
  for (let i = 0; i < n && s0 + i < L.length; i++) {
    const v = fn(i / SR, i / n) * gain;
    L[s0 + i] += v * (1 - Math.max(0, pan));
    R[s0 + i] += v * (1 + Math.min(0, pan));
  }
}
const env = (p, a = 0.02) => (p < a ? p / a : Math.pow(1 - (p - a) / (1 - a), 2));

for (let bar = 0; bar < BARS; bar++) {
  const t0 = bar * 4 * BEAT;
  const ch = chords[bar % 4];
  const swell = Math.min(1, 0.45 + bar * 0.05);

  for (const m of ch) {
    add(t0, 4 * BEAT + 0.4, (t, p) => {
      const e = Math.sin(Math.PI * Math.min(1, p * 1.05));
      return (Math.sin(2 * Math.PI * hz(m) * t) + 0.4 * Math.sin(2 * Math.PI * hz(m) * 2.003 * t)) * e;
    }, 0.05 * swell, (m % 3 - 1) * 0.4);
  }
  add(t0, 4 * BEAT, (t, p) => Math.sin(2 * Math.PI * hz(ch[0] - 12) * t) * Math.sin(Math.PI * p), 0.12 * swell);

  if (bar >= 2 && bar < BARS - 1) {
    for (let b = 0; b < 4; b++) {
      add(t0 + b * BEAT, 0.25, (t, p) => Math.sin(2 * Math.PI * (48 * Math.exp(-t * 18) + 42) * t) * env(p, 0.005), 0.35);
    }
  }
  if (bar >= 8 && bar < BARS - 1) {
    for (let b = 0; b < 4; b++) {
      add(t0 + (b + 0.5) * BEAT, 0.05, (t, p) => (Math.random() * 2 - 1) * env(p, 0.01), 0.05, 0.3);
    }
  }
  if (bar >= 4 && bar < BARS - 1) {
    for (let k = 0; k < 8; k++) {
      const m = ch[k % 3] + 12 + (k % 4 === 3 ? 12 : 0);
      add(t0 + k * BEAT / 2, 0.4, (t, p) => Math.sin(2 * Math.PI * hz(m) * t) * env(p, 0.004), 0.06, k % 2 ? 0.35 : -0.35);
    }
  }
}

let peak = 0;
for (let i = 0; i < L.length; i++) peak = Math.max(peak, Math.abs(L[i]), Math.abs(R[i]));
const norm = 0.8 / peak;
const fade = Math.floor(1.5 * SR);
const data = Buffer.alloc(L.length * 4);
for (let i = 0; i < L.length; i++) {
  const f = i > L.length - fade ? (L.length - i) / fade : 1;
  data.writeInt16LE(Math.round(L[i] * norm * f * 32767), i * 4);
  data.writeInt16LE(Math.round(R[i] * norm * f * 32767), i * 4 + 2);
}
const h = Buffer.alloc(44);
h.write('RIFF', 0); h.writeUInt32LE(36 + data.length, 4); h.write('WAVEfmt ', 8);
h.writeUInt32LE(16, 16); h.writeUInt16LE(1, 20); h.writeUInt16LE(2, 22);
h.writeUInt32LE(SR, 24); h.writeUInt32LE(SR * 4, 28); h.writeUInt16LE(4, 32); h.writeUInt16LE(16, 34);
h.write('data', 36); h.writeUInt32LE(data.length, 40);
writeFileSync(process.argv[2] ?? 'score.wav', Buffer.concat([h, data]));
console.log('score written,', LEN.toFixed(1), 's');
