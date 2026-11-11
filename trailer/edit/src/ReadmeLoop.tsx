import { AbsoluteFill, OffthreadVideo, Sequence, staticFile } from 'remotion';

export const LOOP_FPS = 30;
export const LOOP_SECONDS = 10;
const VIDEO_W = 1114;
const VIDEO_H = 626;

// Silent 10-second loop for the README. The disclosure stays on screen because
// the exchange comes from the app's scripted demo mode, not a live API call.
export const ReadmeLoop = () => (
  <AbsoluteFill style={{ background: '#0B0B3B' }}>
    <Sequence
      from={0}
      durationInFrames={3 * LOOP_FPS}
      style={{ left: 83, width: VIDEO_W, height: VIDEO_H, overflow: 'hidden' }}
    >
      <OffthreadVideo
        src={staticFile('tutor_on_1.mp4')}
        startFrom={20 * LOOP_FPS}
        muted
        style={{ width: VIDEO_W, height: VIDEO_H }}
      />
    </Sequence>
    <Sequence
      from={3 * LOOP_FPS}
      durationInFrames={7 * LOOP_FPS}
      style={{ left: 83, width: VIDEO_W, height: VIDEO_H, overflow: 'hidden' }}
    >
      <OffthreadVideo
        src={staticFile('tutor_on_1.mp4')}
        startFrom={24 * LOOP_FPS}
        muted
        style={{ width: VIDEO_W, height: VIDEO_H, transform: 'scale(1.3)', transformOrigin: '50% 45%' }}
      />
    </Sequence>
    <AbsoluteFill style={{ top: VIDEO_H, height: 94, justifyContent: 'center', paddingLeft: 83 }}>
      <div style={{ borderLeft: '6px solid #4FC3F7', paddingLeft: 20, color: '#fff', fontFamily: '"Segoe UI", Arial, sans-serif' }}>
        <div style={{ fontSize: 28, fontWeight: 600 }}>Built-in tutor, powered by OpenAI</div>
        <div style={{ fontSize: 16, color: '#C9D3E6', marginTop: 5 }}>
          Scripted exchange for this preview; the app uses OpenAI with your own key.
        </div>
      </div>
      <div style={{ position: 'absolute', right: 83, top: 22, background: '#3D5AFE', color: '#fff', padding: '10px 18px', fontFamily: '"Segoe UI", Arial, sans-serif', fontSize: 20, fontWeight: 700 }}>
        OpenAI gpt-6-luna
      </div>
    </AbsoluteFill>
  </AbsoluteFill>
);
