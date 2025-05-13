import { ArtistMetric } from "./artist-metric";

export interface ArtistWithMetrics {
  guid: string;
  name: string;
  pictureMediumUrl: string
  artistMetrics: ArtistMetric[]
}
