// Writes a soft two-note UI cue for replies and completed drops.
// Usage: node scripts/gen-cue.mjs public/reply-cue.wav
import { writeFileSync } from 'node:fs';

const sampleRate = 44100;
const duration = 0.42;
const frames = Math.ceil(sampleRate * duration);
const samples = Buffer.alloc(frames * 4);

for (let i = 0; i < frames; i++) {
  const t = i / sampleRate;
  const local = t < 0.18 ? t : t - 0.18;
  const frequency = t < 0.18 ? 659.25 : 987.77;
  const attack = Math.min(1, local / 0.008);
  const decay = Math.exp(-local * 13);
  const value = Math.sin(2 * Math.PI * frequency * local) * attack * decay * 0.32;
  const pcm = Math.round(value * 32767);
  samples.writeInt16LE(pcm, i * 4);
  samples.writeInt16LE(pcm, i * 4 + 2);
}

const header = Buffer.alloc(44);
header.write('RIFF', 0);
header.writeUInt32LE(36 + samples.length, 4);
header.write('WAVEfmt ', 8);
header.writeUInt32LE(16, 16);
header.writeUInt16LE(1, 20);
header.writeUInt16LE(2, 22);
header.writeUInt32LE(sampleRate, 24);
header.writeUInt32LE(sampleRate * 4, 28);
header.writeUInt16LE(4, 32);
header.writeUInt16LE(16, 34);
header.write('data', 36);
header.writeUInt32LE(samples.length, 40);
writeFileSync(process.argv[2] ?? 'reply-cue.wav', Buffer.concat([header, samples]));
console.log('reply cue written');
