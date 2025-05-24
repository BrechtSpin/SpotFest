import { ArtistMetric } from "./artist-metric";
import { HappeningSummary } from "./happening-summary";

export interface ArtistWithMetrics {
  guid: string;
  name: string;
  pictureMediumUrl: string
  artistMetrics: ArtistMetric[]
  artistHappenings: HappeningSummary[]
}
