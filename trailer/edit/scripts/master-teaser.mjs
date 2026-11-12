// Package a Remotion export with measured audio and standard limited-range H.264.
// Usage: node scripts/master-teaser.mjs <render-source.mp4> <delivery.mp4>
import {spawnSync} from 'node:child_process';
import {existsSync, writeFileSync} from 'node:fs';
import {resolve} from 'node:path';

const [inputArg, outputArg] = process.argv.slice(2);
if (!inputArg || !outputArg) throw new Error('Provide input and output MP4 paths.');
const input = resolve(inputArg);
const output = resolve(outputArg);
if (!existsSync(input) || input === output || existsSync(output)) {
  throw new Error('Input must exist and output must be a different, unused path.');
}

function ffmpeg(args) {
  const result = spawnSync('ffmpeg', ['-hide_banner', ...args], {
    encoding: 'utf8', windowsHide: true, maxBuffer: 8 * 1024 * 1024,
  });
  if (result.error) throw result.error;
  if (result.status !== 0) throw new Error(result.stderr.slice(-4000));
  return result.stderr;
}

function measure(path) {
  const log = ffmpeg(['-i', path, '-vn', '-af',
    'loudnorm=I=-17:TP=-1.5:LRA=11:print_format=json', '-f', 'null', '-']);
  const match = log.match(/\{\s*"input_i"[\s\S]*?\}/);
  if (!match) throw new Error('ffmpeg did not return a loudness measurement.');
  const measurement = JSON.parse(match[0]);
  for (const key of ['input_i', 'input_tp', 'input_lra', 'input_thresh', 'target_offset']) {
    if (!Number.isFinite(Number(measurement[key]))) throw new Error(`Invalid loudness field: ${key}`);
  }
  return measurement;
}

const before = measure(input);
const probe = spawnSync('ffprobe', ['-v', 'error', '-select_streams', 'v:0',
  '-show_entries', 'stream=duration', '-of', 'json', input], {encoding: 'utf8', windowsHide: true});
if (probe.error || probe.status !== 0) throw new Error('Could not measure the video duration.');
const videoDuration = Number(JSON.parse(probe.stdout).streams?.[0]?.duration);
if (!Number.isFinite(videoDuration) || videoDuration <= 0) throw new Error('Invalid video duration.');
const filter = 'loudnorm=I=-17:TP=-1.5:LRA=11' +
  `:measured_I=${before.input_i}:measured_TP=${before.input_tp}` +
  `:measured_LRA=${before.input_lra}:measured_thresh=${before.input_thresh}` +
  `:offset=${before.target_offset}:linear=true`;
ffmpeg(['-n', '-i', input, '-map', '0:v:0', '-map', '0:a:0',
  '-vf', 'scale=in_range=auto:out_range=tv,format=yuv420p',
  '-c:v', 'libx264', '-preset', 'medium', '-crf', '16',
  '-color_range', 'tv', '-colorspace', 'bt709', '-color_primaries', 'bt709', '-color_trc', 'bt709',
  '-af', filter, '-c:a', 'aac', '-b:a', '192k', '-ar', '48000',
  '-t', String(videoDuration), '-movflags', '+faststart', output]);
const after = measure(output);
writeFileSync(`${output}.loudness.json`, JSON.stringify({input, output, before, after}, null, 2) + '\n');
if (Number(after.input_i) < -18 || Number(after.input_i) > -16 || Number(after.input_tp) > -1) {
  throw new Error(`Review audio before delivery: ${after.input_i} LUFS, ${after.input_tp} dBTP.`);
}
console.log(`Mastered: ${output}\n${after.input_i} LUFS; ${after.input_tp} dBTP`);
