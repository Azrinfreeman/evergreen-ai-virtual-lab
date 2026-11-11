import {
  AbsoluteFill,
  Audio,
  OffthreadVideo,
  Sequence,
  interpolate,
  staticFile,
  useCurrentFrame,
} from 'remotion';

export const FPS = 60;
const BPM = 96;
const BEAT = 60 / BPM; // 0.625 s; every cut below lands on a beat
const BAR = BEAT * 4;
export const TOTAL_SECONDS = BAR * 20;

const NAVY = '#0B0B3B';
const BLUE = '#3D5AFE';
const CYAN = '#4FC3F7';
const FONT = '"Segoe UI", "Liberation Sans", Arial, sans-serif';

type Clip = {
  src: string;
  from: number; // start time inside the take, seconds
  at: number; // start time on the timeline, in bars
  bars: number;
  rate: number; // playback speed; source seconds used = bars * BAR * rate
  zoom?: number; // optional punch-in on the centre of the frame
};

const CLIPS: Clip[] = [
  { src: 'akt4_drag_1.mp4', from: 0, at: 0, bars: 2, rate: 1.8 }, // login, dashboard, activity menu
  { src: 'akt4_drag_1.mp4', from: 12, at: 2, bars: 2, rate: 1.6 }, // lesson, diagram, safety rules
  { src: 'akt4_drag_1.mp4', from: 21, at: 4, bars: 4, rate: 1.2 }, // label the tubes
  { src: 'quiz_1.mp4', from: 10.5, at: 8, bars: 3, rate: 1.4 }, // quiz
  { src: 'tutor_on_1.mp4', from: 17, at: 11, bars: 1.4, rate: 2 }, // open tutor, type, send
  { src: 'tutor_on_1.mp4', from: 24, at: 12.4, bars: 1.8, rate: 1, zoom: 1.3 }, // reply
  { src: 'tutor_off_1.mp4', from: 23.5, at: 14.2, bars: 1.8, rate: 1.5, zoom: 1.3 }, // off-topic question, declined
];

type Caption = { at: number; bars: number; title: string; sub?: string; badge?: string };

// Claims follow docs/trailer/IMPLEMENTATION.md section 2.2.
const CAPTIONS: Caption[] = [
  { at: 0, bars: 2, title: 'Evergreen STEM Virtual Lab', sub: 'Seven science activities, in Malay' },
  { at: 2, bars: 2, title: 'Read the lesson first' },
  { at: 4, bars: 4, title: 'Then run the procedure', sub: 'Drag each label onto its tube' },
  { at: 8, bars: 3, title: 'Check yourself with a short quiz' },
  { at: 11, bars: 5, title: 'Built-in tutor, powered by OpenAI', sub: 'It knows which activity you opened, and stays on science', badge: 'OpenAI  gpt-6-luna' },
];

const DIAGRAM_AT = 16;
const END_AT = 18;

type SoundCue = { src: string; at: number; volume: number; frames: number };
const SOUND_CUES: SoundCue[] = [
  { src: 'touchglass.wav', at: 0.94, volume: 1.2, frames: 30 }, // login
  { src: 'touchglass.wav', at: 3.06, volume: 1.2, frames: 30 }, // activity card
  { src: 'touchglass.wav', at: 13.08, volume: 1.2, frames: 30 }, // first label drop
  { src: 'touchglass.wav', at: 15.17, volume: 1.2, frames: 30 }, // second label drop
  { src: 'reply-cue.wav', at: 15.3, volume: 0.55, frames: 30 }, // tubes labelled
  { src: 'yay.mp3', at: 24.5, volume: 0.22, frames: 90 }, // correct quiz answer
  { src: 'touchglass.wav', at: 28.25, volume: 1.2, frames: 30 }, // send tutor question
  { src: 'reply-cue.wav', at: 29.0, volume: 0.55, frames: 30 }, // tutor reply
  { src: 'reply-cue.wav', at: 36.0, volume: 0.4, frames: 30 }, // off-topic decline
];

const scoreVolume = (frame: number) => {
  // Leave headroom for the game's sounds and duck under the quiz cheer.
  return interpolate(frame / FPS, [24.3, 24.5, 25.6, 25.9], [0.8, 0.3, 0.3, 0.8], {
    extrapolateLeft: 'clamp',
    extrapolateRight: 'clamp',
  });
};

const useFade = (startBars: number, bars: number) => {
  const f = useCurrentFrame();
  const s = startBars * BAR * FPS;
  const e = (startBars + bars) * BAR * FPS;
  return interpolate(f, [s, s + 9, e - 9, e], [0, 1, 1, 0], {
    extrapolateLeft: 'clamp',
    extrapolateRight: 'clamp',
  });
};

const VIDEO_W = 1671; // 1920x1080 footage scaled to leave a caption strip
const VIDEO_H = 940;

const CaptionBar = ({ c }: { c: Caption }) => {
  const o = useFade(c.at, c.bars);
  return (
    <AbsoluteFill style={{ top: VIDEO_H, height: 1080 - VIDEO_H, justifyContent: 'center' }}>
      <div
        style={{
          opacity: o,
          marginLeft: (1920 - VIDEO_W) / 2,
          borderLeft: `8px solid ${CYAN}`,
          paddingLeft: 28,
          fontFamily: FONT,
          color: '#fff',
        }}
      >
        <div style={{ fontSize: 44, fontWeight: 600, lineHeight: 1.2 }}>{c.title}</div>
        {c.sub && <div style={{ fontSize: 28, color: '#C9D3E6', marginTop: 4 }}>{c.sub}</div>}
      </div>
      {c.badge && (
        <div style={{ position: 'absolute', right: (1920 - VIDEO_W) / 2, opacity: o, background: BLUE, color: '#fff', fontFamily: FONT, fontSize: 34, fontWeight: 700, padding: '14px 28px' }}>
          {c.badge}
        </div>
      )}
    </AbsoluteFill>
  );
};

type Step = { head: string; body: string; hero?: boolean };
const STEPS: Step[] = [
  { head: 'Open an activity', body: 'The app records the topic, for example MM4' },
  { head: 'Ask a question', body: 'Your text is sent with that topic\u2019s prompt' },
  { head: 'OpenAI', body: 'Model gpt-6-luna via the chat completions API, called from Unity C#', hero: true },
  { head: 'Reply in the panel', body: 'Shown beside your lesson' },
];

const Diagram = () => {
  const f = useCurrentFrame();
  const local = f - DIAGRAM_AT * BAR * FPS;
  const o = useFade(DIAGRAM_AT, 2);
  return (
    <AbsoluteFill style={{ background: '#F2F6FA', opacity: o, fontFamily: FONT, color: NAVY }}>
      <div style={{ position: 'absolute', top: 90, left: 120, fontSize: 56, fontWeight: 700 }}>How the tutor gets its context</div>
      <div style={{ position: 'absolute', top: 360, left: 120, right: 120, display: 'flex', alignItems: 'stretch', gap: 24 }}>
        {STEPS.map((s, i) => {
          const start = i * 0.5 * BEAT * FPS * 2;
          const show = interpolate(local, [start, start + 12], [0, 1], { extrapolateLeft: 'clamp', extrapolateRight: 'clamp' });
          return (
            <div key={s.head} style={{ display: 'flex', alignItems: 'center', gap: 24, flex: 1, opacity: show }}>
              <div style={{ background: s.hero ? BLUE : '#fff', color: s.hero ? '#fff' : NAVY, border: `3px solid ${NAVY}`, padding: 28, flex: s.hero ? 1.5 : 1, minHeight: 260 }}>
                <div style={{ fontSize: 28, color: s.hero ? '#fff' : BLUE, fontWeight: 700 }}>{i + 1}</div>
                <div style={{ fontSize: s.hero ? 56 : 38, fontWeight: 700, margin: '8px 0 12px' }}>{s.head}</div>
                <div style={{ fontSize: 28, color: s.hero ? '#fff' : '#46556B', lineHeight: 1.35 }}>{s.body}</div>
              </div>
              {i < STEPS.length - 1 && <div style={{ fontSize: 56, color: NAVY }}>{'\u2192'}</div>}
            </div>
          );
        })}
      </div>
      <div style={{ position: 'absolute', bottom: 110, left: 120, fontSize: 32, color: '#46556B' }}>
        It knows the activity topic. It does not see your screen or your progress.
      </div>
    </AbsoluteFill>
  );
};

const EndCard = () => {
  const f = useCurrentFrame();
  const o = interpolate(f, [END_AT * BAR * FPS, END_AT * BAR * FPS + 15], [0, 1], { extrapolateLeft: 'clamp', extrapolateRight: 'clamp' });
  return (
    <AbsoluteFill style={{ background: NAVY, opacity: o, fontFamily: FONT, color: '#fff', justifyContent: 'center', alignItems: 'center' }}>
      <div style={{ fontSize: 88, fontWeight: 700 }}>Evergreen STEM Virtual Lab</div>
      <div style={{ fontSize: 36, color: '#fff', marginTop: 28 }}>Tutor powered by OpenAI gpt-6-luna (chat completions)</div>
      <img
        src={staticFile('openai-wordmark.png')}
        alt="OpenAI"
        style={{ width: 240, height: 120, marginTop: 18, objectFit: 'contain' }}
      />
      <div style={{ fontSize: 40, color: CYAN, marginTop: 8 }}>github.com/Azrinfreeman/evergreen-ai-virtual-lab</div>
      <div style={{ position: 'absolute', bottom: 54, fontSize: 26, color: '#C9D3E6', maxWidth: 1500, textAlign: 'center' }}>
        Tutor exchange shown is scripted for this trailer; the app uses OpenAI with your own key.
        <div style={{ fontSize: 20, marginTop: 8 }}>OpenAI wordmark is a trademark of OpenAI.</div>
      </div>
    </AbsoluteFill>
  );
};

export const Trailer = () => (
  <AbsoluteFill style={{ background: NAVY }}>
    {CLIPS.map((c, i) => (
      <Sequence key={i} from={Math.round(c.at * BAR * FPS)} durationInFrames={Math.round(c.bars * BAR * FPS)} style={{ left: (1920 - VIDEO_W) / 2, top: 0, width: VIDEO_W, height: VIDEO_H, overflow: 'hidden' }}>
        <OffthreadVideo
          src={staticFile(c.src)}
          startFrom={Math.round(c.from * FPS)}
          playbackRate={c.rate}
          muted
          style={{ width: VIDEO_W, height: VIDEO_H, transform: `scale(${c.zoom ?? 1})`, transformOrigin: '50% 45%' }}
        />
      </Sequence>
    ))}
    {CAPTIONS.map((c) => (
      <CaptionBar key={c.title} c={c} />
    ))}
    <Diagram />
    <EndCard />
    <Audio src={staticFile('score.wav')} volume={scoreVolume} />
    {SOUND_CUES.map((cue, i) => (
      <Sequence key={i} from={Math.round(cue.at * FPS)} durationInFrames={cue.frames}>
        <Audio src={staticFile(cue.src)} volume={cue.volume} />
      </Sequence>
    ))}
  </AbsoluteFill>
);




