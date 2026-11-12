import {
  AbsoluteFill, Audio, OffthreadVideo, Sequence, interpolate, staticFile,
  useCurrentFrame, useVideoConfig,
} from 'remotion';

export const TEASER_FPS = 60;
const NAVY = '#0B0B3B';
const CYAN = '#4FC3F7';
const FONT = '"Segoe UI", "Liberation Sans", Arial, sans-serif';

type Shot = {
  name: string;
  source: number; // Seconds in journey-01.mp4; see the transcript and source marks.
  seconds: number;
  caption: string;
  zoom?: number;
  demo?: boolean;
};

// One table owns the edit. All footage plays at normal speed; navigation is visibly cut.
// Source is the app's actual scripted demo output. Never replace its reply text here.
export const TEASER_SHOTS: Shot[] = [
  {name: 'Opening preview', source: 37.5, seconds: 2, caption: 'Makmal sains maya · Evergreen STEM'},
  {name: 'Enter the lab', source: 1.5, seconds: 2.5, caption: 'Masuk makmal'},
  {name: 'Choose an activity', source: 5.5, seconds: 2.5, caption: 'Pilih aktiviti'},
  {name: 'Read the lesson', source: 13.8, seconds: 2.5, caption: 'Fahami konsepnya'},
  {name: 'Label the plasmid tube', source: 35.5, seconds: 6, caption: 'Cuba sendiri, langkah demi langkah'},
  {name: 'Open, type and send', source: 42.4, seconds: 11.4, caption: 'Perlukan penjelasan? Tanya pembantu.', demo: true},
  {name: 'Read the restriction enzyme answer', source: 54, seconds: 13, caption: 'Pembantu pembelajaran · Jawapan demo', demo: true, zoom: 1.28},
  {name: 'Ask the follow-up', source: 69.7, seconds: 6.8, caption: 'Teruskan dengan soalan susulan', demo: true},
  {name: 'Read the ligase answer', source: 76.7, seconds: 10, caption: 'Pembantu pembelajaran · Jawapan demo', demo: true, zoom: 1.28},
  {name: 'Resume the lab', source: 91, seconds: 5.5, caption: 'Kembali ke eksperimen'},
  {name: 'Answer and see the score', source: 111.5, seconds: 7, caption: 'Uji kefahaman anda'},
];

export const TEASER_END_AT = TEASER_SHOTS.reduce((sum, shot) => sum + shot.seconds, 0);
export const TEASER_SECONDS = TEASER_END_AT + 5;

const Caption = ({text, demo}: {text: string; demo?: boolean}) => (
  <AbsoluteFill style={{top: 952, height: 128, backgroundColor: NAVY, padding: '20px 72px',
    fontFamily: FONT, color: 'white', justifyContent: 'center'}}>
    <div style={{fontSize: 34, fontWeight: 600, lineHeight: 1.25}}>{text}</div>
    {demo && <div style={{fontSize: 23, color: CYAN, marginTop: 7}}>
      Contoh perbualan berskrip · Tiada sambungan OpenAI
    </div>}
  </AbsoluteFill>
);

const FilmedShot = ({shot}: {shot: Shot}) => {
  const {fps} = useVideoConfig();
  return <>
    <AbsoluteFill style={{height: 952, overflow: 'hidden'}}>
      <OffthreadVideo src={staticFile('player-journey/journey-01.mp4')}
        startFrom={Math.round(shot.source * fps)} muted
        style={{position: 'absolute', width: 1692, height: 952, left: 114, top: 0,
          transform: `scale(${shot.zoom ?? 1})`, transformOrigin: '50% 42%'}} />
    </AbsoluteFill>
    <Caption text={shot.caption} demo={shot.demo} />
  </>;
};

const EndCard = () => {
  const frame = useCurrentFrame();
  return <AbsoluteFill style={{backgroundColor: NAVY, fontFamily: FONT, color: 'white',
    padding: '180px 160px', justifyContent: 'center',
    opacity: interpolate(frame, [0, 8], [0, 1], {extrapolateRight: 'clamp'})}}>
    <div style={{fontSize: 32, color: CYAN, marginBottom: 24}}>MAKMAL MAYA STEM</div>
    <div style={{fontSize: 100, fontWeight: 700}}>Evergreen</div>
    <div style={{fontSize: 48, marginTop: 24}}>Belajar. Cuba. Tanya. Uji diri.</div>
    <div style={{fontSize: 28, marginTop: 58}}>github.com/Azrinfreeman/evergreen-ai-virtual-lab</div>
    <div style={{fontSize: 30, marginTop: 58, maxWidth: 1500, lineHeight: 1.5}}>
      Jawapan pembantu yang ditunjukkan ialah contoh berskrip.<br />
      Demo ini tidak memerlukan akaun OpenAI.
    </div>
  </AbsoluteFill>;
};

const Music = () => {
  const {fps, durationInFrames} = useVideoConfig();
  return <Audio src={staticFile('player-journey/score.wav')} volume={(frame) => {
    const t = frame / fps;
    const duck = t >= 15.5 && t < 59.2 ? 0.45 : 0.72;
    const fade = interpolate(frame, [0, fps, durationInFrames - 2 * fps, durationInFrames],
      [0, 1, 1, 0], {extrapolateLeft: 'clamp', extrapolateRight: 'clamp'});
    return duck * fade;
  }} />;
};

export const PlayerJourneyTeaser = () => {
  const {fps} = useVideoConfig();
  let at = 0;
  return <AbsoluteFill style={{backgroundColor: NAVY}}>
    {TEASER_SHOTS.map((shot) => {
      const from = Math.round(at * fps);
      at += shot.seconds;
      return <Sequence key={shot.name} name={shot.name} from={from}
        durationInFrames={Math.round(at * fps) - from}>
        <FilmedShot shot={shot} />
      </Sequence>;
    })}
    <Sequence name="Title and demo disclosure" from={Math.round(TEASER_END_AT * fps)}
      durationInFrames={5 * fps}><EndCard /></Sequence>
    <Music />
  </AbsoluteFill>;
};
