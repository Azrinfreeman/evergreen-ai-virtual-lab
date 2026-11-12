import { Composition } from 'remotion';
import { Trailer, FPS, TOTAL_SECONDS } from './Trailer';
import { ReadmeLoop, LOOP_FPS, LOOP_SECONDS } from './ReadmeLoop';
import { PlayerJourneyTeaser, TEASER_FPS, TEASER_SECONDS } from './PlayerJourneyTeaser';

export const Root = () => (
  <>
    <Composition
      id="PlayerJourneyTeaser"
      component={PlayerJourneyTeaser}
      durationInFrames={Math.round(TEASER_SECONDS * TEASER_FPS)}
      fps={TEASER_FPS}
      width={1920}
      height={1080}
    />
    <Composition
      id="Trailer"
      component={Trailer}
      durationInFrames={Math.round(TOTAL_SECONDS * FPS)}
      fps={FPS}
      width={1920}
      height={1080}
    />
    <Composition
      id="ReadmeLoop"
      component={ReadmeLoop}
      durationInFrames={LOOP_SECONDS * LOOP_FPS}
      fps={LOOP_FPS}
      width={1280}
      height={720}
    />
  </>
);
